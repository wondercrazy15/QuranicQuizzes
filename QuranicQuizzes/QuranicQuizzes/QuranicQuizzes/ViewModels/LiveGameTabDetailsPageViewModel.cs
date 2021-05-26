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
using Xamarin.Forms;
using static QuranicQuizzes.Models.DashboardDetails;

namespace QuranicQuizzes.ViewModels
{
    //LiveGameTabDetails PageViewModel
    public class LiveGameTabDetailsPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        string qid;

        public ObservableCollection<DashboardQuizze> DetailsList { get; set; } = new ObservableCollection<DashboardQuizze>();

        public ObservableCollection<LeaderBoards> LeaderBoardList { get; set; } = new ObservableCollection<LeaderBoards>();

        private Color _scoreBGcolor { get; set; }
        public Color ScoreBGcolor
        {
            get
            {
                return _scoreBGcolor;

            }
            set
            {
                _scoreBGcolor = value;
                RaisePropertyChanged(nameof(ScoreBGcolor));
                //SelectedItem();
            }
        }

        private Color _leaderBoardBGcolor { get; set; }
        public Color LeaderBoardBGcolor
        {
            get
            {
                return _leaderBoardBGcolor;

            }
            set
            {
                _leaderBoardBGcolor = value;
                RaisePropertyChanged(nameof(LeaderBoardBGcolor));
                //SelectedItem();
            }
        }

        private bool _isVisibleScore { get; set; }
        public bool IsVisibleScore
        {
            get
            {
                return _isVisibleScore;

            }
            set
            {
                _isVisibleScore = value;
                RaisePropertyChanged(nameof(IsVisibleScore));
                //SelectedItem();
            }
        }

        private bool _isVisibleLeaderboard { get; set; }
        public bool IsVisibleLeaderboard
        {
            get
            {
                return _isVisibleLeaderboard;

            }
            set
            {
                _isVisibleLeaderboard = value;
                RaisePropertyChanged(nameof(IsVisibleLeaderboard));
                //SelectedItem();
            }
        }

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

        public LiveGameTabDetailsPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            IsVisibleLeaderboard = false;
            IsVisibleScore = true;
            ScoreBGcolor = Color.FromHex("#f37737");
            LeaderBoardBGcolor = Color.FromHex("#C1C1C1");
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

        //LiveGameTab Details ScoreTap LeaderShip
        public DelegateCommand ScoreTap => new DelegateCommand(async () =>
        {
            if (!IsVisibleScore)
            {
                ScoreBGcolor = Color.FromHex("#f37737");
                LeaderBoardBGcolor = Color.FromHex("#C1C1C1");
                IsVisibleScore = true;
                IsVisibleLeaderboard = false;
               // IsBusy = true;
               // await CategoryApiCall();
               // IsBusy = false;
            }
        });

        //LiveGameTab Details LeaderShip 
        public DelegateCommand LeaderBoardTap => new DelegateCommand(async () =>
        {
            if (!IsVisibleLeaderboard)
            {
                LeaderBoardBGcolor = Color.FromHex("#f37737");
                ScoreBGcolor = Color.FromHex("#C1C1C1");
                IsVisibleScore = false;
                IsVisibleLeaderboard = true;
               // IsBusy = true;
               // await CoursesApiCall();
               // IsBusy = false;
            }

        });

        //Api call for LeaderBoard
        private async Task dataBinding()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                DashboardDetails details = new DashboardDetails();

                details = await _clientAPI.getDashboardDetails(qid);
                if (details != null)
                {
                    if (details.Quizzes != null)
                    {
                        foreach (var item in details.Quizzes)
                        {
                            DetailsList.Add(item);
                        }
                    }

                    if (details.LeaderBoard != null)
                    {
                        for (int i = 0; i < details.LeaderBoard.Count; i++)
                        {
                            LeaderBoards items = details.LeaderBoard[i];

                            LeaderBoardList.Add(new LeaderBoards
                            {
                                Pos = i + 1,
                                Name = items.Name,
                                TotalScore = items.TotalScore
                            });
                        }
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
