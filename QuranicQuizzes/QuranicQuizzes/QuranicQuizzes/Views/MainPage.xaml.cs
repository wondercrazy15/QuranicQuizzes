using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
namespace QuranicQuizzes.Views
{
    //Menu in mainpage
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        private MainPageViewModel vm;
        //MainPageViewModel vm;
        public MainPage()
        {
            try
            {
                InitializeComponent();
                menuList = new List<MasterPageItem>();
                vm = BindingContext as MainPageViewModel;
                //vm = BindingContext as MainPageViewModel;
                //vm.AboutUs.Execute();
                userprofilePic.Source = UserSettings.Logo;
                userName.Text = UserSettings.Name;
                
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        DonateStack.IsVisible = false;
                        break;
                    case Device.Android:
                        
                        DonateStack.IsVisible = true;
                        break;
                }
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                MasterPageItem page3 = new MasterPageItem();
                MasterPageItem page4 = new MasterPageItem();
                MasterPageItem page5 = new MasterPageItem();
                MasterPageItem page6 = new MasterPageItem();
                MasterPageItem page7 = new MasterPageItem();
                MasterPageItem page8 = new MasterPageItem();
                //if(GlobalConst.BaseUrl".ProfilePicture / 1051.jpg")
                //Fot Android / IOS icons
                var page1 = new MasterPageItem() { id = 1, Title = "Home", Icon = "home_icon.png", TargetType = typeof(HomePage) };
                var page2 = new MasterPageItem() { id = 2, Title = "Dashboard", Icon = "dashboard_icon.png", TargetType = typeof(HomePage) };
                if (!UserSettings.IsPaidMember)
                {
                    page3 = new MasterPageItem() { id = 3, Title = "Subscribe", Icon = "subscribe_icon.png", TargetType = typeof(HomePage) };
                }
                else
                {
                    if (UserSettings.IsFamilyAdmin)
                        page4 = new MasterPageItem() { id = 4, Title = "My Students", Icon = "student_icon.png", TargetType = typeof(HomePage) };
                    if (UserSettings.IsTeacher || UserSettings.IsFamilyAdmin)
                        page5 = new MasterPageItem() { id = 5, Title = "Student Report", Icon = "report_icon.png", TargetType = typeof(HomePage) };
                    //if (UserSettings.IsStudent)
                    //    page6 = new MasterPageItem() { id = 6, Title = "My Assignments", Icon = "diamond_icon.png", TargetType = typeof(HomePage) };
                    //page6 = new MasterPageItem() { id = 6, Title = "Student Reports", Icon = "Add.png", TargetType = typeof(HomePage) };
                    page7 = new MasterPageItem() { id = 7, Title = "Join Live game", Icon = "live_icon.png", TargetType = typeof(HomePage) };
                }
                //if(UserSettings.IsInstituteAdmin)
                //InstituteMenu.IsVisible = true;
                //else
                //    InstituteMenu.IsVisible = false;
                menuList.Add(page1);
                menuList.Add(page2);
                if (!UserSettings.IsPaidMember)
                    menuList.Add(page3);
                else
                {
                    if (UserSettings.IsFamilyAdmin)
                        menuList.Add(page4);
                    if (UserSettings.IsTeacher || UserSettings.IsFamilyAdmin)
                        menuList.Add(page5);
                    //if (UserSettings.IsStudent)
                    //    menuList.Add(page6);
                    menuList.Add(page7);
                }
                
                BindableLayout.SetItemsSource(navigationDrawerList, menuList);
                Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(typeof(HomePage)));
                
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Opps,Something wrong..!!");
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };
                Crashes.TrackError(ex, properties);
            }
        }

        //Notification Tap open Assignment page
        private async void NotificationCall()
        {
            await Navigation.PushAsync(new AssignmentPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (CrossConnectivity.Current.IsConnected)
            {
                
                    vm.NewAssignmentListCount.Execute();
                    Device.StartTimer(TimeSpan.FromSeconds(20), () =>
                    {
                        Device.BeginInvokeOnMainThread(() => GetNewAssignmentList());
                        return true;
                    });
                
            }
            else
            {
                vm.ShowMessage("Please check your internet connection");
            }
            if (GlobalConst.IsNotificationTap)
                NotificationCall();

        }

        //NewAssignmentList count
        private async void GetNewAssignmentList()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                vm.NewAssignmentListCount.Execute();
            }
            else
            {
                vm.ShowMessage("Please check your internet connection");
            }
            
        }

        async void AboutUsInfoClickCommand(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AboutUSPage());
            IsPresented = false;
        }
        async void DonateTap(System.Object sender, System.EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("https://donorbox.org/help-quranic-quizzes-add-more-free-content"));
            IsPresented = false;
        }
        async void FAQClickCommand(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FAQPage());
            IsPresented = false;
        }
        async void PrivacyPolicyClickCommand(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PrivacyPolicyPage());
            IsPresented = false;
        }
        async void SignOutTap(System.Object sender, System.EventArgs e)
        {
            if (await UserDialogs.Instance.ConfirmAsync("Are you sure you want to Logout? ", "", "Yes", "No"))
            {
                UserSettings.IsLogin = false;
                UserSettings.Logo =string.Empty;
                UserSettings.Name = string.Empty;
                UserSettings.IsStudent = false;
                UserSettings.IsPaidMember = false;
                UserSettings.IsInstituteAdmin = false;
                UserSettings.IsFamilyAdmin = false;
                UserSettings.IsTeacher = false;
                UserSettings.IsEnrolled = false;
                UserSettings.AccesToken = string.Empty;
                DependencyService.Get<IRegisterToken>().RemoveRegisterToken();
                await Navigation.PushAsync(new Signup_Login_HomePage());
                IsPresented = false;
            }
        }

        async void AllAssignment(System.Object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new AssignmentPage());
            IsPresented = false;
        }

        //Feedback Click 
        async void FeedbackClickCommand(System.Object sender, System.EventArgs e)
        {
            try
            {
                DependencyService.Get<IAppRating>().RateApp();
                IsPresented = false;
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
            //await Xamarin.Essentials.Launcher.OpenAsync("https://quranicquizzes.com/Legal/Terms"); //Device.OpenUri(new System.Uri("https://quranicquizzes.com/Legal/Terms"));
        }

        async void TermClickCommand(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TermsPage());
            IsPresented = false;
        }
        async void ContactUsTap(System.Object sender, System.EventArgs e)
        {
            try
            {
                await Launcher.OpenAsync(new Uri("mailto:info@quranicquizzes.com?subject= Account Enquiry "));
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
            
            //await Launcher.OpenAsync(new Uri("mailto:info@quranicquizzes.com?subject= Account Enquiry "));
            IsPresented = false;
        }

        //Upper Menu click
        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                var args = (TappedEventArgs)e;
                var myselecteditem = args.Parameter as MasterPageItem;

                // var myselecteditem = ((TapGestureRecognizer)sender).CommandParameter;
                switch (myselecteditem.id)
                {
                    case 1:
                        Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(typeof(HomePage)));
                        break;
                    case 2:
                        await Navigation.PushAsync(new DashboardPage());
                        break;
                    case 3:
                        await Navigation.PushAsync(new SubscribePage());
                        //Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(typeof(HomePage)));
                        break;
                    case 4:
                        await Navigation.PushAsync(new MyStudentPage());
                        break;
                    case 5:
                        await Navigation.PushAsync(new StudentReportPage());
                        break;
                    case 6:
                        await Navigation.PushAsync(new AssignmentPage());
                        //vm.AssignmentsClickCommand.Execute();
                        //Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(typeof(HomePage)));
                        break;
                    case 7:
                        await Navigation.PushAsync(new LiveGamePage());
                        //Detail = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(typeof(HomePage)));
                        break;
                }
                IsPresented = false;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };
                Crashes.TrackError(ex, properties);
                DependencyService.Get<IToast>().Show("Opps,Something wrong..!!");
            }
        }
    }
}
