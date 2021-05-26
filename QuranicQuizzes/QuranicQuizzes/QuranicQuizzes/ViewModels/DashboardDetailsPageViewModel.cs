using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Prism.Navigation;

using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using static QuranicQuizzes.Models.DashboardDetails;

namespace QuranicQuizzes.ViewModels
{
    //Dashboard Quizzes Detail Viewmodel
    public class DashboardDetailsPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        string qid;

        public string _title;
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                SetProperty(ref _title, value);
            }
        }

        public ObservableCollection<DashboardQuizze> DetailsList { get; set; } = new ObservableCollection<DashboardQuizze>();

        public DashboardDetailsPageViewModel(INavigationService navigationService,IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                qid = parameters["qid"] as string;
                Title = parameters["title"] as string;
                if (CrossConnectivity.Current.IsConnected)
                {
                    await dataBinding();
                }
                else
                {
                    ShowMessage("Please check your internet connection");
                }
                
            }
        }

        //Dashboard Quizzes click information
        private async Task dataBinding()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                DashboardDetails details = new DashboardDetails();

                details = await _clientAPI.getDashboardDetails(qid);
                if (details != null)
                {
                    foreach (var item in details.Quizzes)
                    {
                        DetailsList.Add(item);
                    }
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
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
