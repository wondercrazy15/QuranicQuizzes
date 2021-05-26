
using System;
using System.Collections.Generic;
using CarouselView.FormsPlugin.iOS;
using Firebase.CloudMessaging;
using Foundation;
using LabelHtml.Forms.Plugin.iOS;
using MediaManager;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.iOS;
using QuranicQuizzes.Views;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AppDelegate))]
namespace QuranicQuizzes.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IMessagingDelegate, IUNUserNotificationCenterDelegate, IRegisterToken
    {
        
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            try
            {
                Rg.Plugins.Popup.Popup.Init();
                global::Xamarin.Forms.Forms.Init();
                CarouselViewRenderer.Init();
                HtmlLabelRenderer.Initialize();
                XF.Material.iOS.Material.Init();
                FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
                CrossMediaManager.Current.Init();
                Firebase.Core.App.Configure();
                LoadApplication(new App(new iOSInitializer()));
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    // iOS 10 or later
                    var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                        Console.WriteLine(granted);
                    });

                    // For iOS 10 display notification (sent via APNS)
                    UNUserNotificationCenter.Current.Delegate = this;

                    // For iOS 10 data message (sent via FCM)
                    Messaging.SharedInstance.Delegate = this;

                }
                else
                {
                    // iOS 9 or before
                    var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                    var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                    UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                }

                //Messaging.SharedInstance.Subscribe("Register");

                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            catch (System.Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url,string sourceApplication, NSObject annotation)
        {
            try
            {
                Console.WriteLine(url);
                //LoadApplication(new App(url.ToString()));

                if (url.ToString().Contains("#access_token"))
                {
                    string path = url.ToString().Substring(0, url.ToString().IndexOf("&token_type="));
                    var access_token = path.Split(new string[] { "#access_token=" }, StringSplitOptions.None);
                    if (access_token != null)
                    {
                        UserSettings.AccesToken = "bearer " + access_token[1];
                        //LoadApplication(new App(new iOSInitializer()));
                        LoadApplication(new App(url.ToString()));
                        // App.Current.MainPage = new NavigationPage(new MainPage());
                    }
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

            /* now store the url somewhere in the app’s context. The url is in the url NSUrl object. The data is in url.Host if the link as a scheme as superduperapp://something_interesting */
            return true;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //Toast.MakeToast("Device id" + deviceToken.ToString()).Show();

        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {

        }


        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {

            try
            {
                //APICall NU = new APICall();
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

                //NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
                //NSDictionary alert = aps.ObjectForKey(new NSString("alert")) as NSDictionary;
                //var notificationTitle = (alert[new NSString("title")] as NSString).ToString();
                //var notificationMessage = (alert[new NSString("body")] as NSString).ToString();
                //var links = (userInfo.ObjectForKey(new NSString("link")) as NSString).ToString();

                if (UIApplication.SharedApplication.ApplicationState.Equals(UIApplicationState.Active))
                {

                    //App.Current.MainPage = new NavigationPage(new HomePage());
                    //MessagingCenter.Send(Xamarin.Forms.Application.Current, "Home", new HomePage());
                    //App is in foreground. Act on it.
                    //App.Current.MainPage = new HomePage();
                    DependencyService.Get<IToast>().Show("New Assignment is available in My Assignments");
                }
                else
                {
                    if (!string.IsNullOrEmpty(UserSettings.UserId))
                    {
                        GlobalConst.IsNotificationTap = true;
                        App.Current.MainPage = new NavigationPage(new MainPage());
                    }
                }

                //LoadApplication(new App("Notification", QuizID, HWID));
                //var d = NU.GetUrl(links);
                //if (d)
                //{
                //    GlobalConst.NotificationUrl = links;
                //    UserSettings.IsnotificationTap = true;
                //    if (UIApplication.SharedApplication.ApplicationState.Equals(UIApplicationState.Active))
                //    {
                //        App.Current.MainPage = new NavigationPage(new HomePage());
                //        //MessagingCenter.Send(Xamarin.Forms.Application.Current, "Home", new HomePage());
                //        //App is in foreground. Act on it.
                //        //App.Current.MainPage = new HomePage();
                //    }
                //    else
                //    {
                //        //app.NotificaionRedirect();
                //        App.Current.MainPage = new NavigationPage(new HomePage());
                //    }
                //}
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
                //LogInfo.ReportErrorInfo(ex.Message, ex.StackTrace, "AppDelegate-DidReceiveRemoteNotification");

            }
        }

        public void RegisterToken()
        {
            if(UserSettings.IsStudent)
            Messaging.SharedInstance.Subscribe(UserSettings.UserId);
        }

        public void RemoveRegisterToken()
        {
            Messaging.SharedInstance.Unsubscribe(UserSettings.UserId);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Register any platform specific implementations
        }
    }
}
