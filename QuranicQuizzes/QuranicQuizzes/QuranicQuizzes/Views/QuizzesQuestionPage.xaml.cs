using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Plugin.SimpleAudioPlayer;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
namespace QuranicQuizzes.Views
{
    public partial class QuizzesQuestionPage : ContentPage
    {
        public QuizzesQuestionPageViewModel vm;
        public QuizzesQuestionPage()
        {
            try
            {
                InitializeComponent();
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                vm = BindingContext as QuizzesQuestionPageViewModel;
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                MessagingCenter.Subscribe<object>(this, "StartProgress", (sender) => {
                    progressBarAsync();
                });
                MessagingCenter.Subscribe<object>(this, "ShowResultPage", (sender) => {
                    ShowResultPage();
                });
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        //Animation of slidding
        private async void ShowResultPage()
        {
            try
            {

                if (vm.IsQuestionAnswerInfoVisible)
                {
                    if (vm.IsQuestionVisible)
                    {
                       // vm.IsQuestionAnswerInfoVisible = true;
                        var animation = new Animation();
                        Animation translateLayout;
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            // You're on a phone
                            translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, 0, -500);
                        }
                        else
                        {
                            // You're on a tablet
                            translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, 0, -900);
                        }
                        //translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, 0, -500);
                        var textFieldChangeOpacity = new Animation(v => QuestionAnswerInfoVisibleLayout.Opacity = v, 1, 0);
                        animation.Add(0, 1, translateLayout);
                        animation.Add(0, 0.2, textFieldChangeOpacity);
                        animation.Commit(this, "Slide", 0, 1400, Easing.Linear);

                        await QuestionAnswerInfoVisibleLayout.FadeTo(1, 4550);
                    }
                    else
                    {
                        var animation = new Animation();
                        Animation translateLayout;
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            // You're on a phone
                            translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, -500, 0);
                        }
                        else
                        {
                            // You're on a tablet
                            translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, -900, 0);
                        }
                        
                        var textFieldChangeOpacity = new Animation(v => QuestionAnswerInfoVisibleLayout.Opacity = v, 0, 1);
                        animation.Add(0, 1, translateLayout);
                        animation.Add(0, 0.2, textFieldChangeOpacity);
                        animation.Commit(this, "Slide", 5, 800, Easing.Linear);
                        vm.IsQuestionVisible = false;
                        await QuestionAnswerInfoVisibleLayout.FadeTo(1, 4100);
                    }
                }
                else
                {
                    if (GlobalConst.istestMode)
                    {
                        // QuestionAnswerInfoVisibleLayout.Opacity = 0;
                        var animation = new Animation();
                        Animation translateLayout;
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            // You're on a phone
                            translateLayout = new Animation(v => QuestionVisibleLayout.TranslationX = v, -500, 0);
                        }
                        else
                        {
                            // You're on a tablet
                            translateLayout = new Animation(v => QuestionVisibleLayout.TranslationX = v, -900, 0);
                        }

                        var textFieldChangeOpacity = new Animation(v => QuestionAnswerInfoVisibleLayout.Opacity = v, 0, 1);
                        animation.Add(0, 0.2, textFieldChangeOpacity);
                        animation.Add(0, 1, translateLayout);
                        animation.Commit(this, "Slide", 5, 1000, Easing.Linear);
                        //  QuestionAnswerInfoVisibleLayout.FadeTo(1, 100);
                        vm.IsQuestionAnswerInfoVisible = false;
                        await QuestionAnswerInfoVisibleLayout.FadeTo(1, 4000);
                    }
                    else
                    {

                        vm.IsQuestionAnswerInfoVisible = true;
                        var animation = new Animation();
                        Animation translateLayout;
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            // You're on a phone
                            translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, 0, -500);
                        }
                        else
                        {
                            // You're on a tablet
                            translateLayout = new Animation(v => QuestionAnswerInfoVisibleLayout.TranslationX = v, 0, -900);
                        }
                       
                        var textFieldChangeOpacity = new Animation(v => QuestionAnswerInfoVisibleLayout.Opacity = v, 1, 0);
                        animation.Add(0, 1, translateLayout);
                        animation.Add(0, 0.2, textFieldChangeOpacity);
                        animation.Commit(this, "Slide", 0, 1400, Easing.Linear);

                        await QuestionAnswerInfoVisibleLayout.FadeTo(1, 4550);
                    }
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

        //Progress bar
        private async void progressBarAsync()
        {
            try
            {
                progressBar.Progress = 1;
                await progressBar.ProgressTo(0, 15100, Easing.Linear);
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
            

        }
       
        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.PopupStack.Count > 0)
            {
                popUpDissmiss();

            }
            else
            {
                vm.SoundStop.Execute();
                MessagingCenter.Unsubscribe<object>(this, "StartProgress");
                MessagingCenter.Unsubscribe<object>(this, "ShowResultPage");
                vm.Back();
            }

            return true;
        }

        //Information popup hide
        public async void popUpDissmiss()
        {
            try
            {

                await PopupNavigation.Instance.PopAllAsync();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Opps,Something wrong..!!");
            }
        }
    }
}
