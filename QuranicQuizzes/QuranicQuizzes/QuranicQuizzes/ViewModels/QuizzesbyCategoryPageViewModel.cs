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
using Xamarin.Forms.Extended;

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// Viewmodel of QuizzesByCategoriesPage
    /// </summary>
    public class QuizzesbyCategoryPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public int pageCount = 1;
        public int searchPageCount = 1;
        public int TotalSearchPageCount;
        public int SubcategoryId = 0;
        public int TotalPageCount = 0;
        public Command<object> LearnTap { get; set; }
        public Command<object> PlayTap { get; set; }
        public Command<object> LiveGameTap { get; set; }
        public Command<object> AssignmentTap { get; set; }

        public QuizzesbyCategoryPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                _clientAPI = clientAPI;
                IsVisibleSubCategories = false;
                IsVisibleCategories = true;
               
                LearnTap = new Command<object>(LearnTapClick, (x) => CanNavigate);
                PlayTap = new Command<object>(PlayTapClick, (x) => CanNavigate);
                LiveGameTap = new Command<object>(LiveGameTapClick, (x) => CanNavigate);
                AssignmentTap = new Command<object>(AssignmentTapClick, (x) => CanNavigate);
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

        private async void AssignmentTapClick(object obj)
        {
            if (obj != null)
            {
                CanNavigate = false;
                var QuizzeData = obj as Quizze;
                if (QuizzeData != null)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add("QuizzeData", QuizzeData);
                    parameters.Add("SubCategories", LvSelectedSubCategoryName);
                    await _navigationService.NavigateAsync(nameof(CreateNewAssignmentPage), parameters);
                }
                CanNavigate = true;
            }
        }

        private async void LiveGameTapClick(object obj)
        {
            if (obj != null)
            {
                CanNavigate = false;
                var data = obj as Quizze;
                if (data != null)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add("qid", data.Id.ToString());
                    parameters.Add("SubCategories", LvSelectedSubCategoryName);
                    await _navigationService.NavigateAsync(nameof(StartGamePage), parameters);
                }
                CanNavigate = true;
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
                    GlobalConst.isLearnTab = false;
                    var Learndata = obj as Quizze;
                    if (Learndata.IsLearnTabFree)
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        if (Learndata != null)
                        {
                            var parameters = new NavigationParameters();
                            parameters.Add("PlayTab", Learndata);
                            parameters.Add("Categories", Category);
                            parameters.Add("SubCategories", LvSelectedSubCategoryName);
                            if (Learndata.IsLearnTabFree)
                            {
                                await _navigationService.NavigateAsync(nameof(MainLearnPage), parameters);
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
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
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
                    var data = obj as Quizze;


                    if (data != null)
                    {
                        if (!data.LockQuiz)
                        {
                            UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                            GlobalConst.isLearnTab = false;
                            //CheckSession SessionData = await _clientAPI.GetCheckSession(data.Id,GlobalConst.isCourse, 0);
                            //parameters.Add("SessionInfo", SessionData);
                            var parameters = new NavigationParameters();
                            parameters.Add("PlayTab", data);
                            parameters.Add("Categories", Category);
                            parameters.Add("SubCategories", LvSelectedSubCategoryName);
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

        //Api call of Search
        private async void SelectedItem()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (string.IsNullOrEmpty(SearchData))
                    {
                        IsSearch = false;
                        await Databinding();
                    }
                    else
                    {
                        IsSearch = true;
                        if (LvSelectedSubCategoryName != null)
                        {
                            SubcategoryId = LvSelectedSubCategoryName.ID;
                        }
                        Quizzes SearchRespo = new Quizzes();
                        SearchRespo = await _clientAPI.GetSearchData(Category.ID, SubcategoryId, SearchData, pageCount);
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
                                    Name = data.Name,
                                    Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                                    ImageURL = GlobalConst.ApiUrl + data.ImageURL,
                                    NumberPlayed = data.NumberPlayed,
                                    NumberQuestions = data.NumberQuestions,
                                    Created = data.Created,
                                    LockQuiz = data.LockQuiz,
                                    ShowIcon = !data.LockQuiz,
                                    ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                                    IsTeacher = UserSettings.IsTeacher,
                                    IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                                    CourseLockQuiz = data.CourseLockQuiz,
                                    CourseQuizPassed = data.CourseQuizPassed,
                                    CategoryID = data.CategoryID,
                                    ShowHint = data.ShowHint,
                                    LearnTabSound = string.IsNullOrEmpty(data.LearnTabSound) ? "" : GlobalConst.ApiUrl + data.LearnTabSound,
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

        public bool _isFilter;
        public bool IsFilter
        {
            get
            {
                return _isFilter;
            }

            set
            {
                SetProperty(ref _isFilter, value);

            }
        }


        private SubCategories _lvSelectedSubCategoryName { get; set; }
        public SubCategories LvSelectedSubCategoryName
        {
            get
            {
                return _lvSelectedSubCategoryName;

            }
            set
            {
                _lvSelectedSubCategoryName = value;
                RaisePropertyChanged(nameof(LvSelectedSubCategoryName));
               
            }
        }

        //Api call of Sub categories by quizzes
        private async void SelectedSubCategoriesItem()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    if (LvSelectedSubCategoryName.ID == 0)
                    {
                        IsFilter = false;
                        await Databinding();

                    }
                    else
                    {
                        IsFilter = true;
                        var category = await _clientAPI.GetSubCategoriesByQuizzes(LvSelectedSubCategoryName.ID, pageCount);

                        if (category != null)
                        {
                            TotalPageCount = category.totalPages;
                            Quizzescategory.Clear();

                            foreach (var data in category.quizzes)
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
                                    LearnTabSound = string.IsNullOrEmpty(data.LearnTabSound) ? "" : GlobalConst.ApiUrl + data.LearnTabSound,
                                    TestOnly = data.TestOnly
                                });
                            }
                        }
                    }
                    UserDialogs.Instance.HideLoading();
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
                UserDialogs.Instance.HideLoading();
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
        //public InfiniteScrollCollection<Quizze> Quizzescategory { get; set; } = new InfiniteScrollCollection<Quizze>();

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
        //Search box hide show
        public DelegateCommand iconSearchTap => new DelegateCommand(async () =>
        {
            IsVisibleSearch = !IsVisibleSearch;
        });

        //Clear serach data
        public DelegateCommand iconCloseTap => new DelegateCommand(async () =>
        {
            SearchData = "";
        });

        //Lazy loading Api call
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
                        category = await _clientAPI.GetSearchData(Category.ID, SubcategoryId, SearchData, pageCount);

                    }
                    else if (IsFilter)
                    {
                        category = await _clientAPI.GetSubCategoriesByQuizzes(LvSelectedSubCategoryName.ID, pageCount);
                    }
                    else
                    {
                        category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                    }
                    //var category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                    if (category != null)
                    {
                        foreach (var data in category.quizzes)
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
                                CourseQuizPassed = data.CourseQuizPassed,
                                CategoryID = data.CategoryID,
                                IsTeacher = UserSettings.IsTeacher,
                                ShowHint = data.ShowHint,
                                LearnTabSound = string.IsNullOrEmpty(data.LearnTabSound) ? "" : GlobalConst.ApiUrl + data.LearnTabSound,
                                TestOnly = data.TestOnly
                            });
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
                UserDialogs.Instance.HideLoading();
            }
        }

        public ObservableCollection<Quizze> Quizzescategory { get; set; } = new ObservableCollection<Quizze>();

        public int _selectedIndex { get; set; }
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                _selectedIndex = value;
                RaisePropertyChanged(nameof(SelectedIndex));
            }
        }

        //Data of Categories Selected by Home page categories
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                GlobalConst.isCourse = false;
                Category = parameters["Categories"] as Categories;
                LvSelectedSubCategoryName=parameters["SubCategories"] as SubCategories;
                if (Category != null)
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        await Databinding();
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

        //Api call of Quizzes by category
        public async Task Databinding()
        {
            try
            {
                //Quizzes category = new Quizze();
                //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                Quizzes category = new Quizzes();
                if (LvSelectedSubCategoryName != null && LvSelectedSubCategoryName.ID != 0)
                {
                    IsFilter = true;
                    SubcategoryId = LvSelectedSubCategoryName.ID;
                    category = await _clientAPI.GetSubCategoriesByQuizzes(LvSelectedSubCategoryName.ID, pageCount);
                }
                else
                {
                    category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                }

                TotalPageCount = category.totalPages;
                if (category != null)
                {
                    Quizzescategory.Clear();
                    
                    foreach (var data in category.quizzes)
                    {
                        Quizzescategory.Add(new Quizze
                        {
                            Id = data.Id,
                            Name = string.IsNullOrEmpty(data.Name)?"": data.Name,
                            Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                            ImageURL = string.IsNullOrEmpty(data.ImageURL)?"": GlobalConst.ApiUrl + data.ImageURL,
                            NumberPlayed = data.NumberPlayed,
                            NumberQuestions = data.NumberQuestions,
                            Created = data.Created,
                            LockQuiz = data.LockQuiz,
                            ShowIcon = !data.LockQuiz,
                            ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                            IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                            CourseLockQuiz = data.CourseLockQuiz,
                            CourseQuizPassed = data.CourseQuizPassed,
                            CategoryID = data.CategoryID,
                            ShowHint = data.ShowHint,
                            IsTeacher = UserSettings.IsTeacher,
                            LearnTabSound = string.IsNullOrEmpty(data.LearnTabSound) ? "" : GlobalConst.ApiUrl + data.LearnTabSound,
                            TestOnly = data.TestOnly
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
