using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// Viewmodel of QuizzesByCoursesPage
    /// </summary>
    public class QuizzesByCoursesPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public int pageCount = 1;
        public int searchPageCount = 1;
        public int TotalSearchPageCount;
        public int SubcategoryId = 0;
        public int TotalPageCount;
        public Command<object> LearnTap { get; set; }
        public Command<object> PlayTap { get; set; }
        public Command<object> LiveGameTap { get; set; }

        public QuizzesByCoursesPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                _clientAPI = clientAPI;
                LearnTap = new Command<object>(LearnTapClick, (x) => CanNavigate);
                PlayTap = new Command<object>(PlayTapClick, (x) => CanNavigate);
                LiveGameTap = new Command<object>(LiveGameTapClick, (x) => CanNavigate);
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

        private async void LiveGameTapClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    CanNavigate = false;
                    var data = obj as Quizze;
                    if (data != null)
                    {
                        var parameters = new NavigationParameters();
                        parameters.Add("qid", data.Id.ToString());
                        await _navigationService.NavigateAsync(nameof(StartGamePage), parameters);
                    }
                    CanNavigate = true;
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

        //Learn Button click
        public async void LearnTapClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    CanNavigate = false;
                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    GlobalConst.isLearnTab = false;
                    var Learndata = obj as Quizze;
                    if (Learndata != null)
                    {
                        if (Learndata.IsLearnTabFree)
                        {
                            var parameters = new NavigationParameters();
                            parameters.Add("PlayTab", Learndata);
                            parameters.Add("Categories", Category);
                            parameters.Add("Coureses", Coureses);

                            if (Learndata.IsLearnTabFree)
                            {
                                await _navigationService.NavigateAsync(nameof(LearnTabPage), parameters);
                            }
                        }
                    }
                    else
                    {
                        await _navigationService.NavigateAsync(nameof(SubscribePage));
                    }
                    CanNavigate = true;
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
                UserDialogs.Instance.HideLoading();
            }
        }

        //Learn Button click
        public async void PlayTapClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    CanNavigate = false;
                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    var data = obj as Quizze;
                    if (data != null)
                    {
                        if (!data.LockQuiz)
                        {
                            GlobalConst.isLearnTab = false;
                            //CheckSession SessionData = await _clientAPI.GetCheckSession(data.Id,GlobalConst.isCourse, 0);
                            //parameters.Add("SessionInfo", SessionData);
                            var parameters = new NavigationParameters();
                            parameters.Add("PlayTab", data);
                            parameters.Add("Categories", Category);
                            parameters.Add("Coureses", Coureses);


                            if (data.IsLearnTabFree)
                            {
                                await _navigationService.NavigateAsync(nameof(PlayTabPage), parameters);
                            }
                        }
                        else
                        {
                            await _navigationService.NavigateAsync(nameof(SubscribePage));
                        }
                    }
                    CanNavigate = true;
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

        private string _searchData { get; set; }
        public string SearchData
        {
            get
            {
                return _searchData;
            }
            set
            {
                if (_searchData != value)
                {
                    _searchData = value;
                    RaisePropertyChanged(nameof(SearchData));
                    SelectedItem();
                }
            }
        }

        bool _canNavigate = true;
        public bool CanNavigate
        {
            get { return _canNavigate; }
            set
            {
                _canNavigate = value;
                RaisePropertyChanged(nameof(CanNavigate));
                // OnPropertyChanged();
            }
        }

        //Serach bar hide show
        public DelegateCommand iconSearchTap => new DelegateCommand(async () =>
        {
            IsVisibleSearch = !IsVisibleSearch;
        });

        //Clear search data
        public DelegateCommand iconCloseTap => new DelegateCommand(async () =>
        {
            SearchData = "";
        });
        //LockCourseTap
        public DelegateCommand LockCourseTap => new DelegateCommand(async () =>
        {
            DependencyService.Get<IToast>().Show("First quiz must be passed in order to unlock");

        });

        //Lazy loading,Load more pages Api call
        public async void LoadMoreData(bool search)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    IsBusy = true;
                    pageCount = pageCount + 1;
                    Quizzes category = new Quizzes();
                    if (IsSearch)
                    {
                        //category = await _clientAPI.GetSearchData(Category.ID, SubcategoryId, SearchData, pageCount);
                        //category = await _clientAPI.GetSearchData(Coureses.ID, 0, SearchData, pageCount);
                    }
                    else
                    {
                        //category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                        category = await _clientAPI.GetCoursesByQuizzes(Coureses.ID, pageCount);
                    }
                    //var category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                    if (category != null)
                    {
                        var quizPass = false;
                        for (int i = 0; i < category.quizzes.Count; i++)
                        {
                            var data = category.quizzes[i];


                            Quizzescategory.Add(new Quizze
                            {
                                Id = data.Id,
                                Name = string.IsNullOrEmpty(data.Name) ? "" : data.Name,
                                Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                                ImageURL = string.IsNullOrEmpty(data.ImageURL) ? "" : GlobalConst.ApiUrl + data.ImageURL,
                                NumberPlayed = data.NumberPlayed,
                                NumberQuestions = data.NumberQuestions,
                                Created = data.Created,
                                LockQuiz = data.LockQuiz,
                                ShowIcon = !data.LockQuiz,
                                ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                                IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                                ShowLockQuizPassed = !quizPass,
                                NotShowLockQuizPassed = quizPass,
                                IsTeacher = UserSettings.IsTeacher,
                                CourseLockQuiz = data.CourseLockQuiz,
                                CourseQuizPassed = data.CourseQuizPassed,
                                CategoryID = data.CategoryID,
                                ShowHint = data.ShowHint,
                                ShowLearnTab = data.ShowLearnTab,
                                ShowPlayTab = !data.ShowLearnTab,
                                TestOnly = data.TestOnly
                            });

                            quizPass = data.CourseQuizPassed;
                        }


                    }
                    IsBusy = false;
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

        //Search Api call
        private async void SelectedItem()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (string.IsNullOrEmpty(SearchData))
                    {
                        IsSearch = false;
                        await DatabindingCourses();
                    }
                    else
                    {
                        IsSearch = true;


                        Quizzes SearchRespo = new Quizzes();
                        SearchRespo = await _clientAPI.GetSearchData(Coureses.ID, 0, SearchData, pageCount);
                        //var SearchRespo = await _clientAPI.GetSearchData(Category.ID, SubcategoryId, SearchData, searchPageCount);
                        if (SearchRespo != null)
                        {
                            TotalPageCount = SearchRespo.totalPages;
                            Quizzescategory.Clear();
                            foreach (var data in SearchRespo.quizzes)
                            {
                                Quizzescategory.Add(new Quizze
                                {
                                    Id = data.Id,
                                    Name = string.IsNullOrEmpty(data.Name) ? "" : data.Name,
                                    Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                                    ImageURL = string.IsNullOrEmpty(data.ImageURL) ? "" : GlobalConst.ApiUrl + data.ImageURL,
                                    NumberPlayed = data.NumberPlayed,
                                    NumberQuestions = data.NumberQuestions,
                                    Created = data.Created,
                                    LockQuiz = data.LockQuiz,
                                    ShowIcon = !data.LockQuiz,
                                    ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                                    IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                                    CourseLockQuiz = data.CourseLockQuiz,
                                    IsTeacher = UserSettings.IsTeacher,
                                    CourseQuizPassed = data.CourseQuizPassed,
                                    CategoryID = data.CategoryID,
                                    ShowHint = data.ShowHint,
                                    ShowLearnTab = data.ShowLearnTab,
                                    ShowPlayTab = !data.ShowLearnTab,
                                    TestOnly = data.TestOnly
                                });
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
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        }

        private Coures _coures { get; set; }
        public Coures Coureses
        {
            get
            {
                return _coures;
            }
            set
            {
                _coures = value;
                RaisePropertyChanged(nameof(Coureses));
            }
        }

        //Data of Courses Selected by Home page course
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

                Coureses = parameters["Coureses"] as Coures;
                if (Coureses != null)
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        await DatabindingCourses();
                    }
                    else
                    {
                        ShowMessage("Please check your internet connection");
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

        public ObservableCollection<Quizze> Quizzescategory { get; set; } = new ObservableCollection<Quizze>();

        //Api call of quizzes by course
        public async Task DatabindingCourses()
        {
            try
            {
                //Quizzes category = new Quizze();
                Quizzes category = new Quizzes();

                //if (LvSelectedSubCategoryName != null && LvSelectedSubCategoryName.ID != 0)
                //{
                //    SubcategoryId = LvSelectedSubCategoryName.ID;
                //    category = await _clientAPI.GetSubCategoriesByQuizzes(LvSelectedSubCategoryName.ID, pageCount);
                //}
                //else
                //{
                //    category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                //}
                category = await _clientAPI.GetCoursesByQuizzes(Coureses.ID, pageCount);
                TotalPageCount = category.totalPages;
                if (category != null)
                {
                    Quizzescategory.Clear();
                    int pos = 0;
                    var quizPass = false;
                    for (int i = 0; i < category.quizzes.Count; i++)
                    {
                        var data = category.quizzes[i];

                        if (i == 0)
                        {
                            Quizzescategory.Add(new Quizze
                            {
                                Id = data.Id,
                                Name = string.IsNullOrEmpty(data.Name) ? "" : data.Name,
                                Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                                ImageURL = string.IsNullOrEmpty(data.ImageURL) ? "" : GlobalConst.ApiUrl + data.ImageURL,
                                NumberPlayed = data.NumberPlayed,
                                NumberQuestions = data.NumberQuestions,
                                Created = data.Created,
                                LockQuiz = data.LockQuiz,
                                ShowIcon = !data.LockQuiz,
                                ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                                IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                                ShowLockQuizPassed = false,
                                NotShowLockQuizPassed = true,
                                CourseLockQuiz = data.CourseLockQuiz,
                                IsTeacher = UserSettings.IsTeacher,
                                CourseQuizPassed = data.CourseQuizPassed,
                                CategoryID = data.CategoryID,
                                ShowHint = data.ShowHint,
                                ShowLearnTab = data.ShowLearnTab,
                                ShowPlayTab = !data.ShowLearnTab,
                                TestOnly = data.TestOnly
                            });
                        }
                        else
                        {
                            Quizzescategory.Add(new Quizze
                            {
                                Id = data.Id,
                                Name = string.IsNullOrEmpty(data.Name) ? "" : data.Name,
                                Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                                ImageURL = string.IsNullOrEmpty(data.ImageURL) ? "" : GlobalConst.ApiUrl + data.ImageURL,
                                NumberPlayed = data.NumberPlayed,
                                NumberQuestions = data.NumberQuestions,
                                Created = data.Created,
                                LockQuiz = data.LockQuiz,
                                ShowIcon = !data.LockQuiz,
                                ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                                IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                                ShowLockQuizPassed = !quizPass,
                                NotShowLockQuizPassed = quizPass,
                                CourseLockQuiz = data.CourseLockQuiz,
                                IsTeacher = UserSettings.IsTeacher,
                                CourseQuizPassed = data.CourseQuizPassed,
                                CategoryID = data.CategoryID,
                                ShowHint = data.ShowHint,
                                ShowLearnTab = data.ShowLearnTab,
                                ShowPlayTab = !data.ShowLearnTab,
                                TestOnly = data.TestOnly
                            });
                        }
                        quizPass = data.CourseQuizPassed;
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

        public bool _isVisibleSubCategories;
        public bool IsVisibleSubCategories
        {
            get
            {
                return _isVisibleSubCategories;
            }

            set
            {
                SetProperty(ref _isVisibleSubCategories, value);

            }
        }

        public bool _isVisibleSearch;
        public bool IsVisibleSearch
        {
            get
            {
                return _isVisibleSearch;
            }

            set
            {
                _isVisibleSearch = value;
                RaisePropertyChanged(nameof(IsVisibleSearch));

            }
        }


        public bool _isVisibleCategories;
        public bool IsVisibleCategories
        {
            get
            {
                return _isVisibleCategories;
            }

            set
            {
                SetProperty(ref _isVisibleCategories, value);

            }
        }

        public bool _isSearch;
        public bool IsSearch
        {
            get
            {
                return _isSearch;
            }

            set
            {
                SetProperty(ref _isSearch, value);

            }
        }

        private ObservableCollection<SubCategories> _lvSubCategories { get; set; } = new ObservableCollection<SubCategories>();
        public ObservableCollection<SubCategories> LvSubCategories
        {
            get
            {
                return _lvSubCategories;

            }
            set
            {
                _lvSubCategories = value;
                RaisePropertyChanged(nameof(LvSubCategories));
                //SelectedItem();
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

    }
}