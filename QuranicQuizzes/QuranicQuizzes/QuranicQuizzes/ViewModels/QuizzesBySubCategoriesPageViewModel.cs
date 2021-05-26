using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    public class QuizzesBySubCategoriesPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public int pageCount = 1;
        public int searchPageCount = 1;
        public int TotalSearchPageCount;
        public int SubcategoryId = 0;
        public int TotalPageCount = 0;
        public Command<object> SubcategoriesTap { get; set; }


        public QuizzesBySubCategoriesPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                _clientAPI = clientAPI;
                IsVisibleSubCategories = false;
                IsVisibleCategories = true;

                SubcategoriesTap = new Command<object>(SubcategoriesTapClick, (x) => CanNavigate);
            
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

        //Subcategories Selected  Button click
        public async void SubcategoriesTapClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    CanNavigate = false;
                    var SubCategories = obj as SubCategories;
                    if (SubCategories != null)
                    {
                        //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

                        var parameters = new NavigationParameters();

                        parameters.Add("Categories", Category);
                        parameters.Add("SubCategories", SubCategories);
                        await _navigationService.NavigateAsync(nameof(QuizzesByCategoriesPage), parameters);
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
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                GlobalConst.isCourse = false;
                Category = parameters["Categories"] as Categories;
                if (Category != null)
                {
                    if (Category.SubCategories.Count > 0)
                    {
                        IsVisibleSubCategories = true;
                        IsVisibleCategories = false;
                        var subCategories = new SubCategories();
                        subCategories.ID = 0;
                        subCategories.SubCategoryName = "Show All";
                        subCategories.ImageURL = Category.ImageURL;
                        LvSubCategories.Add(subCategories);
                        foreach (var item in Category.SubCategories)
                        {
                            LvSubCategories.Add(new SubCategories {
                                ID=item.ID,
                                SubCategoryName=item.SubCategoryName,
                                ImageURL=(string.IsNullOrEmpty(item.ImageURL))? Category.ImageURL: GlobalConst.ApiUrl+item.ImageURL
                            });
                        }
                        //LvSubCategories = Category.SubCategories;
                        //var selectedProjectName = 0;
                        //SelectedIndex = selectedProjectName;

                    }
                   
                    // await Databinding();
                    //Quizzescategory = new ObservableCollection<Quizze>(category.quizzes);
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
