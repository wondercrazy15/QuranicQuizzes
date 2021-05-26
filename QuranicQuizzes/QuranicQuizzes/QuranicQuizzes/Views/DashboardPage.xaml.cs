using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class DashboardPage : ContentPage
    {
        private DashboardPageViewModel vm;
        public DashboardPage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as DashboardPageViewModel;
                var StartCalenderTapped = new TapGestureRecognizer();
                StartCalenderTapped.Tapped += StartCalenderTapped_Tapped;
                StartDatePickerImageTap.GestureRecognizers.Add(StartCalenderTapped);

                var EndCalenderTapped = new TapGestureRecognizer();
                EndCalenderTapped.Tapped += EndCalenderTapped_Tapped;
                EndDatePickerImageTap.GestureRecognizers.Add(EndCalenderTapped);

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

        //end date selecction from calender
        private void EndCalenderTapped_Tapped(object sender, EventArgs e)
        {
            EndDatePicker.Focus();
        }

        //Start date selecction from calender
        private void StartCalenderTapped_Tapped(object sender, EventArgs e)
        {
            StartDatePicker.Focus();
        }

        //Menu selection of Dashboard
         void MaterialMenuButton_MenuSelected(System.Object sender, XF.Material.Forms.UI.MenuSelectedEventArgs e)
        {
            var id = e.Result.Index;
            string StartDate, Enddate;
            switch (id)
            {
                case 0:
                    //Today
                    vm.IsCustomDate = false;
                    Enddate = DateTime.Now.ToString("yyyy-MM-dd");
                    StartDate = DateTime.Now.ToString("yyyy-MM-dd");
                    vm.getDashboardData(StartDate, Enddate);
                    break;
                case 1:
                    //Yesterday
                    vm.IsCustomDate = false;
                    StartDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    Enddate = DateTime.Now.ToString("yyyy-MM-dd");
                    vm.getDashboardData(StartDate, Enddate);
                    break;
                case 2:
                    //7 day
                    vm.IsCustomDate = false;
                    StartDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    Enddate = DateTime.Now.ToString("yyyy-MM-dd");
                    vm.getDashboardData(StartDate, Enddate);
                    break;
                case 3:
                    //30 day
                    vm.IsCustomDate = false;
                    StartDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                    Enddate = DateTime.Now.ToString("yyyy-MM-dd");
                    vm.getDashboardData(StartDate, Enddate);
                    break;
                case 4:
                    //This Month
                    vm.IsCustomDate = false;
                    var today = DateTime.Now;
                    StartDate = new DateTime(today.Year, today.Month, 1).ToString("yyyy-MM-dd");
                    Enddate = DateTime.Now.ToString("yyyy-MM-dd");
                    vm.getDashboardData(StartDate, Enddate);
                    break;
                case 5:
                    //Last Month
                    vm.IsCustomDate = false;
                    var todays = DateTime.Today;
                    var month = new DateTime(todays.Year, todays.Month, 1);
                    StartDate = month.AddMonths(-1).ToString("yyyy-MM-dd");
                    Enddate = month.AddDays(-1).ToString("yyyy-MM-dd");
                    vm.getDashboardData(StartDate, Enddate);
                    break;
                case 6:
                    //Custom
                    vm.IsCustomDate = true;
                    vm.SelectedStartDate = DateTime.Today;
                    vm.SelectedEndDate = DateTime.Today;
                    //var todays = DateTime.Today;
                    //var month = new DateTime(todays.Year, todays.Month, 1);
                    //StartDate = month.AddMonths(-1).ToString("yyyy-MM-dd");
                    //Enddate = month.AddDays(-1).ToString("yyyy-MM-dd");
                    //vm.getDashboardData(StartDate, Enddate);
                    break;
                default:
                    break;
            }
            //var args = (TappedEventArgs)e;
            //var myselecteditem = args.Parameter as MaterialMenuItem;
        }
    }

}
