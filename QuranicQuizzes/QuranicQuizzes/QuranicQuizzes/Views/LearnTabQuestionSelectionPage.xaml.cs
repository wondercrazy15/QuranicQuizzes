using System;
using System.Collections.Generic;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class LearnTabQuestionSelectionPage : ContentPage
    {
        private LearnTabQuestionSelectionViewModel vm;

        public LearnTabQuestionSelectionPage()
        {
            InitializeComponent();
            vm = BindingContext as LearnTabQuestionSelectionViewModel;
        }
    }
}
