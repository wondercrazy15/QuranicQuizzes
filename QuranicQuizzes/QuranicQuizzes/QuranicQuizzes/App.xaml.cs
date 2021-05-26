using System;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.ViewModels;
using QuranicQuizzes.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace QuranicQuizzes
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        IClientAPI clientAPI = new ClientAPI();
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnStart()
        {
            base.OnStart();
            AppCenter.Start("android=a96ff88c-56ae-4808-afd9-68e1896323a9;" +
                     "uwp={Your UWP App secret here};" +
                     "ios=cc1cf700-5cd1-43a4-93f2-16d82d14212a;",
                     typeof(Analytics), typeof(Crashes));
        }

        const int smallWightResolution = 751;
        const int smallHeightResolution = 1335;
        public static int heightDevice = 0;
        void LoadStyles()
        {
            if (IsASmallDevice())
            {
                dictionary.MergedDictionaries.Add(SmallDevicesStyle.SharedInstance);
            }
            else
            {
                dictionary.MergedDictionaries.Add(GeneralDevicesStyle.SharedInstance);
            }
        }

        public static bool IsASmallDevice()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Width (in pixels)
            var width = mainDisplayInfo.Width;
            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            heightDevice = ((int)mainDisplayInfo.Height);
            return (width <= smallWightResolution && height <= smallHeightResolution);
        }
        protected override async void OnInitialized()
        {
            try
            {

                InitializeComponent();
                XF.Material.Forms.Material.Init(this);
                //await NavigationService.NavigateAsync("NavigationPage/Signup_Login_HomePage");
                LoadStyles();
                if (UserSettings.IsLogin)
                {
                    MainPage = new NavigationPage(new MainPage());
                    await NavigationService.NavigateAsync(nameof(MainPage));
                }
                else
                {
                    MainPage = new NavigationPage(new Signup_Login_HomePage());
                    await NavigationService.NavigateAsync(nameof(Signup_Login_HomePage));
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
        public App(string myUri)
        {
            if (myUri.Contains("#access_token"))
            {
                string path = myUri.Substring(0, myUri.IndexOf("&token_type="));
                var access_token = path.Split(new string[] { "#access_token=" }, StringSplitOptions.None);
                if (access_token != null)
                {
                    UserSettings.AccesToken = "bearer " + access_token[1];
                    onAppHome();
                }
            }
            
        }
        public App(string myUri,string QuizID,string HWID)
        {
            
            if (!GlobalConst.IsNotificationTap)
            {
                
                GlobalConst.IsNotificationTap = true;
                if (myUri.Equals("Notification"))
                {
                    onNotificationTap(QuizID, HWID);
                }
                
            }
        }

        private async void onNotificationTap(string QuizID, string HWID)
        {
            try
            {
                var parameters = new NavigationParameters();
                parameters.Add("QuizID", QuizID);
                parameters.Add("HWID", HWID);
                GlobalConst.IsNotificationTap = false;
                await Current.MainPage.Navigation.PushAsync(new AssignmentPage());

                //MainPage = new NavigationPage(new MainPage());
                //await NavigationService.NavigateAsync("MainPage");
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


        private async void onAppHome()
        {
            try
            {
                //var data=DependencyService.Get<IClientAPI>();
                var UserData = await clientAPI.GetUserInfo();
                if (UserData != null)
                {
                    UserSettings.UserId = UserData.UserID;
                    UserSettings.Name = UserData.Name;
                    UserSettings.IsAdmin = UserData.IsAdmin;
                    UserSettings.IsEnrolled = UserData.IsEnrolled;
                    UserSettings.IsPaidMember = UserData.IsPaidMember;
                    UserSettings.IsStudent = UserData.IsStudent;
                    UserSettings.IsTeacher = UserData.IsTeacher;
                    UserSettings.IsFamilyAdmin = UserData.IsFamilyAdmin;
                    UserSettings.IsInstituteAdmin = UserData.IsInstituteAdmin;
                    UserSettings.Logo = GlobalConst.ApiUrl + UserData.Logo;
                    DependencyService.Get<IRegisterToken>().RegisterToken();
                    UserSettings.IsLogin = true;
                    MainPage = new NavigationPage(new MainPage());
                    await NavigationService.NavigateAsync(nameof(MainPage));
                }
                else
                {
                    UserSettings.IsLogin = false;
                    var alertDialogConfiguration = new MaterialAlertDialogConfiguration
                    {
                        MessageTextColor = Color.Black,
                        MessageFontFamily = XF.Material.Forms.Material.GetResource<OnPlatform<string>>("QuranFont"),
                        TintColor = Color.Black
                    };
                    await MaterialDialog.Instance.AlertAsync(message: "You have already registered this email account with us using a different social login like Gmail or Microsoft, please login using orignal source", configuration: alertDialogConfiguration);

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
        private async void onAppStart()
        {
            if (GlobalConst.isFromLogin)
            {
                await NavigationService.NavigateAsync(nameof(LoginPage));
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(SignupPage));
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            try
            {
                containerRegistry.RegisterForNavigation<NavigationPage>();
                containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
                containerRegistry.Register<IClientAPI, ClientAPI>();
                containerRegistry.RegisterForNavigation<Signup_Login_HomePage, Signup_Login_HomePageViewModel>();
                containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
                containerRegistry.RegisterForNavigation<SignupPage, SignupPageViewModel>();
                containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
                containerRegistry.RegisterForNavigation<QuizzesByCoursesPage, QuizzesByCoursesPageViewModel>();
                containerRegistry.RegisterForNavigation<QuizzesByCategoriesPage, QuizzesbyCategoryPageViewModel>();
                containerRegistry.RegisterForNavigation<LearnTabPage, LearnTabPageViewModel>();
                containerRegistry.RegisterForNavigation<PlayTabPage, PlayTabPageViewModel>();
                containerRegistry.RegisterForNavigation<QuizzesQuestionPage, QuizzesQuestionPageViewModel>();
                containerRegistry.RegisterForNavigation<EndQuizPage, EndQuizPageViewModel>();
                containerRegistry.RegisterForNavigation<EndQuizTestModePage, EndQuizTestModePageViewModel>();
                containerRegistry.RegisterForNavigation<WebPopupPage, WebPopupPageViewModel>();
                containerRegistry.RegisterForNavigation<DashboardPage, DashboardPageViewModel>();
                containerRegistry.RegisterForNavigation<SubscribePage, SubscribePageViewModel>();
                containerRegistry.RegisterForNavigation<StartGamePage, StartGamePageViewModel>();
                containerRegistry.RegisterForNavigation<AssignmentPage, AssignmentPageViewModel>();
                containerRegistry.RegisterForNavigation<CreateNewAssignmentPage, CreateNewAssignmentPageViewModel>();
                containerRegistry.RegisterForNavigation<AssignToUserPage, AssignToUserPageViewModel>();
                containerRegistry.RegisterForNavigation<QuizzesBySubCategoriesPage, QuizzesBySubCategoriesPageViewModel>();
                containerRegistry.RegisterForNavigation<DashboardDetailsPage, DashboardDetailsPageViewModel>();
                containerRegistry.RegisterForNavigation<LiveGameTabDetailsPage, LiveGameTabDetailsPageViewModel>();
                containerRegistry.RegisterForNavigation<MainLearnPage, MainLearnPageViewModel>();
                containerRegistry.RegisterForNavigation<LearnTabFlipPage, LearnTabFlipPageViewModel>();
                containerRegistry.RegisterForNavigation<LearnTabQuestionSelectionPage, LearnTabQuestionSelectionViewModel>();
                containerRegistry.Register<IRegisterToken>();
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
    }
}
