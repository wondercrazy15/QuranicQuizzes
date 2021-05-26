using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace QuranicQuizzes.Views
{
    public partial class PlayTabPage : ContentPage
    {
        private string quizID;
        private string hWID;
        private PlayTabPageViewModel vm;

        public PlayTabPage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as PlayTabPageViewModel;
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                GlobalConst.IsNotificationTap = false;
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
            GlobalConst.IsNotificationTap = false;
        }

        protected override bool OnBackButtonPressed()
        {
            //vm.CloseQuizze.Execute();
            return base.OnBackButtonPressed();
        }
    }
}
