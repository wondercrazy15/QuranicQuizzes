using System;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// This View model is use for Sign up page
    /// </summary>
    public class SignupPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;

        public SignupPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
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
        });

        ////Facebook button Click 
        //public DelegateCommand FacebookTap => new DelegateCommand(async () =>
        //{
        //    GlobalConst.isFromLogin = false;
        //    Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Facebook&response_type=token&redirect_uri=quranicquizzes://home"));
        //});

        ////Microsoft button Click 
        //public DelegateCommand MicrosoftTap => new DelegateCommand(async () =>
        //{
        //    GlobalConst.isFromLogin = false;
        //    Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Microsoft&response_type=token&redirect_uri=quranicquizzes://home"));
        //});

        ////Google button Click 
        //public DelegateCommand GoogleTap => new DelegateCommand(async () =>
        //{
        //    GlobalConst.isFromLogin = false;
        //    Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=Google&response_type=token&redirect_uri=quranicquizzes://home"));
        //});
        ////AppleTap button Click 
        //public DelegateCommand AppleTap => new DelegateCommand(async () =>
        //{
        //    GlobalConst.isFromLogin = true;
        //    Device.OpenUri(new Uri("https://quranicquizzes.com/api/Account/ExternalLogin?ak=9yT6MRqbdBYbEvQQ7tweG&client_id=ULmmYwrgERU3SJ3wJAT9Q6Rr2&provider=https://appleid.apple.com&response_type=token&redirect_uri=quranicquizzes://home"));
        //});

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
    }
}
