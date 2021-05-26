using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class QuizzesByCategoriesPage : ContentPage
    {
        private QuizzesbyCategoryPageViewModel vm;

        public QuizzesByCategoriesPage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as QuizzesbyCategoryPageViewModel;
                btnDropDown.Clicked += BtnDropDown_Clicked;
                EntryList.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
                EntryList.ItemAppearing += async (sender, e) =>
                {
                    var items = EntryList.ItemsSource as IList;
                    if (items == null
                        || items.Count == 0)
                        return;
                    if (e.Item != items[items.Count - 1])
                        return;
                    if (vm.TotalPageCount>1&&vm.TotalPageCount!=vm.pageCount )
                    {
                        vm.LoadMoreData(vm.IsSearch);
                    }
                };

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

        private void BtnDropDown_Clicked(object sender, EventArgs e)
        {
            try
            {
                //allQuizzes.TitleColor = Color.Black;
                allQuizzes.Focus();
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
    }
}
