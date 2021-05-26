using System;
using System.Collections.Generic;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    //EndQuizPage
    public partial class EndQuizPage : ContentPage
    {
        private EndQuizPageViewModel vm;

        public EndQuizPage()
        {
            InitializeComponent();
            vm = BindingContext as EndQuizPageViewModel;
            MessagingCenter.Subscribe<object>(this, "RemoveBackStack", (sender) => {
                RemoveBackStack();
            });
        }
        protected override bool OnBackButtonPressed()
        {
            vm.Back();
            return true;
        }

        public void RemoveBackStack()
        {
            // Remove page before Edit Page
            this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 3]);
            this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
            this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
            // This PopAsync will now go to List Page
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<object>(this, "RemoveBackStack");

        }
    }
}
