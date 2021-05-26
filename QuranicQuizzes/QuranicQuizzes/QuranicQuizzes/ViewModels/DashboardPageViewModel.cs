using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Forms;
using XF.Material.Forms.Models;
using static QuranicQuizzes.Models.Dashboard;

namespace QuranicQuizzes.ViewModels
{
    //Dashboard viewmodel
    public class DashboardPageViewModel : ViewModelBase
    {

        INavigationService _navigationService;
        IClientAPI _clientAPI;
        Dashboards dashboard;
        private double _barValue;
        public Command<object> AssignmentTap { get; set; }
        public Command<object> QuizzesTap { get; set; }
        public Command<object> LiveGameTap { get; set; }
        public Command<object>  HomeListTap { get; set; }
        public ObservableCollection<HomeTab> HomeList { get; set; } = new ObservableCollection<HomeTab>();


        public ObservableCollection<QuizzesTable> QuizzesDataList { get; set; } = new ObservableCollection<QuizzesTable>();

        public ObservableCollection<AssignmentTable> AssignmentDataList { get; set; } = new ObservableCollection<AssignmentTable>();

        public ObservableCollection<LiveGameTable> LiveGameDataList { get; set; } = new ObservableCollection<LiveGameTable>();

        public DashboardPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            IsHomeList = true;
            HomeBGcolor = Color.FromHex("#f37737");
            QuizzesBGcolor = Color.FromHex("#C1C1C1");
            LivegamesBGcolor = Color.FromHex("#C1C1C1");
            Assignments = Color.FromHex("#C1C1C1");
            AssignmentTap = new Command<object>(AssignmentTapClick);
            QuizzesTap = new Command<object>(QuizzesTapClick);
            LiveGameTap = new Command<object>(LiveGameTapClick);
            HomeListTap= new Command<object>(HomeListTapClick);
            
            MaxDate = DateTime.Today;

            if (CrossConnectivity.Current.IsConnected)
            {

                var StartDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                var Enddate = DateTime.Now.ToString("yyyy-MM-dd");

                if (dashboard == null)
                {
                    dashboard = new Dashboards();


                    getDashboardData(StartDate, Enddate);
                }
            }
            else
            {
                ShowMessage("Please check your internet connection");
            }
        }

