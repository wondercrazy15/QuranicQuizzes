using System;
using System.Collections.Generic;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class MainLearnPage : ContentPage
    {
        private MainLearnPageViewModel vm;

        public MainLearnPage()
        {
            InitializeComponent();
            vm = BindingContext as MainLearnPageViewModel;
        }
    }
}
