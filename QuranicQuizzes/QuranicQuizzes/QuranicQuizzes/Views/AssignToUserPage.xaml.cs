using System;
using System.Collections;
using System.Collections.Generic;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    //AssignToUser Page
    public partial class AssignToUserPage : ContentPage
    {
        private AssignToUserPageViewModel vm;

        public AssignToUserPage()
        {
            InitializeComponent();
            vm = BindingContext as AssignToUserPageViewModel;
            EntryList.ItemAppearing += async (sender, e) =>
            {
                var items = EntryList.ItemsSource as IList;
                if (items == null
                    || items.Count == 0)
                    return;
                if (e.Item != items[items.Count - 1])
                    return;
                if (vm.TotalPageCount > 1 && vm.TotalPageCount != vm.pageCount)
                {
                    vm.LoadMoreData(vm.IsSearch);
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //EntryList.ItemsSource = vm.UserList;
        }
    }
}
