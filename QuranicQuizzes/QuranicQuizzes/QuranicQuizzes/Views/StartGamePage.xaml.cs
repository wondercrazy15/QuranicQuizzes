using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class StartGamePage : ContentPage
    {
        private StartGamePageViewModel vm;

        public StartGamePage()
        {
            InitializeComponent();
            vm = BindingContext as StartGamePageViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
           
            webview.EvaluateJavaScriptAsync("endGame()");

            return base.OnBackButtonPressed();
        }

        //End game Event call
        void Handle_CloseTapped(object sender, System.EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    MessagingCenter.Send<object, string>(this, "EndLiveGame", "endGame()");
                }
                else
                {
                    webview.EvaluateJavaScriptAsync("endGame()");
                }
                this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
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
           
            //OnBackButtonPressed();

        }

    }
}
