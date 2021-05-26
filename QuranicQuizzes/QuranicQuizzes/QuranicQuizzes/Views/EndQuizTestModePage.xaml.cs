using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    //EndQuiz of TestMode Page
    public partial class EndQuizTestModePage : ContentPage
    {
        private EndQuizTestModePageViewModel vm;

        public EndQuizTestModePage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as EndQuizTestModePageViewModel;
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
        protected override bool OnBackButtonPressed()
        {
            vm.Back();
            return true;
        }
    }
}
