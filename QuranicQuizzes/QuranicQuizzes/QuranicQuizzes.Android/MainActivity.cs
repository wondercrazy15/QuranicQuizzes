using System.Collections.Generic;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CarouselView.FormsPlugin.Android;
using Firebase.Messaging;
using LabelHtml.Forms.Plugin.Droid;
using MediaManager;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Plugin.InAppBilling;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using QuranicQuizzes.Droid;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Views;
using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace QuranicQuizzes.Droid
{
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "quranicquizzes",
    DataHost = "home",
    AutoVerify = true,
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]

    [Activity(Label = "QuranicQuizzes", HardwareAccelerated =true, Icon = "@mipmap/ic_launcher",
        Theme = "@style/MainTheme",WindowSoftInputMode =Android.Views.SoftInput.AdjustResize|Android.Views.SoftInput.StateVisible,
        LaunchMode = Android.Content.PM.LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IRegisterToken
    {
        private string myUri;

        public static Context ActivityContext { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);
                Rg.Plugins.Popup.Popup.Init(this, bundle);
                CarouselViewRenderer.Init();
                Forms.SetFlags("CollectionView_Experimental");
                HtmlLabelRenderer.Initialize();
                global::Xamarin.Forms.Forms.Init(this, bundle);
                UserDialogs.Init(this);
                ActivityContext = this;
                Xamarin.Essentials.Platform.Init(this, bundle);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
                Firebase.FirebaseApp.InitializeApp(this);
                FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
                XF.Material.Droid.Material.Init(this, bundle);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
                CrossMediaManager.Current.Init(this);
                //FirebasePushNotificationManager.ProcessIntent(this, Intent);
                LoadApplication(new App(new AndroidInitializer()));
                if (!string.IsNullOrEmpty(UserSettings.UserId))
                {
                    Bundle bundles = Intent.Extras;
                    if (bundles != null)
                    {
                        var d=bundle.GetString("Notification");
                        DependencyService.Get<IToast>().Show(d);
                        if (!GlobalConst.IsNotificationTap)
                        {
                            //App.Current.MainPage = new NavigationPage(new MainPage());
                            //DependencyService.Get<IToast>().Show("Mainactivity Notification intent called");
                            //Notification();
                            var QuizID = "311";
                            var HWID = "227";
                            LoadApplication(new App("Notification", QuizID, HWID));
                        }
                    }
                }
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
        }
        
        public async void Notification()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AssignmentPage());
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            this.Intent = intent;
            
            if (intent != null && intent.Data != null)
            {
                myUri = intent.Data.ToString();
                string NotificationId = intent.GetStringExtra("Notification");

                var link = intent.GetStringExtra("link");
                
                //Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new HomePage());
                LoadApplication(new App(myUri));
            }
            else if (intent != null)
            {
                //GlobalConst.IsNotificationTap = true;
               // App.Current.MainPage = new NavigationPage(new MainPage());
                if (!GlobalConst.IsNotificationTap)
                {
                    //App.Current.MainPage = new NavigationPage(new MainPage());
                    //DependencyService.Get<IToast>().Show("Mainactivity Notification intent called");
                    
                    var QuizID = "311";
                    var HWID = "227";
                    LoadApplication(new App("Notification", QuizID, HWID));
                }

            }
            //LoadApplication(new App("Notification"));

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void RegisterToken()
        {
            if(!string.IsNullOrEmpty(UserSettings.UserId))
            FirebaseMessaging.Instance.SubscribeToTopic(UserSettings.UserId);
        }

        public void RemoveRegisterToken()
        {
            FirebaseMessaging.Instance.UnsubscribeFromTopic(UserSettings.UserId);
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        protected override void OnPause()
        {
            base.OnPause();
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

