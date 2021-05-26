using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// Home page of signup and login Screen
    /// </summary>
    public class Signup_Login_HomePageViewModel:ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;

        public Signup_Login_HomePageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            Banners.Clear();
            Banners.Add(new Banner { Heading = "SUMMER COLLECTION", Message = "40% Discount", Caption = "BEST DISCOUNT THIS SEASON", Image = "banner1.jpg" });
            Banners.Add(new Banner { Heading = "WOMEN'S CLOTHINGS", Message = "UP TO 50% OFF", Caption = "GET 50% OFF ON EVERY ITEM", Image = "banner2.jpg" });
            Banners.Add(new Banner { Heading = "ELEGANT COLLECTION", Message = "20% Discount", Caption = "UNIQUE COMBINATIONS OF ITEMS", Image = "banner3.jpg" });
            Banners.Add(new Banner { Heading = "ELEGANT COLLECTION", Message = "20% Discount", Caption = "UNIQUE COMBINATIONS OF ITEMS", Image = "banner4.jpg" });
        }
        public ObservableCollection<Banner> Banners { get; set; } = new ObservableCollection<Banner>();

        //Click of Sign up button
        public DelegateCommand RegisterCommmand => new DelegateCommand(async () =>
        {
            try
            {
                await _navigationService.NavigateAsync(nameof(SignupPage));
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

        });

        //Click of Login button
        public DelegateCommand LoginCommmand => new DelegateCommand(async () =>
        {
            try
            {
                await _navigationService.NavigateAsync(nameof(LoginPage));
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

        });
      

    }
}
