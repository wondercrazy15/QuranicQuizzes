using System;
using System.Collections.Generic;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class AssignmentPage : ContentPage
    {
        //AssignmentList Page
        private AssignmentPageViewModel vm;

        public AssignmentPage()
        {
            InitializeComponent();
            vm=BindingContext as AssignmentPageViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //if(GlobalConst.IsNotificationTap)
            //vm.NotificationTap();
            GlobalConst.IsNotificationTap = false;
        }
       
    }
}
