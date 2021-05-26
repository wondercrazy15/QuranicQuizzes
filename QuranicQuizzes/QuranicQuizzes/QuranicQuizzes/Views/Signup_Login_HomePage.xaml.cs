using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace QuranicQuizzes.Views
{
    public partial class Signup_Login_HomePage : ContentPage
    {
        private Signup_Login_HomePageViewModel vm;
        public Signup_Login_HomePage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as Signup_Login_HomePageViewModel;
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                //cvBanners.ItemsSource = vm.Banners;
                //cvBanners.Position = 0;
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
        private System.Timers.Timer timer;
        protected override void OnAppearing()
        {
            timer = new System.Timers.Timer(TimeSpan.FromSeconds(5).TotalMilliseconds) { AutoReset = true, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            timer?.Dispose();
            base.OnDisappearing();
        }

        //Slider timmer
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                if (cvBanners.Position == 2)
                {
                    cvBanners.Position = 0;
                    return;
                }

                cvBanners.Position += 1;
            });
        }
        protected override bool OnBackButtonPressed()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    DependencyService.Get<IAndroidMethods>().CloseApp();
                    break;
            }
            return false;
        }

    }
}
