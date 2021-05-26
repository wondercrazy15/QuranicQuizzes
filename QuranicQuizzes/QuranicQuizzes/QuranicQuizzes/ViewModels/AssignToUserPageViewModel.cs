using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    //Assign Assignment to user ViewModel
    public class AssignToUserPageViewModel:ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public int pageCount = 1;
        public int searchPageCount = 1;
        public int TotalSearchPageCount;
        public int TotalPageCount = 0;
        public Command<object> UserAssignTap { get; set; }

        public AssignToUserPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            UserAssignTap = new Command<object>(UserAssignTapClick);
        }

        //Assign Assignment to user
        private async void UserAssignTapClick(object obj)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (obj != null)
                    {
                        var data = obj as User;
                        if (!data.IsAssigned)
                        {
                            var res = await _clientAPI.AssignHomeworkToUser(CreateAssignment.ID, data.Id);
                            if (res.Equals(200))
                            {
                                User qt = new User();
                                qt.Id = data.Id;
                                qt.Name = data.Name;
                                qt.IconImage = "checked_user.png";
                                qt.BackgroundColor = "#28a745";
                                qt.IsAssigned = true;
                                var index = UserList.IndexOf(data);
                                UserList.RemoveAt(index);
                                UserList.Insert(index, qt);
                                var SelectedUsercount = UserList.Where(x => x.IsAssigned == true).Count();

                                if (SelectedUsercount == AssignedToUserList.Users.Count)
                                {
                                    IsAssignToAll = false;
                                    IsAssigned = true;
                                }
                                else
                                {
                                    IsAssignToAll = true;
                                    IsAssigned = false;
                                }
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

        private bool _isAssignToAll { get; set; }
        public bool IsAssignToAll
        {
            get => _isAssignToAll;
            set
            {
                _isAssignToAll = value;
                RaisePropertyChanged(nameof(IsAssignToAll));
            }
        }
        private bool _isAssigned { get; set; }
        public bool IsAssigned
        {
            get => _isAssigned;
            set
            {
                _isAssigned = value;
                RaisePropertyChanged(nameof(IsAssigned));
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
                    //SelectedItem();
                }
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


        //Assign Assignment to all user
        public DelegateCommand AssignToAll => new DelegateCommand(async () =>
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var res = await _clientAPI.AssignHomeworkToAllUser(CreateAssignment.ID);
                    if (res.Equals(200))
                    {
                        IsAssignToAll = false;
                        IsAssigned = true;
                        if (AssignedToUserList != null)
                        {
                            UserList.Clear();
                            foreach (var item in AssignedToUserList.Users)
                            {
                                UserList.Add(new User
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    IsAssigned = true,
                                    IconImage = "checked_user.png",
                                    BackgroundColor = "#28a745"
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
            
        });

        Quizze QuizzeData = new Quizze();
        CreateAssignment CreateAssignment = new CreateAssignment();
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                QuizzeData = parameters["QuizzeData"] as Quizze;
                CreateAssignment = parameters["CreateAssignment"] as CreateAssignment;
               
                if (QuizzeData != null)
                {
                    AssignmentTitle = QuizzeData.Name;

                }
                if(CreateAssignment!=null)
                {
                    AssignmentTitle = CreateAssignment.QuizName;
                }
                if (CrossConnectivity.Current.IsConnected)
                {
                    await getAssignToUserList();
                }
                else
                {
                    ShowMessage("Please check your internet connection");
                }
             
                UserDialogs.Instance.HideLoading();
            }
        }

        AssignedToUser AssignedToUserList = new AssignedToUser();
        public ObservableCollection<User> UserList { get; set; } = new ObservableCollection<User>();

        private async Task getAssignToUserList()
        {
            try
            {
                AssignedToUserList = await _clientAPI.GetAssignedToUserList(CreateAssignment.ID, pageCount);
                if (AssignedToUserList != null)
                {
                    foreach (var item in AssignedToUserList.Users)
                    {
                        UserList.Add(new User
                        {
                            Id=item.Id,
                            Name=item.Name,
                            IsAssigned=item.IsAssigned,
                            IconImage= "checked_user.png",
                            BackgroundColor= (item.IsAssigned) ? "#28a745":"#868e96"
                        });
                    }
                    var SelectedUsercount = UserList.Where(x =>x.IsAssigned==true).Count();

                    if (SelectedUsercount== AssignedToUserList.Users.Count)
                    {
                        IsAssignToAll = false;
                        IsAssigned = true;
                    }
                    else
                    {
                        IsAssignToAll = true;
                        IsAssigned = false;
                    }
                    //qt.IconImage = (data.IconImage).Equals("close.png") ? "done.png" : "close.png";
                    //qt.BackgroundColor = (data.BackgroundColor).Equals("#868e96") ? "#28a745" : "#868e96";
                    // UserList = new ObservableCollection<User>(data.Users);
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
        //Lazy loading Api call
        public async void LoadMoreData(bool search)
        {
            try
            {
                //IsBusy = true;
                //pageCount = pageCount + 1;
                //Quizzes category = new Quizzes();
                //if (IsSearch)
                //{
                //    category = await _clientAPI.GetSearchData(Category.ID, SubcategoryId, SearchData, pageCount);

                //}
                //else if (IsFilter)
                //{
                //    category = await _clientAPI.GetSubCategoriesByQuizzes(LvSelectedSubCategoryName.ID, pageCount);
                //}
                //else
                //{
                //    category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                //}
                ////var category = await _clientAPI.GetCategoriesByQuizzes(Category.ID, pageCount);
                //if (category != null)
                //{
                //    foreach (var data in category.quizzes)
                //    {
                //        Quizzescategory.Add(new Quizze
                //        {
                //            Id = data.Id,
                //            Name = data.Name,
                //            Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                //            ImageURL = GlobalConst.ApiUrl + data.ImageURL,
                //            NumberPlayed = data.NumberPlayed,
                //            NumberQuestions = data.NumberQuestions,
                //            Created = data.Created,
                //            LockQuiz = data.LockQuiz,
                //            ShowIcon = !data.LockQuiz,
                //            ShowLearnIcon = (data.LockQuiz) ? !data.IsLearnTabFree : false,
                //            IsLearnTabFree = (data.LockQuiz) ? data.IsLearnTabFree : true,
                //            CourseLockQuiz = data.CourseLockQuiz,
                //            CourseQuizPassed = data.CourseQuizPassed,
                //            CategoryID = data.CategoryID,
                //            IsTeacher = UserSettings.IsTeacher,
                //            ShowHint = data.ShowHint,
                //            TestOnly = data.TestOnly
                //        });
                //    }
                //}
                //IsBusy = false;
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
    }
}
