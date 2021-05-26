using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.SimpleAudioPlayer;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace QuranicQuizzes.ViewModels
{
    public class QuizzesQuestionPageViewModel : ViewModelBase
    {
        /// <summary>
        /// QuizzesQuestionPage ViewModel 
        /// </summary>

        INavigationService _navigationService;
        IClientAPI _clientAPI;
        ISimpleAudioPlayer player ;
        public Command<object> SelectedQuizeAnswer { get; set; }

        public QuizzesQuestionPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                _clientAPI = clientAPI;
                SelectedQuizeAnswer = new Command<object>(SelectedQuizeAnswerInfo);
                

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


        public string _Result;
        public string Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
                RaisePropertyChanged(nameof(Result));
            }
        }


        public string _ResultImg;
        public string ResultImg
        {
            get
            {
                return _ResultImg;
            }
            set
            {
                _ResultImg = value;
                RaisePropertyChanged(nameof(ResultImg));
            }
        }

        public string _LblQuestionAnswer;
        public string LblQuestionAnswer
        {
            get
            {
                return _LblQuestionAnswer;
            }
            set
            {
                _LblQuestionAnswer = value;
                RaisePropertyChanged(nameof(LblQuestionAnswer));
            }
        }

        public string _LblTapContinue;
        public string LblTapContinue
        {
            get
            {
                return _LblTapContinue;
            }
            set
            {
                _LblTapContinue = value;
                RaisePropertyChanged(nameof(LblTapContinue));
            }
        }

        public DateTime _StartTimeOfQuestion;
        public DateTime startTimeOfQuestion
        {
            get
            {
                return _StartTimeOfQuestion;
            }
            set
            {
                _StartTimeOfQuestion = value;
                RaisePropertyChanged(nameof(startTimeOfQuestion));
            }
        }
        public DateTime _EndTimeOfQuestion;
        public DateTime endTimeOfQuestion
        {
            get
            {
                return _EndTimeOfQuestion;
            }
            set
            {
                _EndTimeOfQuestion = value;
                RaisePropertyChanged(nameof(endTimeOfQuestion));
            }
        }

        public double _TotalScore;
        public double TotalScore
        {
            get
            {
                return _TotalScore;
            }
            set
            {
                _TotalScore = value;
                RaisePropertyChanged(nameof(TotalScore));
            }
        }
        public int _CorrectQuestion;
        public int CorrectQuestion
        {
            get
            {
                return _CorrectQuestion;
            }
            set
            {
                _CorrectQuestion = value;
                RaisePropertyChanged(nameof(CorrectQuestion));
            }
        }
        public double _point;
        public double point
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



        public int _currentQuestion;
        public int CurrentQuestion
        {
            get
            {
                return _currentQuestion;
            }
            set
            {
                _currentQuestion = value;
                RaisePropertyChanged(nameof(CurrentQuestion));
            }
        }
        Answer QuestionAns = new Answer();
        int Question = 0;
     
        private async void SelectedQuizeAnswerInfo(object obj)
        {
            try
            {
                if (obj != null)
                {
                    if (QuestionAns != obj as Answer && Question != data.ID)
                    {
                        Question = data.ID;
                        QuestionAns = obj as Answer;
                        //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        //if (CrossMediaManager.Current.IsPlaying())
                        //    await CrossMediaManager.Current.Stop();

                        endTimeOfQuestion = DateTime.Now;
                        var timedifference = (endTimeOfQuestion - startTimeOfQuestion).TotalMilliseconds;
                        if (QuestionAns.IsAnswer && timedifference <= 15100)
                        {
                            point = getPoints(timedifference);
                            Result = "Correct";
                            ResultBackground = "#47c9a2";
                            ResultImg = "Round_yes";
                        }
                        else if (QuestionAns.IsAnswer && timedifference > 15100)
                        {
                            point = 700;
                            Result = "Correct";
                            ResultBackground = "#47c9a2";
                            ResultImg = "Round_yes";
                        }
                        else
                        {
                            point = 0;
                            Result = "Incorrect";
                            ResultBackground = "#ee3535";
                            ResultImg = "Round_close";
                        }
                        if (QuestionAns.IsAnswer)
                        {
                            TotalScore += point;
                            CorrectQuestion++;
                        }
                        int index = QuizzesTypesQuestions.Questions.FindIndex(s => s.ID.Equals(data.ID));
                        int AnsIndex = QuizzesTypesQuestions.Questions[index].Answers.FindIndex(s => s.QuestionID.Equals(QuestionAns.QuestionID));
                        if (index != -1)
                        {
                            QuizzesTypesQuestions.Questions[index].point = point;
                            QuizzesTypesQuestions.Questions[index].Answers[AnsIndex].IsAnswerSelected = true;
                            data.point = point;
                            data.Answers[AnsIndex].IsAnswerSelected = true;
                        }
                        CurrentQuestion++;
                        if (GlobalConst.istestMode)
                        {
                           // IsQuestionVisible = true;
                            IsQuestionAnswerInfoVisible = false;
                            var Respo = await _clientAPI.GetEvaluateAnswer(GlobalConst.Session, data.ID, QuestionAns.ID, (int)point);
                            if (CurrentQuestion < QuizzesTypesQuestions.Questions.Count)
                            {
                                await SetQuestionData();
                                MessagingCenter.Send<object>(this, "StartProgress");
                            }
                            else
                            {
                                var parameters = new NavigationParameters();
                                parameters.Add("EndGameResult", CorrectQuestion.ToString());
                                parameters.Add("TotalPoints", TotalScore.ToString());
                                parameters.Add("Categories", Category);
                                parameters.Add("SessionInfo", SessionInfo);
                                parameters.Add("PlayTab", Quizzesdata);
                                parameters.Add("QuestionsList", QuizzesTypesQuestions);
                                parameters.Add("Coureses", Coureses);
                                parameters.Add("SubCategories", LvSelectedSubCategoryName);
                                await _navigationService.NavigateAsync(nameof(EndQuizTestModePage), parameters);
                            }
                        }
                        else
                        {
                            var Respo = await _clientAPI.GetEvaluateAnswer(GlobalConst.Session, data.ID, QuestionAns.ID, (int)point);
                            if (IsSoundURL)
                            {
                                if (player.IsPlaying)
                                {
                                    player.Stop();
                                }
                            }
                            
                            IsQuestionVisible = false;
                            IsQuestionAnswerInfoVisible = true;
                            MessagingCenter.Send<object>(this, "ShowResultPage");
                            LblTapContinue = "Tap to Continue";
                            LblQuestionAnswer = data.Answers.Where(x => x.IsAnswer == true).FirstOrDefault().AnswerText;
                        }
                      
                    }
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

        public DelegateCommand SoundStop => new DelegateCommand(async () =>
        {
            if (IsSoundURL)
            {
                if (player.IsPlaying)
                {
                    player.Stop();
                }
            }
            //await CrossMediaManager.Current.Stop();
        });

        private List<Answer> shuffle(List<Answer> a)
        {
            for (var i = a.Count - 1; i > 0; i--)
            {
                var rand = new Random();
                int j = (int)Math.Floor((double)rand.Next(0, a.Count - 1));
                var x = a[i];
                a[i] = a[j];
                a[j] = x;
            }
            return a;
        }

        //Set new Question with answer
        private async Task SetQuestionData()
        {
            try
            {

                startTimeOfQuestion = DateTime.Now;
                int QuestionCount = QuizzesTypesQuestions.Questions.Count;
                TotalQuestionCount = (CurrentQuestion + 1) + "/" + QuestionCount;

                QuizzesTypesQuestions.Questions[CurrentQuestion].Answers = shuffle(QuizzesTypesQuestions.Questions[CurrentQuestion].Answers);
                data = QuizzesTypesQuestions.Questions[CurrentQuestion];
                QuestionText = data.QuestionText;
                ImageText = data.ImageText;
                IsImageText = data.IsImageText;
                ImageURL = data.FinalImageURL;
                IsImageURL = data.IsImage;


                if (!string.IsNullOrEmpty(data.Notes))
                {
                    IsNotes = true;
                    Notes = data.Notes;
                }
                else
                {
                    IsNotes = false;
                }
                if (!string.IsNullOrEmpty(data.SoundURL))
                {
                    SoundURL = GlobalConst.ApiUrl + data.SoundURL;
                    IsSoundURL = true;
                    //CrossMediaManager.Current.Play(SoundURL);

                    player = CrossSimpleAudioPlayer.Current;
                    //string url = "some-url-string";
                    WebClient wc = new WebClient();
                    Stream fileStream = wc.OpenRead(SoundURL);
                    player.Load(fileStream);
                    player.Play();
                    
                }
                QuizzesQuestionAnswersList.Clear();
                if (data.Answers.Count > 0)
                {
                    double BoxHeight = 0;


                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        // You're on a phone
                        if (IsASmallDevice())
                            BoxHeight = 45;
                        else
                            BoxHeight = 55;

                    }
                    else
                    {
                        // You're on a tablet
                        BoxHeight = 70;
                    }


                    var count = 0;
                    foreach (var item in data.Answers)
                    {
                        if (string.IsNullOrEmpty(item.AnswerText))
                            count++;
                    }
                    if (count == 2 || data.Answers.Count == 2)
                    {
                        //BoxHeight = 60;
                        //if (string.IsNullOrEmpty(data.Answers[2].AnswerText))
                        //{
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            // You're on a phone
                            if (IsASmallDevice())
                                BoxHeight = 100;
                            else
                                BoxHeight = 110;

                        }
                        else
                        {
                            // You're on a tablet
                            BoxHeight = 130;
                        }
                        
                        //}
                    }
                    else if (count == 1 || data.Answers.Count == 3)
                    {
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            // You're on a phone
                            if (IsASmallDevice())
                                BoxHeight = 63;
                            else
                                BoxHeight = 70;

                        }
                        else
                        {
                            // You're on a tablet
                            BoxHeight = 90;
                        }
                        
                    }
                    if (data.Answers[0] != null && !string.IsNullOrEmpty(data.Answers[0].AnswerText))
                    {

                        QuizzesQuestionAnswersList.Add(new Answer
                        {
                            ID = data.Answers[0].ID,
                            QuestionID = data.Answers[0].QuestionID,
                            AnswerText = data.Answers[0].AnswerText,
                            BorderColor = "#E71C08",
                            BackgroundColor = "#ee4035",
                            IsAnswer = data.Answers[0].IsAnswer,
                            IsArabic = data.Answers[0].IsArabic,
                            Heights = BoxHeight,
                        });
                    }
                    if (data.Answers[1] != null && !string.IsNullOrEmpty(data.Answers[1].AnswerText))
                    {
                        QuizzesQuestionAnswersList.Add(new Answer
                        {
                            ID = data.Answers[1].ID,
                            QuestionID = data.Answers[1].QuestionID,
                            AnswerText = data.Answers[1].AnswerText,
                            BorderColor = "#D45C00",
                            BackgroundColor = "#f37736",
                            IsAnswer = data.Answers[1].IsAnswer,
                            IsArabic = data.Answers[1].IsArabic,
                            Heights = BoxHeight,
                        });
                    }
                    if (data.Answers.Count > 2)
                    {
                        if (data.Answers[2] != null && !string.IsNullOrEmpty(data.Answers[2].AnswerText))
                        {
                            QuizzesQuestionAnswersList.Add(new Answer
                            {
                                ID = data.Answers[2].ID,
                                QuestionID = data.Answers[2].QuestionID,
                                AnswerText = data.Answers[2].AnswerText,
                                BorderColor = "#3D4BAB",
                                BackgroundColor = "#415db7",
                                IsAnswer = data.Answers[2].IsAnswer,
                                IsArabic = data.Answers[2].IsArabic,
                                Heights = BoxHeight,
                            });
                        }
                    }
                    if (data.Answers.Count > 3)
                    {
                        if (data.Answers[3] != null && !string.IsNullOrEmpty(data.Answers[3].AnswerText))
                        {
                            QuizzesQuestionAnswersList.Add(new Answer
                            {
                                ID = data.Answers[3].ID,
                                QuestionID = data.Answers[3].QuestionID,
                                AnswerText = data.Answers[3].AnswerText,
                                BorderColor = "#2D912D",
                                BackgroundColor = "#58a441",
                                IsAnswer = data.Answers[3].IsAnswer,
                                IsArabic = data.Answers[3].IsArabic,
                                Heights = BoxHeight,
                            });
                        }
                    }

                    MessagingCenter.Send<object>(this, "StartProgress");
                  

                    UserDialogs.Instance.HideLoading();
                    if (CurrentQuestion != 0)
                    {
                        MessagingCenter.Send<object>(this, "ShowResultPage");
                    }
                }
                

                // IsQuestionAnswerInfoVisible = false;
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



        private int getPoints(double millisecond)
        {
            double p = (Math.Max(0, 15000 - millisecond) * 0.02) + 700;
            return (int)p;
        }
        public async void Back()
        {
            try
            {
               
                var parameters = new NavigationParameters();
                parameters.Add("Categories", Category);
                parameters.Add("Coureses", Coureses);
                parameters.Add("SubCategories", LvSelectedSubCategoryName);

                if (assignmentData == null)
                {
                    if (GlobalConst.isCourse)
                    {
                        if (GlobalConst.isLearnTab)
                            await _navigationService.NavigateAsync("../../../../QuizzesByCoursesPage", parameters);
                        else
                            await _navigationService.NavigateAsync("../../../QuizzesByCoursesPage", parameters);
                    }
                    else
                    {
                        if (GlobalConst.isLearnTab)
                            await _navigationService.NavigateAsync("../../../../QuizzesByCategoriesPage", parameters);
                        else
                            await _navigationService.NavigateAsync("../../../QuizzesByCategoriesPage", parameters);
                    }
                }
                else
                {
                    //await _navigationService.NavigateAsync(nameof(AssignmentPage));
                    if(GlobalConst.isNewAssignments)
                        await _navigationService.NavigateAsync("../../../NewAssignmentPage");
                    else
                        await _navigationService.NavigateAsync("../../../AssignmentPage");
                }
                if (IsSoundURL)
                {
                    if (player.IsPlaying)
                    {
                        player.Stop();
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

       

        //Info Button click
        public DelegateCommand InfoClick => new DelegateCommand(async () =>
        {
            try
            {
                //await UserDialogs.Instance.AlertAsync(Notes, "Note", "Ok");
                await PopupNavigation.Instance.PushAsync(new NotePopupPage(Notes, SourceURL));
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
        //Close Click
        public DelegateCommand CloseTap => new DelegateCommand(async () =>
        {
            try
            {
                Back();

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


        public DelegateCommand ClickContinue => new DelegateCommand(async () =>
        {
            try
            {
                if (CurrentQuestion < QuizzesTypesQuestions.Questions.Count)
                {
                    IsQuestionVisible = true;
                    ImageURL = string.Empty;
                    //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                  //  IsQuestionAnswerInfoVisible = false;
                    await SetQuestionData();
                 
                    MessagingCenter.Send<object>(this, "StartProgress");

                }
                else
                {
                    var parameters = new NavigationParameters();
                    string RightAns = CorrectQuestion.ToString();
                    parameters.Add("EndGameResult", RightAns);
                    parameters.Add("TotalPoints", TotalScore.ToString());
                    parameters.Add("Categories", Category);
                    parameters.Add("SessionInfo", SessionInfo);
                    parameters.Add("PlayTab", Quizzesdata);
                    parameters.Add("QuestionsList", QuizzesTypesQuestions);
                    parameters.Add("Coureses", Coureses);
                    parameters.Add("assignmentData", assignmentData);
                    parameters.Add("Coureses", Coureses);
                    parameters.Add("SubCategories", LvSelectedSubCategoryName);
                    //Coures Coureses = new Coures();
                    await _navigationService.NavigateAsync(nameof(EndQuizPage), parameters);
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


        public DelegateCommand IsShowHintCommand => new DelegateCommand(async () =>
        {
            try
            {
               await PopupNavigation.Instance.PushAsync(new WebPopupPage(SourceURLHeaderName, SourceURL));
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

        public DelegateCommand SoundTap => new DelegateCommand(async () =>
        {
            // await CrossMediaManager.Current.Play(SoundURL);
            try
            {
                player.Play();
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


        private ObservableCollection<QuizzesQuestionsDesignList> _QuizzesAnswersList = new ObservableCollection<QuizzesQuestionsDesignList>();
        public ObservableCollection<QuizzesQuestionsDesignList> QuizzesAnswersList
        {
            get { return _QuizzesAnswersList; }
            set
            {
                _QuizzesAnswersList = value;
                RaisePropertyChanged("QuizzesAnswersList");
            }
        }


        private ObservableCollection<Answer> _QuizzesQuestionAnswersList = new ObservableCollection<Answer>();
        public ObservableCollection<Answer> QuizzesQuestionAnswersList
        {
            get { return _QuizzesQuestionAnswersList; }
            set
            {
                _QuizzesQuestionAnswersList = value;
                RaisePropertyChanged("QuizzesQuestionAnswersList");
            }
        }

        public string _TotalQuestionCount;
        public string TotalQuestionCount
        {
            get
            {
                return _TotalQuestionCount;
            }

            set
            {
                _TotalQuestionCount = value;
                RaisePropertyChanged(nameof(TotalQuestionCount));
            }
        }

        public string _QuestionText;
        public string QuestionText
        {
            get
            {
                return _QuestionText;
            }

            set
            {
                _QuestionText = value;
                RaisePropertyChanged(nameof(QuestionText));
            }
        }



        public bool _IsQuestionAnswerInfoVisible;
        public bool IsQuestionAnswerInfoVisible
        {
            get
            {
                return _IsQuestionAnswerInfoVisible;
            }

            set
            {
                _IsQuestionAnswerInfoVisible = value;
                RaisePropertyChanged(nameof(IsQuestionAnswerInfoVisible));
            }
        }

        public bool _IsQuestionVisible;
        public bool IsQuestionVisible
        {
            get
            {
                return _IsQuestionVisible;
            }

            set
            {
                _IsQuestionVisible = value;
                RaisePropertyChanged(nameof(IsQuestionVisible));
            }
        }

        public bool _IsQuestionText;
        public bool IsQuestionText
        {
            get
            {
                return _IsQuestionText;
            }

            set
            {
                _IsQuestionText = value;
                RaisePropertyChanged(nameof(IsQuestionText));
            }
        }

        public string _ImageText;
        public string ImageText
        {
            get
            {
                return _ImageText;
            }

            set
            {
                _ImageText = value;
                RaisePropertyChanged(nameof(ImageText));
            }
        }

        public bool _IsImageText;
        public bool IsImageText
        {
            get
            {
                return _IsImageText;
            }

            set
            {
                _IsImageText = value;
                RaisePropertyChanged(nameof(IsImageText));
            }
        }

        public string _ImageURL;
        public string ImageURL
        {
            get
            {
                return _ImageURL;
            }

            set
            {
                _ImageURL = value;
                RaisePropertyChanged(nameof(ImageURL));
            }
        }

        public bool _IsImageURL;
        public bool IsImageURL
        {
            get
            {
                return _IsImageURL;
            }

            set
            {
                _IsImageURL = value;
                RaisePropertyChanged(nameof(IsImageURL));
            }
        }

        public string _Notes;
        public string Notes
        {
            get
            {
                return _Notes;
            }

            set
            {
                _Notes = value;
                RaisePropertyChanged(nameof(Notes));
            }
        }


        public bool _IsNotes;
        public bool IsNotes
        {
            get
            {
                return _IsNotes;
            }

            set
            {
                _IsNotes = value;
                RaisePropertyChanged(nameof(IsNotes));
            }
        }



        public string _ResultBackground;
        public string ResultBackground
        {
            get
            {
                return _ResultBackground;
            }
            set
            {
                _ResultBackground = value;
                RaisePropertyChanged(nameof(ResultBackground));
            }
        }

        public string _SoundURL;
        public string SoundURL
        {
            get
            {
                return _SoundURL;
            }
            set
            {
                _SoundURL = value;
                RaisePropertyChanged(nameof(SoundURL));
            }
        }

        public bool _IsSoundURL;
        public bool IsSoundURL
        {
            get
            {
                return _IsSoundURL;
            }
            set
            {
                _IsSoundURL = value;
                RaisePropertyChanged(nameof(IsSoundURL));
            }
        }
        public bool _IsShowHint;
        public bool IsShowHint
        {
            get
            {
                return _IsShowHint;
            }
            set
            {
                _IsShowHint = value;
                RaisePropertyChanged(nameof(IsShowHint));
            }
        }

        QuizzesQuestions QuizzesTypesQuestions = new QuizzesQuestions();
        Question data = new Question();
        Quizze Quizzesdata = new Quizze();
        CheckSession SessionInfo = new CheckSession();
        Coures Coureses = new Coures();
        string SourceURL;
        string SourceURLHeaderName;
        Assignment assignmentData = new Assignment();
        int CourseId = 0;
        int QuizID = 0;
        int HWID = 0;
        int CategoryID = 0;
        SubCategories LvSelectedSubCategoryName = new SubCategories();

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
               
                IsQuestionAnswerInfoVisible = false;
                Category = parameters["Categories"] as Categories;
                Quizzesdata = parameters["PlayTab"] as Quizze;
                SessionInfo = parameters["SessionInfo"] as CheckSession;
                QuizzesTypesQuestions = parameters["QuestionsList"] as QuizzesQuestions;
                Coureses = parameters["Coureses"] as Coures;
                assignmentData = parameters["assignmentData"] as Assignment;

                LvSelectedSubCategoryName = parameters["SubCategories"] as SubCategories;
                if (assignmentData != null)
                {
                    QuizID = assignmentData.ID;
                }
                if (Quizzesdata != null)
                {
                    QuizID = Quizzesdata.Id;
                }
                //Coures Coureses = new Coures();
                //parameters.Add("SourceURL", SourceURL);
                if (QuizzesTypesQuestions != null)
                {
                    if (Quizzesdata != null)
                    {
                        if (Quizzesdata.ShowHint)
                        {
                            IsShowHint = true;
                            SourceURL = "http://quranicquizzes.com/" + "Quizzes/LearnApp/" + QuizID + "?" + GlobalConst.ApiUrlKey + "&iscourse=" + GlobalConst.isCourse;
                            SourceURLHeaderName = Quizzesdata.Name;
                        }
                    }

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        await SetQuestionData();
                    }
                    else
                    {
                        ShowMessage("Please check your internet connection");
                    }
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

    }
}