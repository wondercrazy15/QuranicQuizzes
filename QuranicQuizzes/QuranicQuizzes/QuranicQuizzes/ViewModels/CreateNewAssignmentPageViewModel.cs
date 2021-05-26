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
    //Create new assignment Viewmodel
    public class CreateNewAssignmentPageViewModel : ViewModelBase
    {

        public string _assignmentTitle;
        public string AssignmentTitle
        {
            get
            {
                return _assignmentTitle;
            }
            set
            {
                _assignmentTitle = value;
                RaisePropertyChanged(nameof(AssignmentTitle));
            }
        }

        public DateTime _selectedDate { get; set; }
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }

            set
            {
                _selectedDate = value;
                RaisePropertyChanged(nameof(SelectedDate));
            }
        }

        public DateTime _minDate;
        public DateTime MinDate
        {
            get
            {
                return _minDate;
            }
            set
            {
                _minDate = value;
                RaisePropertyChanged(nameof(MinDate));
            }
        }

        public ObservableCollection<CreateAssignment> AssignmentList { get; set; } = new ObservableCollection<CreateAssignment>();

        INavigationService _navigationService;
        IClientAPI _clientAPI;
        string qid = "";
        public Command<object> AssignmentTap { get; set; }
        public CreateNewAssignmentPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            SelectedDate = DateTime.Today;
            MinDate = DateTime.Now.AddDays(1);
            AssignmentTap = new Command<object>(AssignmentTapClick);
        }


        //Click on Assignment
        private async void AssignmentTapClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    var CreateAssignment = obj as CreateAssignment;
                    if (CreateAssignment != null)
                    {
                        var parameters = new NavigationParameters();
                        parameters.Add("CreateAssignment", CreateAssignment);
                        await _navigationService.NavigateAsync(nameof(AssignToUserPage), parameters);
                    }
                }
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
        //public async override void Initialize(INavigationParameters parameters)
        //{
        //    base.Initialize(parameters);
        //    if (parameters != null)
        //    {
        //        QuizzeData = parameters["QuizzeData"] as Quizze;
        //        AssignmentTitle = QuizzeData.Name;
        //        await getAssignmentsList();
        //    }
        //}
        Quizze QuizzeData = new Quizze();
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                QuizzeData = parameters["QuizzeData"] as Quizze;
                AssignmentTitle = QuizzeData.Name;
               
            }
        }

        //Add New assignments
        public DelegateCommand AddNewAssignmentTap => new DelegateCommand(async () =>
         {
             //var parameters = new NavigationParameters();
             //parameters.Add("QuizzeData", QuizzeData);
             //await _navigationService.NavigateAsync(nameof(AssignToUserPage), parameters);
             try
             {
                 if (CrossConnectivity.Current.IsConnected)
                 {
                     var data = await _clientAPI.CreateAssignment(QuizzeData.Id, QuizzeData.CategoryID, SelectedDate.ToString("yyyy-MM-dd"));
                     if (data != null)
                     {
                         AssignmentList.Add(new CreateAssignment
                         {
                             ID = data.ID,
                             SetDate = data.SetDate,
                             DeadLine = data.DeadLine,
                             QuizName = data.QuizName,
                             assignedTo = data.assignedTo,
                             assignedCount = data.assignedTo.Count
                         });
                     }
                 }
                 else
                 {
                     ShowMessage("Please check your internet connection");
                 }

                
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
             
         });

        //API call for Assignment List
        public async Task getAssignmentsList()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

                var assignmentData = await _clientAPI.GetCreateAssignments(QuizzeData.Id.ToString());

                //AssignmentList = new ObservableCollection<CreateAssignment>(assignmentData);

                if (assignmentData != null)
                {
                    AssignmentList.Clear();
                    foreach (var item in assignmentData)
                    {
                        AssignmentList.Add(new CreateAssignment
                        {
                            ID = item.ID,
                            SetDate = item.SetDate,
                            DeadLine = item.DeadLine,
                            QuizName = item.QuizName,
                            assignedTo = item.assignedTo,
                            assignedCount = item.assignedTo.Count
                        });
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
