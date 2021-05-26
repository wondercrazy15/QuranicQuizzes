using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Webkit;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.CustomControls;
using QuranicQuizzes.Droid.Helpers;
using QuranicQuizzes.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AWebView = Android.Webkit.WebView;
[assembly: ExportRenderer(typeof(AuthroiseWebView), typeof(WebViewRendererForAndroid))]
namespace QuranicQuizzes.Droid.Helpers
{
    [Obsolete]
    public class WebViewRendererForAndroid : WebViewRenderer
    {
        IWebViewController ElementController => Element;
        public WebViewRendererForAndroid(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                AuthroiseWebView formsWebView = e.NewElement as AuthroiseWebView;
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.LoadWithOverviewMode = true;
                Control.Settings.UseWideViewPort = true;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowContentAccess = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.Settings.DomStorageEnabled = true;
                Control.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.SetSupportZoom(true);
                Control.Settings.DisplayZoomControls = false;
                this.Control.Settings.DomStorageEnabled = true;
                this.Control.VerticalScrollBarEnabled = true;
                Control.SetWebViewClient(new WebClient(this));
            }
        }
        class WebClient : WebViewClient
        {
            WebViewRendererForAndroid _renderer;
            public WebClient(WebViewRendererForAndroid renderer)
            {
                _renderer = renderer ?? throw new ArgumentNullException("renderer");
            }
            public override bool ShouldOverrideUrlLoading(AWebView view, string url)
            {
                try
                {
                    var Token = UserSettings.AccesToken;
                    Dictionary<String, String> headers = new Dictionary<String, String>();
                    headers.Add("Authorization", Token);
                    view.LoadUrl(url, headers);
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
                return true;
            }
            public override void OnReceivedError(AWebView view, IWebResourceRequest request, WebResourceError error)
            {
                base.OnReceivedError(view, request, error);
            }
            public override void OnReceivedHttpError(AWebView view, IWebResourceRequest request, WebResourceResponse errorResponse)
            {
                base.OnReceivedHttpError(view, request, errorResponse);
                var source = new UrlWebViewSource { Url = request.Url.ToString() };
                var args = new WebNavigatedEventArgs(WebNavigationEvent.NewPage, source, request.Url.ToString(), WebNavigationResult.Failure);
                _renderer.ElementController.SendNavigated(args);
            }
            public override void OnReceivedError(AWebView view, [GeneratedEnum] ClientError errorCode, string description, string failingUrl)
            {
                base.OnReceivedError(view, errorCode, description, failingUrl);
                var source = new UrlWebViewSource { Url = failingUrl };
                var args = new WebNavigatedEventArgs(WebNavigationEvent.NewPage, source, failingUrl, WebNavigationResult.Failure);
                _renderer.ElementController.SendNavigated(args);
            }
            public override void OnPageFinished(AWebView view, string url)
            {
                base.OnPageFinished(view, url);
                if (url.Contains("LiveGamesMobile"))
                {
                    var Token = UserSettings.AccesToken.Replace("bearer ", string.Empty);
                    view.LoadUrl(
                "javascript:(startGame(true,'" + Token + "'))");
                }
                else
                {
                    var Token = UserSettings.AccesToken.Replace("bearer ", string.Empty);
                    view.LoadUrl(
                "javascript:(loadData('" + Token + "'))");
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            }

        }
    }
}