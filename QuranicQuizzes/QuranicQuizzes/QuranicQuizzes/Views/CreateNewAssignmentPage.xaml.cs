using System;
using System.Collections.Generic;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    //CreateNewAssignment Page
    public partial class CreateNewAssignmentPage : ContentPage
    {
        private CreateNewAssignmentPageViewModel vm;

        public CreateNewAssignmentPage()
        {
            InitializeComponent();
            vm = BindingContext as CreateNewAssignmentPageViewModel;
            var CalenderTapped = new TapGestureRecognizer();
            CalenderTapped.Tapped += CalenderTapped_Tapped;
            CalenderImageTap.GestureRecognizers.Add(CalenderTapped);
        }

        private void CalenderTapped_Tapped(object sender, EventArgs e)
        {
            DatePicker.Focus();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await vm.getAssignmentsList();
        }
    }
}
