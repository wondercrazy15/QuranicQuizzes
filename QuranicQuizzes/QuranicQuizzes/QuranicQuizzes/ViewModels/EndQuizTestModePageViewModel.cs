using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    //EndQuiz TestModePage ViewModel
    public class EndQuizTestModePageViewModel : ViewModelBase
    {

        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public Command<object> CategoryClickCommand { get; set; }
        public Command<object> CourseListCommand { get; set; }
        public EndQuizTestModePageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            if (CrossConnectivity.Current.IsConnected)
            {
                Apicall();
            }
            else
            {
                ShowMessage("Please check your internet connection");
            }
            
        }

        // Api call of EndQuiz TestMode
        private async void Apicall()
        {
            var data = await _clientAPI.EndQuiz(GlobalConst.Session);
        }

        Quizze Quizzesdata = new Quizze();
        CheckSession SessionInfo = new CheckSession();
        QuizzesQuestions QuizzesTypesQuestions = new QuizzesQuestions();
        Coures Coureses = new Coures();
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                Quizzesdata = parameters["PlayTab"] as Quizze;
                SessionInfo = parameters["SessionInfo"] as CheckSession;
                Category = parameters["Categories"] as Categories;
                point = parameters["TotalPoints"] as string;
               
                Coureses = parameters["Coureses"] as Coures;
                
                var RightAns = parameters["EndGameResult"] as string;

                QuizzesTypesQuestions = parameters["QuestionsList"] as QuizzesQuestions;
                if (QuizzesTypesQuestions != null)
                {
                    RightQuestions = RightAns + " out of " + QuizzesTypesQuestions.Questions.Count;
                    BackGroundColor = "#00ced1";
                    Percentages = (Math.Floor((decimal.Parse(RightAns)/QuizzesTypesQuestions.Questions.Count)*100)).ToString()+"%";
                    //Percentages =
                    var p=(Math.Floor((decimal.Parse(RightAns) / QuizzesTypesQuestions.Questions.Count) * 100));
                    if(p>=85)
                    {
                        PassFail = "Pass";
                        BackGroundColor = "#47c9a2";
                    }
                    else
                    {
                        PassFail = "Fail";
                        BackGroundColor = "#ee3535";
                    }
                    QuizzesQuestionAnswersList = new ObservableCollection<Question>(QuizzesTypesQuestions.Questions);
                    
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

        private ObservableCollection<Question> _QuizzesQuestionAnswersList = new ObservableCollection<Question>();
        public ObservableCollection<Question> QuizzesQuestionAnswersList
        {
            get { return _QuizzesQuestionAnswersList; }
            set
            {
                _QuizzesQuestionAnswersList = value;
                RaisePropertyChanged("QuizzesQuestionAnswersList");
            }
        }

        private Categories _Category { get; set; }
        public Categories Category
        {
            get
            {
                return _Category;
            }
            set
            {
                _Category = value;
                RaisePropertyChanged(nameof(Category));
            }
        }

        public async void Back()
        {
            try
            {
                var parameters = new NavigationParameters();
                parameters.Add("Categories", Category);
                parameters.Add("Coureses", Coureses);


                if (GlobalConst.isCourse)
                {

                    if (GlobalConst.isLearnTab)
                        await _navigationService.NavigateAsync("../../../../../QuizzesByCoursesPage", parameters);
                    else

                        await _navigationService.NavigateAsync("../../../../QuizzesByCoursesPage", parameters);
                }
                else
                {
                    if (GlobalConst.isLearnTab)
                        await _navigationService.NavigateAsync("../../../../../QuizzesByCategoriesPage", parameters);
                    else

                        await _navigationService.NavigateAsync("../../../../QuizzesByCategoriesPage", parameters);
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

        //PlayAgain Click
        public DelegateCommand PlayAgainClickCommand => new DelegateCommand(async () =>
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                var parameters = new NavigationParameters();
                parameters.Add("Categories", Category);
                parameters.Add("SessionInfo", SessionInfo);
                parameters.Add("PlayTab", Quizzesdata);
                parameters.Add("Coureses", Coureses);
             
                if (GlobalConst.isLearnTab)
                    await _navigationService.NavigateAsync("../../../../PlayTabPage", parameters);
                else
                    await _navigationService.NavigateAsync("../../../PlayTabPage", parameters);

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

        });


        public DelegateCommand CloseQuizze => new DelegateCommand(async () =>
        {
            Back();
        });

        public string _BackGroundColor;
        public string BackGroundColor
        {
            get
            {
                return _BackGroundColor;
            }

            set
            {
                _BackGroundColor = value;
                RaisePropertyChanged(nameof(BackGroundColor));
            }
        }

        
        
        public string _point;
        public string point
        {
            get
            {
                return _point;
            }

            set
            {
                _point = value;
                RaisePropertyChanged(nameof(point));
            }
        }
        

        public string _PassFail;
        public string PassFail
        {
            get
            {
                return _PassFail;
            }

            set
            {
                _PassFail = value;
                RaisePropertyChanged(nameof(PassFail));
            }
        }

        public string _Percentages;
        public string Percentages
        {
            get
            {
                return _Percentages;
            }

            set
            {
                _Percentages = value;
                RaisePropertyChanged(nameof(Percentages));
            }
        }

        public string _RightQuestions;
        public string RightQuestions
        {
            get
            {
                return _RightQuestions;
            }

            set
            {
                _RightQuestions = value;
                RaisePropertyChanged(nameof(RightQuestions));
            }
        }

        public DelegateCommand twitterClick => new DelegateCommand(async () =>
        {
            await Xamarin.Essentials.Launcher.OpenAsync("https://twitter.com/QuranicQuizzes/");
        });
        public DelegateCommand facebookClick => new DelegateCommand(async () =>
        {
            await Xamarin.Essentials.Launcher.OpenAsync("https://www.facebook.com/QuranicQuizzes/");
        });
        public DelegateCommand instagramClick => new DelegateCommand(async () =>
        {
            //await Share.RequestAsync(new ShareTextRequest
            //{
            //    Text = "ff",
            //    Title = "Share Text"
            //});
            await Xamarin.Essentials.Launcher.OpenAsync("https://www.instagram.com/QuranicQuizzes/");
        });

    }
}
