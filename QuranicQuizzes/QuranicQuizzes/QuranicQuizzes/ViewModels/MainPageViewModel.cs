using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// Main page viewmodel
    /// It is user for Menu
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
      
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        //public List<MasterPageItem> menuList { get; set; }

        public MainPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            IsMoreInfo = false;
            IsInstitute = false;
            gobacknullAsync();
            if (UserSettings.IsStudent)
                IsAssignment = true;
            else
                IsAssignment = false;
            
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

        }
        private ObservableCollection<MasterPageItem> _menuList = new ObservableCollection<MasterPageItem>();
        public ObservableCollection<MasterPageItem> menuList
        {
            get { return _menuList; }
            set
            {
                _menuList = value;
                RaisePropertyChanged("menuList");
            }
        }

        public bool _IsMoreInfo;
        public bool IsMoreInfo
        {
            get
            {
                return _IsMoreInfo;
            }

            set
            {
                _IsMoreInfo = value;
                RaisePropertyChanged(nameof(IsMoreInfo));
            }
        }
        
        public bool _IsAssignment;
        public bool IsAssignment
        {
            get
            {
                return _IsAssignment;
            }

            set
            {
                _IsAssignment = value;
                RaisePropertyChanged(nameof(IsAssignment));
            }
        }
        public bool _IsNewAssignment;
        public bool IsNewAssignment
        {
            get
            {
                return _IsNewAssignment;
            }
            set
            {
                _IsNewAssignment = value;
                RaisePropertyChanged(nameof(IsNewAssignment));
            }
        }

        private ObservableCollection<Assignment> _newAssignmentList = new ObservableCollection<Assignment>();
        public ObservableCollection<Assignment> NewAssignmentList
        {
            get { return _newAssignmentList; }
            set
            {
                _newAssignmentList = value;
                RaisePropertyChanged("NewAssignmentList");
            }
        }

        //NewAssignment Count
        public DelegateCommand NewAssignmentListCount => new DelegateCommand(async () =>
        {
            try
            {
                var data = await _clientAPI.GetNewAssignments();
                if (data != null && data.Count > 0)
                {
                    //NewAssignmentList = new ObservableCollection<Assignment>(data);
                    NewAssignmentList.Clear();
                    foreach (var item in data)
                    {
                        NewAssignmentList.Add(new Assignment {
                            ID= item.ID,
                            HWID= item.HWID,
                            CategoryID= item.CategoryID,
                            Attempts= item.Attempts,
                            QuizName= item.QuizName,
                            SetTime=item.SetTime,
                            SetTimeLabel= item.SetTimeLabel,
                            Deadline= item.Deadline,
                            DeadlineLabel= item.DeadlineLabel
                        });
                    }
                    IsNewAssignment = true;
                    NewAssignmentCounts = data.Count.ToString();
                }
                else
                {
                    IsNewAssignment = false;
                    NewAssignmentCounts = "0";
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
       
        public DelegateCommand MoreInfoClickCommand => new DelegateCommand(async () =>
        {
            IsMoreInfo =!IsMoreInfo;
        });
        public DelegateCommand AssignmentsClickCommand => new DelegateCommand(async () =>
        {
            try
            {
                await _navigationService.NavigateAsync(nameof(AssignmentPage));
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
        public string _NewAssignmentCounts;
        public string NewAssignmentCounts
        {
            get
            {
                return _NewAssignmentCounts;
            }

            set
            {
                _NewAssignmentCounts = value;
                RaisePropertyChanged(nameof(NewAssignmentCounts));
            }
        }
        public bool _IsInstitute;
        public bool IsInstitute
        {
            get
            {
                return _IsInstitute;
            }

            set
            {
                _IsInstitute = value;
                RaisePropertyChanged(nameof(IsInstitute));
            }
        }
        public bool _IsNewAssignmentTap;
        public bool IsNewAssignmentTap
        {
            get
            {
                return _IsNewAssignmentTap;
            }

            set
            {
                _IsNewAssignmentTap = value;
                RaisePropertyChanged(nameof(IsNewAssignmentTap));
            }
        }

        public DelegateCommand NewAssignmentClickCommand => new DelegateCommand(async () =>
        {
            IsNewAssignmentTap = !IsNewAssignmentTap;
        });

        public DelegateCommand InstituteClickCommand => new DelegateCommand(async () =>
        {
            IsInstitute = !IsInstitute;
        });
        public DelegateCommand AboutUsInfoClickCommand => new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(nameof(AboutUSPage));
        });
        public DelegateCommand FAQClickCommand => new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(nameof(FAQPage));
        });
        public DelegateCommand PrivacyPolicyClickCommand => new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(nameof(PrivacyPolicyPage));
        });

        public async void gobacknullAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(null);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
