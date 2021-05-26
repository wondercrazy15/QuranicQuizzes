using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// Login page viewmodel
    /// </summary>
    public class LoginPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;

        public LoginPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
#if DEBUG
            Email="app.2@quranicquizzes.com";
            Password="Jasm1n";
#endif
        }

        public string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        public string _password;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
        

        

        //Terms and condition Click 
        public DelegateCommand TermsClickCommand => new DelegateCommand(async () =>
        {
            await Browser.OpenAsync("https://quranicquizzes.com/Legal/Terms", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,

            });

            //await Xamarin.Essentials.Launcher.OpenAsync("https://quranicquizzes.com/Legal/Terms"); //Device.OpenUri(new System.Uri("https://quranicquizzes.com/Legal/Terms"));
        });

        //Login button Click 
        public DelegateCommand LoginClick => new DelegateCommand(async () =>
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password))
                    {
                        DependencyService.Get<IToast>().Show("Please enter username and password");
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        Login d = new Login();
                        d.grant_type = "password";
                        d.username = Email;
                        d.password = Password;

                        var data = await _clientAPI.DoLogin(d);
                        if (data.access_token != null)
                        {
                            
                            UserSettings.AccesToken = "bearer " + data.access_token;
                            var UserData = await _clientAPI.GetUserInfo();
                            if (UserData != null)
                            {
                                UserSettings.UserId = UserData.UserID;
                                UserSettings.Name = UserData.Name;
                                UserSettings.IsAdmin=UserData.IsAdmin;
                                UserSettings.IsEnrolled = UserData.IsEnrolled;
                                UserSettings.IsPaidMember = UserData.IsPaidMember;
                                UserSettings.IsStudent = UserData.IsStudent;
                                UserSettings.IsTeacher = UserData.IsTeacher;
                                UserSettings.IsFamilyAdmin = UserData.IsFamilyAdmin;
                                UserSettings.IsInstituteAdmin = UserData.IsInstituteAdmin;
                                UserSettings.Logo = GlobalConst.ApiUrl+UserData.Logo;
                                DependencyService.Get<IRegisterToken>().RegisterToken();
                                
                            }
                            UserSettings.IsLogin = true;
                            await _navigationService.NavigateAsync(nameof(MainPage));
                            UserDialogs.Instance.HideLoading();
                        }
                        else
                        {
                            DependencyService.Get<IToast>().Show(data.errorData.error_description);
                            UserDialogs.Instance.HideLoading();

                        }
                        
                    }
                }
                else
                {
                    ShowMessage("Please check your internet connection");
                }
                
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
            }

        });

        

        //Facebbok button Click 
        public DelegateCommand FacebookTap => new DelegateCommand(async () =>
        {
            await Browser.OpenAsync("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Facebook&response_type=token&redirect_uri=quranicquizzes://home", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,
                
            });

            GlobalConst.isFromLogin = true;
            //Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Facebook&response_type=token&redirect_uri=quranicquizzes://home"));
        });

        //Microsoft button Click 
        public DelegateCommand MicrosoftTap => new DelegateCommand(async () =>
        {
            GlobalConst.isFromLogin = true;
            await Browser.OpenAsync("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Microsoft&response_type=token&redirect_uri=quranicquizzes://home", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,
            });
            //Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Microsoft&response_type=token&redirect_uri=quranicquizzes://home"));
        });

        //Google button Click 
        public DelegateCommand GoogleTap => new DelegateCommand(async () =>
        {
            GlobalConst.isFromLogin = true;
            await Browser.OpenAsync("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Google&response_type=token&redirect_uri=quranicquizzes://home", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,
            });
            //Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Google&response_type=token&redirect_uri=quranicquizzes://home"));
        });

        //AppleTap button Click 
        public DelegateCommand AppleTap => new DelegateCommand(async () =>
        {
            GlobalConst.isFromLogin = true;
            await Browser.OpenAsync("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=https://appleid.apple.com&response_type=token&redirect_uri=quranicquizzes://home", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,
            });
           // Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=https://appleid.apple.com&response_type=token&redirect_uri=quranicquizzes://home"));
        });

        public async void Back()
        {
            await _navigationService.NavigateAsync(nameof(Signup_Login_HomePage));
        }

    }
}
