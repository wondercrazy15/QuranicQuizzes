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
using QuranicQuizzes.Views;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace QuranicQuizzes.ViewModels
{
    public class PlayTabPageViewModel : ViewModelBase
    {
        /// <summary>
        /// PlayTabPage viewmodel
        /// </summary>
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public Command<object> SelectedQuizes { get; set; }
        public Command<object> InfoClickCommand { get; set; }
        public bool Resume=false;

        public PlayTabPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                _clientAPI = clientAPI;
                SelectedQuizes = new Command<object>(SelectedQuizesData);
                InfoClickCommand = new Command<object>(InfoClicksCommand);

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

        //Info Click
        private async void InfoClicksCommand(object obj)
        {
            if (obj != null)
            {
                var data = obj as QuizzesType;

                var alertDialogConfiguration = new MaterialAlertDialogConfiguration
                {
                    MessageTextColor = Color.Black,
                    MessageFontFamily = XF.Material.Forms.Material.GetResource<OnPlatform<string>>("QuranFont"),
                    TintColor = Color.Black
                };

                if (data.Id == 0)
                {
                    await MaterialDialog.Instance.AlertAsync(message: "This will change the order of questions in quiz", configuration: alertDialogConfiguration);
                }
                else if (data.Id == 1)
                {
                    await MaterialDialog.Instance.AlertAsync(message: "This removes any duplicate words in quiz.e.g.repeated words like “An - Nas” in surah An - Nas in Quran category", configuration: alertDialogConfiguration);
                }
                else if (data.Id == 2)
                {
                    await MaterialDialog.Instance.AlertAsync(message: "This removes answers as you go along and provides results at the end only", configuration: alertDialogConfiguration);
                }
            }
        }

        private bool _IsLableDisplay { get; set; }
        public bool IsLableDisplay
        {
            get => _IsLableDisplay;
            set
            {
                _IsLableDisplay = value;
                RaisePropertyChanged(nameof(IsLableDisplay));
            }
        }
        public string _BackgroundColor;
        public string BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }

            set
            {
                SetProperty(ref _BackgroundColor, value); 
            }
        }

        public string _IconImage;
        public string IconImage
        {
            get
            {
                return _IconImage;
            }

            set
            {
                SetProperty(ref _IconImage, value);
            }
        }

        //Selected Quizes type
        private void SelectedQuizesData(object obj)
        {
            try
            {
                if (obj != null)
                {
                    var data = obj as QuizzesType;
                    if (!data.IsEnables)
                    {
                        QuizzesType qt = new QuizzesType();
                        qt.Id = data.Id;
                        qt.Name = data.Name;
                        qt.IconImage = (data.IconImage).Equals("close.png") ? "done.png" : "close.png";
                        qt.BackgroundColor = (data.BackgroundColor).Equals("#868e96") ? "#28a745" : "#868e96";
                        qt.IsEnables = false;
                        qt.IsVisibles = data.IsVisibles;
                        var index = QuizzesTypes.IndexOf(data);
                        QuizzesTypes.RemoveAt(index);
                        QuizzesTypes.Insert(index, qt);
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

       // public ObservableCollection<QuizzesType> QuizzesTypes { get; set; } = new ObservableCollection<QuizzesType>();

        private ObservableCollection<QuizzesType> _QuizzesTypes= new ObservableCollection<QuizzesType>();
        public ObservableCollection<QuizzesType> QuizzesTypes
        {
            get { return _QuizzesTypes; }
            set
            {
                _QuizzesTypes = value;
                RaisePropertyChanged("QuizzesTypes");
            }
        }
        

        private bool _IsResume { get; set; }
        public bool IsResume
        {
            get => _IsResume;
            set
            {
                _IsResume = value;
                RaisePropertyChanged(nameof(IsResume));
            }
        }
        private bool _IsStartResume { get; set; }
        public bool IsStartResume
        {
            get => _IsStartResume;
            set
            {
                _IsStartResume = value;
                RaisePropertyChanged(nameof(IsStartResume));
            }
        }
        private bool _IsStart { get; set; }
        public bool IsStart
        {
            get => _IsStart;
            set
            {
                _IsStart = value;
                RaisePropertyChanged(nameof(IsStart));
            }
        }
        
        private bool _IsQuizzesList { get; set; }
        public bool IsQuizzesList
        {
            get => _IsQuizzesList;
            set
            {
                _IsQuizzesList = value;
                RaisePropertyChanged(nameof(IsQuizzesList));
            }
        }
        private bool _isBusy { get; set; }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }
        
        public string _QuizzesTypesHeight;
        public string QuizzesTypesHeight
        {
            get
            {
                return _QuizzesTypesHeight;
            }

            set
            {
                _QuizzesTypesHeight = value;
                RaisePropertyChanged(nameof(QuizzesTypesHeight));
            }
        }
        public string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string _StartResume;
        public string StartResume
        {
            get
            {
                return _StartResume;
            }

            set
            {
                _StartResume = value;
                RaisePropertyChanged(nameof(StartResume));
            }
        }
        Quizze Quizzesdata = new Quizze();
        CheckSession SessionInfo = new CheckSession();
        Categories Category = new Categories();
        Coures Coureses = new Coures();
        Assignment assignmentData = new Assignment();
        string SourceURL;
        int CourseId=0;
        int QuizID = 0;
        int HWID = 0;
        int CategoryID = 0;
        SubCategories LvSelectedSubCategoryName=new SubCategories();
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                GlobalConst.IsNotificationTap = false;
                GlobalConst.istestMode = false;
                IsStartResume = true;
                GlobalConst.IsNotificationTap = false;
                Quizzesdata = parameters["PlayTab"] as Quizze;
                assignmentData = parameters["assignmentData"] as Assignment;
                LvSelectedSubCategoryName = parameters["SubCategories"] as SubCategories;
                IsStart = true;
                if (Quizzesdata != null)
                {
                    QuizID = Quizzesdata.Id;
                    IsQuizzesList = true;
                    CategoryID = Quizzesdata.CategoryID;
                }
                if (assignmentData != null)
                {
                    QuizID = assignmentData.ID;
                    HWID = assignmentData.HWID;
                    Name = assignmentData.QuizName;
                    IsQuizzesList = false;
                    
                }
                CheckSession SessionInfo = await _clientAPI.GetCheckSession(QuizID, GlobalConst.isCourse, HWID);

                Category = parameters["Categories"] as Categories;
                SourceURL = parameters["SourceURL"] as string;
                Coureses = parameters["Coureses"] as Coures;
                
                if (Coureses!=null)
                {
                    CourseId = Coureses.ID;
                }

                if (Quizzesdata != null)
                {
                    Name = Quizzesdata.Name;
                }
                if (SessionInfo!=null && SessionInfo.HasSession)
                {

                    StartResume = "Restart";
                    IsResume = true;
                    if (assignmentData != null)
                    {
                       
                        IsStartResume = false;
                    }
                }
                else
                {
                    StartResume = "Play";
                    IsResume = false;
                    
                }
                QuizzesTypes.Clear();
                if (Quizzesdata != null)
                {
                    if (Quizzesdata.TestOnly)
                    {
                        if (GlobalConst.isCourse)
                        {
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                // You're on a phone
                                if (IsASmallDevice())
                                    QuizzesTypesHeight = "150";
                                else
                                    QuizzesTypesHeight = "155";
                               
                            }
                            else
                            {
                                // You're on a tablet
                                QuizzesTypesHeight = "170";
                            }
                            
                            QuizzesTypes.Add(new QuizzesType { Id = 0, Name = "Shuffle Questions", BackgroundColor = "#28a745", IconImage = "done.png", IsEnables = true, IsVisibles = true, LblNote = "This Quiz can only be played with shuffle enabled" });
                            QuizzesTypes.Add(new QuizzesType { Id = 2, Name = "Test Mode", BackgroundColor = "#28a745", IconImage = "done.png", IsEnables = true, IsVisibles = true, LblNote = "This Quiz can only be played in Test Mode" });

                        }
                        else
                        {
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                // You're on a phone
                                if (IsASmallDevice())
                                    QuizzesTypesHeight = "210";
                                else
                                    QuizzesTypesHeight = "215";
                                
                            }
                            else
                            {
                                // You're on a tablet
                                QuizzesTypesHeight = "220";
                            }
                            
                            QuizzesTypes.Add(new QuizzesType { Id = 0, Name = "Shuffle Questions", BackgroundColor = "#28a745", IsVisibles = true, IconImage = "done.png", IsEnables = true, LblNote = "This Quiz can only be played with shuffle enabled" });
                            QuizzesTypes.Add(new QuizzesType { Id = 1, Name = "Remove Duplicates", BackgroundColor = "#868e96", IsVisibles = true, IconImage = "close.png", IsEnables = false });
                            QuizzesTypes.Add(new QuizzesType { Id = 2, Name = "Test Mode", BackgroundColor = "#28a745", IsVisibles = true, IconImage = "done.png", IsEnables = true, LblNote = "This Quiz can only be played in Test Mode" });
                        }
                    }
                    else
                    {
                        if (GlobalConst.isCourse)
                        {
                           
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                // You're on a phone
                                if (IsASmallDevice())
                                    QuizzesTypesHeight = "100";
                                else
                                    QuizzesTypesHeight = "105";
                                
                            }
                            else
                            {
                                // You're on a tablet
                                QuizzesTypesHeight = "120";
                            }
                            QuizzesTypes.Add(new QuizzesType { Id = 0, Name = "Shuffle Questions", BackgroundColor = "#28a745", IsVisibles = true, IconImage = "done.png", IsEnables = false });
                            QuizzesTypes.Add(new QuizzesType { Id = 1, Name = "Test Mode", BackgroundColor = "#868e96", IsVisibles = true, IconImage = "close.png", IsEnables = false });

                        }
                        else
                        {
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                // You're on a phone
                                if (IsASmallDevice())
                                    QuizzesTypesHeight = "150";
                                else
                                    QuizzesTypesHeight = "155";
                            }
                            else
                            {
                                // You're on a tablet
                                QuizzesTypesHeight = "170";
                            }
                          
                            QuizzesTypes.Add(new QuizzesType { Id = 0, Name = "Shuffle Questions", BackgroundColor = "#28a745", IsVisibles = true, IconImage = "done.png", IsEnables = false });
                            QuizzesTypes.Add(new QuizzesType { Id = 1, Name = "Remove Duplicates", BackgroundColor = "#868e96", IsVisibles = true, IconImage = "close.png", IsEnables = false });
                            QuizzesTypes.Add(new QuizzesType { Id = 2, Name = "Test Mode", BackgroundColor = "#868e96", IsVisibles = true, IconImage = "close.png", IsEnables = false });
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


        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

        }

        //ResumeQuizze click
        public DelegateCommand ResumeQuizze => new DelegateCommand(async () =>
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    bool qShuffle = false;
                    bool removeDuplicates = false;
                    bool testMode = false;

                    foreach (var item in QuizzesTypes)
                    {
                        if (item.IconImage.Equals("done.png"))
                        {
                            if (item.Name.Equals("Shuffle Questions"))
                                qShuffle = true;
                            if (item.Name.Equals("Remove Duplicates"))
                                removeDuplicates = true;
                            if (item.Name.Equals("Test Mode"))
                            {
                                testMode = true;
                                GlobalConst.istestMode = true;
                            }

                        }
                    }
                    if (assignmentData != null)
                    {
                        qShuffle = true;
                    }
                    var data = await _clientAPI.GetQuestionsOfQuizzes(QuizID, CategoryID, testMode, qShuffle, removeDuplicates, IsResume, GlobalConst.isCourse, CourseId, false, (HWID > 0) ? true : false, HWID);
                    if (data != null)
                    {
                        GlobalConst.Session = data.SessionID;
                        var parameters = new NavigationParameters();
                        parameters.Add("SessionInfo", SessionInfo);
                        parameters.Add("PlayTab", Quizzesdata);
                        parameters.Add("QuestionsList", data);
                        parameters.Add("Categories", Category);
                        parameters.Add("SourceURL", SourceURL);
                        parameters.Add("Coureses", Coureses);
                        parameters.Add("assignmentData", assignmentData);
                        parameters.Add("SubCategories", LvSelectedSubCategoryName);
                        await _navigationService.NavigateAsync(nameof(QuizzesQuestionPage), parameters);
                    }// Task<QuizzesQuestions> GetQuestionsOfQuizzes(int id, int categoryID, bool testMode, bool qShuffle, bool removeDuplicates, bool resume, bool isCourse, int courseID, bool retakeWrong, bool isHomework, int homeworkID);


                    //UserDialogs.Instance.HideLoading();
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
        });

        //Play quize button
        public DelegateCommand StartQuizze => new DelegateCommand(async () =>
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    bool qShuffle = false;
                    bool removeDuplicates = false;
                    bool testMode = false;

                    foreach (var item in QuizzesTypes)
                    {
                        if (item.IconImage.Equals("done.png"))
                        {
                            if (item.Name.Equals("Shuffle Questions"))
                                qShuffle = true;
                            if (item.Name.Equals("Remove Duplicates"))
                                removeDuplicates = true;
                            if (item.Name.Equals("Test Mode"))
                            {
                                //if(GlobalConst.isCourse)
                                //    testMode = false;
                                //else
                                    testMode = true;
                                GlobalConst.istestMode = true;
                            }

                        }
                    }
                    if (assignmentData != null)
                    {
                        qShuffle = true;
                    }
                    var data = await _clientAPI.GetQuestionsOfQuizzes(QuizID, CategoryID, testMode, qShuffle, removeDuplicates, false, GlobalConst.isCourse, CourseId, false, (HWID > 0) ? true : false, HWID);
                    if (data != null)
                    {
                        GlobalConst.Session = data.SessionID;
                        var parameters = new NavigationParameters();
                        parameters.Add("SessionInfo", SessionInfo);
                        parameters.Add("PlayTab", Quizzesdata);
                        parameters.Add("QuestionsList", data);
                        parameters.Add("Categories", Category);
                        parameters.Add("SourceURL", SourceURL);
                        parameters.Add("Coureses", Coureses);
                        parameters.Add("assignmentData", assignmentData);
                        parameters.Add("SubCategories", LvSelectedSubCategoryName);
                        //await _navigationService.GoBackAsync();

                        await _navigationService.NavigateAsync(nameof(QuizzesQuestionPage), parameters);
                    }// Task<QuizzesQuestions> GetQuestionsOfQuizzes(int id, int categoryID, bool testMode, bool qShuffle, bool removeDuplicates, bool resume, bool isCourse, int courseID, bool retakeWrong, bool isHomework, int homeworkID);
                    else
                    {
                        UserDialogs.Instance.HideLoading();
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
        });

        //Close Quizze click
        public DelegateCommand CloseQuizze => new DelegateCommand(async () =>
        {
            try
            {
                var parameters = new NavigationParameters();
                parameters.Add("Categories", Category);
                parameters.Add("Coureses", Coureses);
                parameters.Add("SubCategories", LvSelectedSubCategoryName);
                
                await _navigationService.GoBackAsync(parameters);
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
    }
}
