using System;
using System.Collections.Generic;
using System.ComponentModel;
using Foundation;
using Microsoft.AppCenter.Crashes;
using ObjCRuntime;
using QuranicQuizzes.CustomControls;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.iOS.Helpers;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(AuthroiseWebView), typeof(iosWebViewRenderer))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class iosWebViewRenderer : ViewRenderer<AuthroiseWebView, WKWebView>
    {
        WKWebView webView;
        private NSMutableUrlRequest webRequest;

        protected override void OnElementChanged(ElementChangedEventArgs<AuthroiseWebView> e)
        {
            base.OnElementChanged(e);
            try
            {

                if (Control == null)
                {

                    var preferences = new WKPreferences();
                    preferences.JavaScriptEnabled = true;
                    preferences.JavaScriptCanOpenWindowsAutomatically = true;
                   

                    var configuration = new WKWebViewConfiguration();
                    configuration.Preferences = preferences;
                    configuration.AllowsInlineMediaPlayback = true;
                    configuration.MediaPlaybackRequiresUserAction = false;
                    configuration.RequiresUserActionForMediaPlayback = true;
                    
                    webView = new WKWebView(Frame, configuration);
                    
                    // webView.NavigationDelegate = new DisplayLinkWebViewDelegate(Element, webRequest);
                    SetNativeControl(webView);
                }
                if (e.NewElement != null)
                {

                   // Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
                    
                    SetNativeControl(webView);
                }
                if (Element != null)
                {
                    var headerKey = new NSString("Authorization");
                    var headerValue = new NSString(UserSettings.AccesToken);
                    var dictionary = new NSDictionary(headerKey, headerValue);
                    UrlWebViewSource source = (Xamarin.Forms.UrlWebViewSource)Element.Source;
                    webRequest = new NSMutableUrlRequest(new NSUrl(source.Url));
                    webRequest.Headers = dictionary;

                    webView.NavigationDelegate = new DisplayLinkWebViewDelegate(Element, webRequest);
                    webView.AutoresizingMask = UIViewAutoresizing.All;
                    webView.LoadRequest(webRequest);

                    MessagingCenter.Subscribe<object, string>(this, "EndLiveGame", (obj, arg) =>
                    {
                        webView.EvaluateJavaScriptAsync(arg);
                    });
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
            
        }

        

        public class DisplayLinkWebViewDelegate : WKNavigationDelegate
        {
            private AuthroiseWebView element;
            private NSMutableUrlRequest webRequest;
            private bool _headerIsSet;
            public DisplayLinkWebViewDelegate(AuthroiseWebView element, NSMutableUrlRequest webRequest)
            {
                this.element = element;
                this.webRequest = webRequest;
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                //element.InvokeCompleted();
               

                UrlWebViewSource source = (Xamarin.Forms.UrlWebViewSource)element.Source;
                if (source.Url.ToString().Contains("LiveGamesMobile"))
                {
                    var Token = UserSettings.AccesToken.Replace("bearer ", string.Empty);
                    webView.EvaluateJavaScriptAsync(
                "javascript:(startGame(true,'" + Token + "'))");
                }
                else
                {
                    var Token = UserSettings.AccesToken.Replace("bearer ", string.Empty);
                    webView.EvaluateJavaScriptAsync(
                "javascript:(loadData('" + Token + "'))");
                }

               // base.DidFinishNavigation(webView, navigation);
            }

            

            public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
            {
                //element.InvokeStarted();
               //base.DidStartProvisionalNavigation(webView, navigation);

               
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction,Action<WKNavigationActionPolicy> decisionHandler)
            {
                //base.DecidePolicy(webView, navigationAction, decisionHandler);

                var request = navigationAction.Request;
                // check if the header is set and if not, create a muteable copy of the original request
                if (!_headerIsSet && request is NSMutableUrlRequest muteableRequest)
                {
                    // define your custom header name and value
                    var keys = new object[] { "Authorization" };
                    var values = new object[] { UserSettings.AccesToken };
                    var headerDict = NSDictionary.FromObjectsAndKeys(values, keys);
                    // set the headers of the new request to the created dict
                    muteableRequest.Headers = headerDict;
                    _headerIsSet = true;
                    // attempt to load the newly created request
                    webView.LoadRequest(muteableRequest);
                    // abort the old one
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                    // exit this whole method
                    return;
                }
                else
                {
                    _headerIsSet = false;
                    decisionHandler(WKNavigationActionPolicy.Allow);
                }
            }

            public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
            {
                //base.DidFailNavigation(webView, navigation, error);
            }


        }

      
    }
}




