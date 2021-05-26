using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class LearnTabPage : ContentPage
    {
        private LearnTabPageViewModel vm;

        public LearnTabPage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as LearnTabPageViewModel;
                //WebView web_view = new WebView
                //{
                //    Source = "https://quranicquizzes.com/Quizzes/LearnApp/311?ak=9yT6MRqbdBYbEvQQ7tweG&isCourse=false",
                //    VerticalOptions = LayoutOptions.FillAndExpand,
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //};
                //mainStack.Children.Add(web_view);
                LearnWebView.Navigated += LearnWebView_Navigated;
                
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

        //If learnTab not available then show messege
        private void LearnWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            try
            {
                switch (e.Result)
                {
                    case WebNavigationResult.Cancel:
                        break;
                    case WebNavigationResult.Failure:
                        LearnWebView.IsVisible = false;
                        ErrorMsg.IsVisible = true;
                        break;
                    case WebNavigationResult.Success:
                        LearnWebView.IsVisible = true;
                        ErrorMsg.IsVisible = false;
                        break;
                    case WebNavigationResult.Timeout:
                        break;
                    default:
                        break;
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

        

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.Back.Execute();
        }
    }
}
