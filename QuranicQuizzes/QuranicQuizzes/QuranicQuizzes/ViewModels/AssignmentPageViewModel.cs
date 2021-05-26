using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    //AssignmentList ViewModel
    public class AssignmentPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public Command<object> AssignmentTap { get; set; }
        public ObservableCollection<Assignment> AssignmentList { get; set; } = new ObservableCollection<Assignment>();

        public AssignmentPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                _clientAPI = clientAPI;
                AssignmentTap = new Command<object>(AssignmentTapClick);
                if (CrossConnectivity.Current.IsConnected)
                {
                    getAssignments();
                }
                else
                {
                    ShowMessage("Please check your internet connection");
                }
                
                //getAssignments();
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

        //Assignment Click
        private async void AssignmentTapClick(object obj)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (obj != null)
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        var assignmentData = obj as Assignment;
                        if (assignmentData != null)
                        {
                            var TodayDate = DateTime.Today;
                            if (assignmentData.Deadline > TodayDate)
                            {
                                var parameters = new NavigationParameters();
                                parameters.Add("assignmentData", assignmentData);
                                await _navigationService.NavigateAsync(nameof(PlayTabPage), parameters);
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                            }
                        }

                    }
                }
                else
                {
                    ShowMessage("Please check your internet connection");
                }

               
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

        //Api call of Assignment List
        private async void getAssignments()
        {
          await DataBinding();
        }

        public async Task DataBinding()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                //AssignmentsData assignmentsData = new AssignmentsData();

                var assignmentLists = await _clientAPI.GetAssignments();

                Color color = Color.FromHex("#000000");
                if (assignmentLists != null)
                {
                    foreach (var item in assignmentLists)
                    {
                        if (item.Attempts == 0)
                        {
                            color = Color.FromHex("#17a2b8");
                        }
                        AssignmentList.Add(new Assignment
                        {
                            ID = item.ID,
                            HWID = item.HWID,
                            CategoryID = item.CategoryID,
                            Attempts = item.Attempts,
                            QuizName = item.QuizName,
                            SetTimeLabel = item.SetTime.ToString("dd/MM/yyyy"),
                            Deadline = item.Deadline.Date,
                            DeadlineLabel = item.DeadlineLabel,
                            TextColor = color
                        });
                    }
                }

                // AssignmentList = new ObservableCollection<Assignment>(assignmentLists);
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
        
        public DelegateCommand BackClick => new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(nameof(MainPage));
        });
        
    }
}