        //Dashboard home Tab information
        private void HomeListTapClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    var data = obj as HomeTab;
                    if (data.dataTitle.Equals("QUIZZES COMPLETED"))
                    {
                        if (!IsQuizzesList)
                        {
                            IsHomeList = false;
                            IsQuizzesList = true;
                            IsLiveGameTabList = false;
                            IsAssignmentTabList = false;
                            HomeBGcolor = Color.FromHex("#C1C1C1");
                            QuizzesBGcolor = Color.FromHex("#f37737");
                            LivegamesBGcolor = Color.FromHex("#C1C1C1");
                            Assignments = Color.FromHex("#C1C1C1");
                        }
                    }
                    else if (data.dataTitle.Equals("LIVE GAMES"))
                    {
                        if (!IsLiveGameTabList)
                        {
                            IsHomeList = false;
                            IsQuizzesList = false;
                            IsLiveGameTabList = true;
                            IsAssignmentTabList = false;
                            HomeBGcolor = Color.FromHex("#C1C1C1");
                            QuizzesBGcolor = Color.FromHex("#C1C1C1");
                            LivegamesBGcolor = Color.FromHex("#f37737");
                            Assignments = Color.FromHex("#C1C1C1");
                        }
                    }
                    else if (data.dataTitle.Equals("ASSIGNMENTS"))
                    {
                        if (!IsAssignmentTabList)
                        {
                            IsHomeList = false;
                            IsQuizzesList = false;
                            IsLiveGameTabList = false;
                            IsAssignmentTabList = true;
                            HomeBGcolor = Color.FromHex("#C1C1C1");
                            QuizzesBGcolor = Color.FromHex("#C1C1C1");
                            LivegamesBGcolor = Color.FromHex("#C1C1C1");
                            Assignments = Color.FromHex("#f37737");
                        }
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

        public DateTime _selectedEndDate { get; set; }
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }
            set
            {
                if (value.Year==1900)
                {
                    return;
                }
                _selectedEndDate = value;
                getDashboardData(SelectedStartDate.ToString("yyyy-MM-dd"), SelectedEndDate.ToString("yyyy-MM-dd"));
                RaisePropertyChanged(nameof(SelectedEndDate));
            }
        }

        public DateTime _selectedStartDate { get; set; }
        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedStartDate;
            }
            set
            {
                if (value.Year==1900)
                {
                    return;
                }
                _selectedStartDate = value;
                getDashboardData(SelectedStartDate.ToString("yyyy-MM-dd"), SelectedEndDate.ToString("yyyy-MM-dd"));
                RaisePropertyChanged(nameof(SelectedStartDate));
            }
        }

        public DateTime _maxDate;
        public DateTime MaxDate
        {
            get
            {
                return _maxDate;
            }
            set
            {
                _maxDate = value;
                RaisePropertyChanged(nameof(MaxDate));
            }
        }

        private bool _IsHomeList { get; set; }
        public bool IsHomeList
        {
            get
            {
                return _IsHomeList;
            }
            set
            {
                _IsHomeList = value;
                RaisePropertyChanged(nameof(IsHomeList));
            }
        }

        private bool _IsQuizzesList { get; set; }
        public bool IsQuizzesList
        {
            get
            {
                return _IsQuizzesList;
            }
            set
            {
                _IsQuizzesList = value;
                RaisePropertyChanged(nameof(IsQuizzesList));
            }
        }
        private bool _IsLiveGameTabList { get; set; }
        public bool IsLiveGameTabList
        {
            get
            {
                return _IsLiveGameTabList;
            }
            set
            {
                _IsLiveGameTabList = value;
                RaisePropertyChanged(nameof(IsLiveGameTabList));
            }
        }
        private bool _IsCustomDate { get; set; }
        public bool IsCustomDate
        {
            get
            {
                return _IsCustomDate;
            }
            set
            {
                _IsCustomDate = value;
                RaisePropertyChanged(nameof(IsCustomDate));
            }
        }
        
        private bool _IsAssignmentTabList { get; set; }
        public bool IsAssignmentTabList
        {
            get
            {
                return _IsAssignmentTabList;
            }
            set
            {
                _IsAssignmentTabList = value;
                RaisePropertyChanged(nameof(IsAssignmentTabList));
            }
        }

        public Color _HomeBGcolor;
        public Color HomeBGcolor
        {
            get
            {
                return _HomeBGcolor;
            }

            set
            {
                _HomeBGcolor = value;
                RaisePropertyChanged(nameof(HomeBGcolor));
            }
        }

        public Color _QuizzesBGcolor;
        public Color QuizzesBGcolor
        {
            get
            {
                return _QuizzesBGcolor;
            }

            set
            {
                _QuizzesBGcolor = value;
                RaisePropertyChanged(nameof(QuizzesBGcolor));
            }
        }

        public Color _LivegamesBGcolor;
        public Color LivegamesBGcolor
        {
            get
            {
                return _LivegamesBGcolor;
            }

            set
            {
                _LivegamesBGcolor = value;
                RaisePropertyChanged(nameof(LivegamesBGcolor));
            }
        }

        public Color _Assignments;
        public Color Assignments
        {
            get
            {
                return _Assignments;
            }

            set
            {
                _Assignments = value;
                RaisePropertyChanged(nameof(Assignments));
            }
        }

        //Dashboard Assignment Tab information
        private async void AssignmentTapClick(object obj)
        {
            try
            {
                // UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                if (obj != null)
                {
                    var assignmentData = obj as AssignmentTable;
                    if (assignmentData != null)
                    {

                        var parameters = new NavigationParameters();
                        parameters.Add("qid", assignmentData.ID.ToString());
                        parameters.Add("title", assignmentData.Name);
                        await _navigationService.NavigateAsync(nameof(DashboardDetailsPage), parameters);

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

        //Dashboard Quizzes Tab information
        private async void QuizzesTapClick(object obj)
        {
            try
            {
                // UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                if (obj != null)
                {
                    var assignmentData = obj as QuizzesTable;
                    if (assignmentData != null)
                    {

                        var parameters = new NavigationParameters();
                        parameters.Add("qid", assignmentData.ID.ToString());
                        parameters.Add("title", assignmentData.Name);
                        await _navigationService.NavigateAsync(nameof(DashboardDetailsPage), parameters);

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

        //Dashboard LiveGame Tab information
        private async void LiveGameTapClick(object obj)
        {
            try
            {
                // UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                if (obj != null)
                {
                    var assignmentData = obj as LiveGameTable;
                    if (assignmentData != null)
                    {
                        var parameters = new NavigationParameters();
                        parameters.Add("qid", assignmentData.ID.ToString());
                        parameters.Add("title", assignmentData.Name);
                        await _navigationService.NavigateAsync(nameof(LiveGameTabDetailsPage), parameters);

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

        //Menu List
        public MaterialMenuItem[] Actions => new MaterialMenuItem[]
        {
            new MaterialMenuItem
            {
                Text = "Today"
            },
            new MaterialMenuItem
            {
                Text = "Yesterday"
            },
            new MaterialMenuItem
            {
                Text = "Last 7 Days"
            },
            new MaterialMenuItem
            {
                Text = "Last 30 Days"
            },
            new MaterialMenuItem
            {
                Text = "This Month"
            },
            new MaterialMenuItem
            {
                Text = "Last Month"
            },
            new MaterialMenuItem
            {
                Text = "Custom Range"
            }
        };

        //API call according start date and end date
        public async void getDashboardData(string startDate, string EndDate)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                dashboard = await _clientAPI.getDashboardData(startDate, EndDate);
                if (dashboard != null)
                {
                    HomeList.Clear();
                    // List<HomeTab> homeList = new List<HomeTab>();
                    //HomeList = new ObservableCollection<HomeTab>();
                    HomeList.Add(new HomeTab
                    {
                        Icon = "puzzle_icon.png",
                        dataCount = dashboard.Quizzes.ToString(),
                        dataTitle = "QUIZZES COMPLETED",
                        Color = "#17a2b8"
                    });
                    HomeList.Add(new HomeTab
                    {
                        Icon = "time_icon.png",
                        dataCount = dashboard.AvgTime.ToString(),
                        dataTitle = "TIME SPENT",
                        Color = "#28a745"
                    });
                    HomeList.Add(new HomeTab
                    {
                        Icon = "score_icon.png",
                        dataCount = dashboard.AvgScore.ToString(),
                        dataTitle = "AVG. SCORE",
                        Color = "#868e96"
                    });
                    HomeList.Add(new HomeTab
                    {
                        Icon = "star_icon.png",
                        dataCount = dashboard.TotalScore.ToString(),
                        dataTitle = "TOTAL SCORE",
                        Color = "#007bff"
                    });
                    HomeList.Add(new HomeTab
                    {
                        Icon = "game_icon.png",
                        dataCount = dashboard.LiveGames.ToString(),
                        dataTitle = "LIVE GAMES",
                        Color = "#dc3545"
                    });
                    HomeList.Add(new HomeTab
                    {
                        Icon = "notebook_icon.png",
                        dataCount = dashboard.Assignments.ToString(),
                        dataTitle = "ASSIGNMENTS",
                        Color = "#ffc107"
                    });
                    // HomeList = new ObservableCollection<HomeTab>(homeList);
                }
                if (dashboard != null && dashboard.QuizzesTable != null)
                {
                    QuizzesDataList.Clear();
                    foreach (var item in dashboard.QuizzesTable)
                    {
                        double Firstvalue = ((double)item.CorrectQuetions / item.TotalQuestions) * 100;
                        double Secondvalue = ((double)item.TotalQuestions - item.CorrectQuetions) / item.TotalQuestions * 100;
                        QuizzesDataList.Add(new QuizzesTable
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CorrectQuetions = item.CorrectQuetions,
                            QuestionsCompleted = item.QuestionsCompleted,
                            QuizFinished = item.QuizFinished,
                            QuizStarted = item.QuizStarted,
                            TotalQuestions = item.TotalQuestions,
                            TotalScore = item.TotalScore,
                            Type = item.Type,
                            BarValue = (double)((item.QuestionsCompleted / item.TotalQuestions) * 100),
                            FirstBarBounds = new Rectangle(0, 0, (double)item.QuestionsCompleted / item.TotalQuestions, 1),
                            CorrectFirstBarValue = (int)Firstvalue,
                            CorrectFirstBarBounds = new Rectangle(0, 0, (double)item.CorrectQuetions / item.TotalQuestions, 1),
                            CorrectSecondBarValue = (int)Secondvalue,
                            CorrectSecondBarBounds = new Rectangle(1, 0, (double)(item.TotalQuestions - item.CorrectQuetions) / item.TotalQuestions, 1),
                            ProgressLabel = item.QuestionsCompleted.ToString() + " / " + item.TotalQuestions.ToString(),
                            CorrectLabel = item.CorrectQuetions.ToString() + " / " + item.TotalQuestions.ToString()
                        });
                    }
                }
                if (dashboard != null && dashboard.AssignmentTable != null)
                {
                    AssignmentDataList.Clear();
                    foreach (var item in dashboard.AssignmentTable)
                    {
                        double Firstvalue = ((double)item.CorrectQuetions / item.TotalQuestions) * 100;
                        double Secondvalue = ((double)item.TotalQuestions - item.CorrectQuetions) / item.TotalQuestions * 100;
                        AssignmentDataList.Add(new AssignmentTable
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CorrectQuetions = item.CorrectQuetions,
                            QuestionsCompleted = item.QuestionsCompleted,
                            QuizFinished = item.QuizFinished,
                            QuizStarted = item.QuizStarted,
                            TotalQuestions = item.TotalQuestions,
                            TotalScore = item.TotalScore,
                            Type = item.Type,
                            BarValue = (double)((item.QuestionsCompleted / item.TotalQuestions) * 100),
                            FirstBarBounds = new Rectangle(0, 0, (double)item.QuestionsCompleted / item.TotalQuestions, 1),
                            CorrectFirstBarValue = (int)Firstvalue,
                            CorrectFirstBarBounds = new Rectangle(0, 0, (double)item.CorrectQuetions / item.TotalQuestions, 1),
                            CorrectSecondBarValue = (int)Secondvalue,
                            CorrectSecondBarBounds = new Rectangle(1, 0, (double)(item.TotalQuestions - item.CorrectQuetions) / item.TotalQuestions, 1),
                            ProgressLabel = item.QuestionsCompleted.ToString() + " / " + item.TotalQuestions.ToString(),
                            CorrectLabel = item.CorrectQuetions.ToString() + " / " + item.TotalQuestions.ToString()
                        });
                    }
                }
                if (dashboard != null && dashboard.LiveGameTable != null)
                {
                    LiveGameDataList.Clear();
                    foreach (var item in dashboard.LiveGameTable)
                    {
                        double Firstvalue = ((double)item.CorrectQuetions / item.TotalQuestions) * 100;
                        double Secondvalue = ((double)item.TotalQuestions - item.CorrectQuetions) / item.TotalQuestions * 100;
                        LiveGameDataList.Add(new LiveGameTable
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CorrectQuetions = item.CorrectQuetions,
                            QuestionsCompleted = item.QuestionsCompleted,
                            QuizFinished = item.QuizFinished,
                            QuizStarted = item.QuizStarted,
                            TotalQuestions = item.TotalQuestions,
                            TotalScore = item.TotalScore,
                            BarValue = (double)((item.QuestionsCompleted / item.TotalQuestions) * 100),
                            FirstBarBounds = new Rectangle(0, 0, (double)item.QuestionsCompleted / item.TotalQuestions, 1),
                            CorrectFirstBarValue = (int)Firstvalue,
                            CorrectFirstBarBounds = new Rectangle(0, 0, (double)item.CorrectQuetions / item.TotalQuestions, 1),
                            CorrectSecondBarValue = (int)Secondvalue,
                            CorrectSecondBarBounds = new Rectangle(1, 0, (double)(item.TotalQuestions - item.CorrectQuetions) / item.TotalQuestions, 1),
                            Type = item.Type,
                            ProgressLabel = item.QuestionsCompleted.ToString() + " / " + item.TotalQuestions.ToString(),
                            CorrectLabel = item.CorrectQuetions.ToString() + " / " + item.TotalQuestions.ToString()
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

        //Dashboard home Tab Click
        public DelegateCommand Home => new DelegateCommand(async () =>
        {
            if (!IsHomeList)
            {
                IsHomeList = true;
                IsQuizzesList = false;
                IsLiveGameTabList = false;
                IsAssignmentTabList = false;
                HomeBGcolor = Color.FromHex("#f37737");
                QuizzesBGcolor = Color.FromHex("#C1C1C1");
                LivegamesBGcolor = Color.FromHex("#C1C1C1");
                Assignments = Color.FromHex("#C1C1C1");
            }
        });

        //Dashboard Quizzes Tab Click
        public DelegateCommand Quizzes => new DelegateCommand(async () =>
        {
            if (!IsQuizzesList)
            {
                IsHomeList = false;
                IsQuizzesList = true;
                IsLiveGameTabList = false;
                IsAssignmentTabList = false;
                HomeBGcolor = Color.FromHex("#C1C1C1");
                QuizzesBGcolor = Color.FromHex("#f37737");
                LivegamesBGcolor = Color.FromHex("#C1C1C1");
                Assignments = Color.FromHex("#C1C1C1");
            }
        });

        //Dashboard Livegames Tab Click
        public DelegateCommand Livegames => new DelegateCommand(async () =>
        {
            if (!IsLiveGameTabList)
            {
                IsHomeList = false;
                IsQuizzesList = false;
                IsLiveGameTabList = true;
                IsAssignmentTabList = false;
                HomeBGcolor = Color.FromHex("#C1C1C1");
                QuizzesBGcolor = Color.FromHex("#C1C1C1");
                LivegamesBGcolor = Color.FromHex("#f37737");
                Assignments = Color.FromHex("#C1C1C1");
            }
        });

        //Dashboard AssignmentsTap Tab Click
        public DelegateCommand AssignmentsTap => new DelegateCommand(async () =>
        {
            if (!IsAssignmentTabList)
            {
                IsHomeList = false;
                IsQuizzesList = false;
                IsLiveGameTabList = false;
                IsAssignmentTabList = true;
                HomeBGcolor = Color.FromHex("#C1C1C1");
                QuizzesBGcolor = Color.FromHex("#C1C1C1");
                LivegamesBGcolor = Color.FromHex("#C1C1C1");
                Assignments = Color.FromHex("#f37737");
            }
        });

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

        }
    }
}
