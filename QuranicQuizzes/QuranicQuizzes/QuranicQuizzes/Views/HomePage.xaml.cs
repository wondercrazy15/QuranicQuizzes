using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace QuranicQuizzes.Views
{
    public partial class HomePage : ContentPage
    {
        private HomePageViewModel vm;

        public HomePage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as HomePageViewModel;
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                CategoriesList.HeightRequest= DeviceDisplay.MainDisplayInfo.Height;
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GlobalConst.isNewAssignments = false;
            //vm.c
            if(vm.category.Count>0)
            CategoriesList.ItemsSource=vm.category;
        }
        protected override bool OnBackButtonPressed()
        {

                if (Device.OS == TargetPlatform.Android)
                    DependencyService.Get<IAndroidMethods>().CloseApp();
                return false;
        }
        
    }
}
