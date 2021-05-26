using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using Plugin.LatestVersion;
using Plugin.StoreReview;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace QuranicQuizzes.ViewModels
{
    /// <summary>
    /// Homepage viewmodel
    /// </summary>
    public class HomePageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public Command<object> CategoryClickCommand { get; set; }
        public Command<object> CourseListCommand { get; set; }

        public HomePageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            CategoryClickCommand = new Command<object>(CategoryView, (x) => CanNavigate);
            CourseListCommand = new Command<object>(CourceView, (x) => CanNavigate);
            IsVisibleCourse = false;
            IsVisibleCategory = true;
            CategoryBGcolor = Color.FromHex("#f37737");
            CourseBGcolor = Color.FromHex("#C1C1C1");
            BackgroundImg = "categorybg.JPG";

            if (CrossConnectivity.Current.IsConnected)
            {

                if (UserSettings.IsLogin)
                {
                    CheckVersion();
                    //Exception e = new Exception("Test Home");
                    //var properties = new Dictionary<string, string>
                    //    {
                    //        { "Messge", "Not coonect with play store" },
                    //        { "StackTrace",UserSettings.Name}
                    //    };
                    //Crashes.TrackError(e, properties);
                    categoriesApiCall();
                    UserInfoApiCall();
                }
            }
            else
            {
                ShowMessage("Please check your internet connection");
            }

            //CheckPayment().Wait(5);

            //var ans = WasItemPurchased();
        }

        private async void CheckVersion()
        {
            try
            {

                string latestVersionNumber = await CrossLatestVersion.Current.GetLatestVersionNumber();
                var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();


                if (!isLatest)
                {
                    var update= await UserDialogs.Instance.ConfirmAsync("There is a new version of this app available. Would you like to update now?", "New Version",  "Yes", "No");

                    //MaterialAlertDialogConfiguration materialAlertDialogConfiguration = new MaterialAlertDialogConfiguration()
                    //{
                        
                    //    BackgroundColor = Color.Gray,
                    //    TintColor = Color.White,
                    //    MessageTextColor = Color.Black
                    //};
                    //var update=await MaterialDialog.Instance.ConfirmAsync(message: "There is a new version of this app available. Would you like to update now?",
                    //                confirmingText: "Yes",
                    //                dismissiveText: "No", configuration: materialAlertDialogConfiguration);

                    if (update==true)
                    {
                        switch (Device.RuntimePlatform)
                        {
                            case Device.iOS:
                                await CrossLatestVersion.Current.OpenAppInStore();
                                break;
                            case Device.Android:
                                await Browser.OpenAsync("https://play.google.com/store/apps/details?id=com.QuranicQuizzes", new BrowserLaunchOptions
                                {
                                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                                    TitleMode = BrowserTitleMode.Show,
                                    PreferredToolbarColor = Color.FromHex("#353A40"),
                                    PreferredControlColor = Color.White,
                                });
                                // Device.OpenUri(new Uri("market://details?id=com.QuranicQuizzes""));
                                break;
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

        //User Unsubscribe or not
        public async Task<bool> WasItemPurchased()
        {
            if (UserSettings.IsPaidMember)
            {
                var billing = CrossInAppBilling.Current;
                try
                {
                    Analytics.TrackEvent("Select Purchase called", new Dictionary<string, string>
                        {
                             { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                        });

                    var alertDialogConfiguration = new MaterialAlertDialogConfiguration
                    {
                        MessageTextColor = Color.Black,
                        MessageFontFamily = XF.Material.Forms.Material.GetResource<OnPlatform<string>>("QuranFont"),
                        TintColor = Color.Black
                    };
                    var connected = await billing.ConnectAsync(ItemType.InAppPurchase);
                    string[] ProductLists = new string[]{"com.droid.quran.subscription.memberpack.1", "com.droid.quran.subscription.memberpack.2","com.droid.quran.subscription.familypack.1","com.droid.quran.subscription.familypack.2","com.droid.quran.subscription.familypack.3","com.droid.quran.subscription.familypack.4","com.droid.quran.subscription.familypack.5","com.ios.quran.subscription.memberpack.1",
                    "com.ios.quran.subscription.memberpack.2","com.ios.quran.subscription.familypack.1","com.ios.quran.subscription.familypack.2","com.ios.quran.subscription.familypack.3","com.ios.quran.subscription.familypack.4","com.ios.quran.subscription.familypack.5"};
                    if (!connected)
                    {
                        //Couldn't connect
                        Exception e = new Exception("User Login");
                        var properties = new Dictionary<string, string>
                        {
                            { "Messge", "Not coonect with play store" },
                            { "StackTrace",UserSettings.Name}
                        };
                        Crashes.TrackError(null, properties);
                        return false;
                    }


                    //check purchases
                    var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);

                    //check for null just incase
                    bool NotPurchase = true;
                    for (int i = 0; i < ProductLists.Length; i++)
                    {
                        if (purchases?.Any(p => p.ProductId == ProductLists[i]) ?? false)
                        {
                            //Purchase restored
                            NotPurchase = false;
                            break;
                        }

                    }
                   
                    if (NotPurchase)
                    {
                        Analytics.TrackEvent("Purchase cancle Call", new Dictionary<string, string>
                        {
                             { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                        });
                        var res = await _clientAPI.PurchaseItemCancle(UserData.PurchaseToken);
                    }
                    else
                    {
                        Analytics.TrackEvent("Purchase cancle else Call", new Dictionary<string, string>
                        {
                             { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                        });
                    }

                }
                catch (InAppBillingPurchaseException purchaseEx)
                {
                    Analytics.TrackEvent("Purchase cancle error", new Dictionary<string, string>
                        {
                             { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                        });

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
                finally
                {
                    await billing.DisconnectAsync();
                }
            }
            return false;
        }

        //Logout
        public async void removeData() {
            try
            {
                UserSettings.IsLogin = false;
                UserSettings.Logo = string.Empty;
                UserSettings.Name = string.Empty;
                UserSettings.IsStudent = false;
                UserSettings.IsPaidMember = false;
                UserSettings.IsInstituteAdmin = false;
                UserSettings.IsFamilyAdmin = false;
                UserSettings.IsTeacher = false;
                UserSettings.IsEnrolled = false;
                UserSettings.AccesToken = string.Empty;
                DependencyService.Get<IRegisterToken>().RemoveRegisterToken();

                await _navigationService.NavigateAsync(nameof(Signup_Login_HomePage));
                ShowMessage("Please login again..");
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

        //Check user token is expire and check user is paid or not
        UserInfo UserData = new UserInfo();
        private async void UserInfoApiCall()
        {
            try
            {
                IsBusy = true;
                UserData = await _clientAPI.GetUserInfo();
                if (UserData != null)
                {
                    UserSettings.IsPurchased = string.IsNullOrEmpty(UserData.PurchaseToken) ? true : false;
                    if (!UserSettings.IsPaidMember == UserData.IsPaidMember)
                    {
                        ShowMessage("Please login again..");
                        removeData();
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        string platform = "";
                        switch (Device.RuntimePlatform)
                        {
                            case Device.iOS:
                                platform = "apple";
                                break;
                            case Device.Android:
                                platform = "google";
                                break;
                        }
                        bool ans;

                        if ((UserData.PaymentGateway != null) ? UserData.PaymentGateway.Equals(platform) : false)
                            ans = await WasItemPurchased();
                    }

                }
                else
                {
                    ShowMessage("Please login again..");
                    removeData();
                }
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        }

        //Click of Course item 
        private async void CourceView(object obj)
        {
            try
            {
                if (obj != null)
                {
                    CanNavigate = false;
                    var LvSelectedItems = obj as Coures;
                    var parameters = new NavigationParameters();
                    parameters.Add("Coureses", LvSelectedItems);
                    GlobalConst.isCourse = true;
                    await _navigationService.NavigateAsync(nameof(QuizzesByCoursesPage), parameters);
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


        //Click of Categories item
        private async void CategoryView(object obj)
        {
            try
            {
                if (obj != null)
                {
                    CanNavigate = false;
                    LvSelectedItem = obj as Categories;
                    var parameters = new NavigationParameters();
                    parameters.Add("Categories", LvSelectedItem);
                    GlobalConst.isCourse = false;
                    if (LvSelectedItem.SubCategories.Count > 0)
                        await _navigationService.NavigateAsync(nameof(QuizzesBySubCategoriesPage), parameters);
                    else
                        await _navigationService.NavigateAsync(nameof(QuizzesByCategoriesPage), parameters);
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

        //private async void CourceView(object obj)
        //{
        //    try
        //    {
        //        var LvSelectedItems = obj as Course;
        //        var parameters = new NavigationParameters();
        //        parameters.Add("Categories", LvSelectedItem);
        //        await _navigationService.NavigateAsync(nameof(QuizzesByCategoriesPage), parameters);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private Categories _lvSelectedItem { get; set; }
        public Categories LvSelectedItem
        {
            get
            {
                return _lvSelectedItem;

            }
            set
            {
                _lvSelectedItem = value;
                RaisePropertyChanged(nameof(LvSelectedItem));
                //SelectedItem();
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

        private bool _isVisibleCategory { get; set; }
        public bool IsVisibleCategory
        {
            get
            {
                return _isVisibleCategory;

            }
            set
            {
                _isVisibleCategory = value;
                RaisePropertyChanged(nameof(IsVisibleCategory));
                //SelectedItem();
            }
        }


        private Color _categoryBGcolor { get; set; }
        public Color CategoryBGcolor
        {
            get
            {
                return _categoryBGcolor;

            }
            set
            {
                _categoryBGcolor = value;
                RaisePropertyChanged(nameof(CategoryBGcolor));
                //SelectedItem();
            }
        }

        private Color _courseBGcolor { get; set; }
        public Color CourseBGcolor
        {
            get
            {
                return _courseBGcolor;

            }
            set
            {
                _courseBGcolor = value;
                RaisePropertyChanged(nameof(CourseBGcolor));
                //SelectedItem();
            }
        }

        private bool _isVisibleCourse { get; set; }
        public bool IsVisibleCourse
        {
            get
            {
                return _isVisibleCourse;

            }
            set
            {
                _isVisibleCourse = value;
                RaisePropertyChanged(nameof(IsVisibleCourse));
                //SelectedItem();
            }
        }

        private string _backgroundImg { get; set; }
        public string BackgroundImg
        {
            get
            {
                return _backgroundImg;

            }
            set
            {
                _backgroundImg = value;
                RaisePropertyChanged(nameof(BackgroundImg));
                //SelectedItem();
            }
        }

        //Api calling for category
        private async Task CategoryApiCall()
        {
            try
            {
                category.Clear();
                var categories = await _clientAPI.GetCategories();

                if (categories != null)
                {
                    foreach (var data in categories)
                    {
                        category.Add(new Categories
                        {
                            ID = data.ID,
                            CategoryName = string.IsNullOrEmpty(data.CategoryName) ? "" : data.CategoryName,
                            Description = data.Description.Equals("null") ? "" : data.Description,
                            ImageURL = GlobalConst.ApiUrl + data.ImageURL,
                            SubCategories = data.SubCategories
                        });

                    }
                }
                //category = new ObservableCollection<Categories>(categories);
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

        public ObservableCollection<Coures> Courses { get; set; } = new ObservableCollection<Coures>();

        //Api calling for Courses
        private async Task CoursesApiCall()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                Courses.Clear();
                var coursesdata = await _clientAPI.GetCourses(1);
                if (coursesdata != null)
                {
                    foreach (var data in coursesdata)
                    {

                        Courses.Add(new Coures
                        {
                            ID = data.ID,
                            Name = data.Name,
                            Description = string.IsNullOrEmpty(data.Description) ? "" : data.Description,
                            ImageURL = GlobalConst.ApiUrl + data.ImageURL
                        });

                    }
                }
                //UserDialogs.Instance.HideLoading();
                //category = new ObservableCollection<Categories>(categories);
            }
            catch (Exception ex)
            {
                //UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        }

        public ObservableCollection<Categories> category { get; set; } = new ObservableCollection<Categories>();

        //Click of Category button
        public DelegateCommand CategoryTap => new DelegateCommand(async () =>
        {
            if (!IsVisibleCategory)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CategoryBGcolor = Color.FromHex("#f37737");
                    CourseBGcolor = Color.FromHex("#C1C1C1");
                    IsVisibleCategory = true;
                    IsVisibleCourse = false;
                    IsBusy = true;
                    BackgroundImg = "categorybg.JPG";
                    await CategoryApiCall();
                    IsBusy = false;
                }
                else
                {
                    ShowMessage("Please check your internet connection");
                }

            }
        });

        public bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        //Click of Course button
        public DelegateCommand CourseTap => new DelegateCommand(async () =>
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (!IsVisibleCourse)
                {
                    CourseBGcolor = Color.FromHex("#f37737");
                    CategoryBGcolor = Color.FromHex("#C1C1C1");
                    IsVisibleCategory = false;
                    IsVisibleCourse = true;
                    IsBusy = true;
                    BackgroundImg = "coursebg.png";
                    await CoursesApiCall();
                    IsBusy = false;
                }
            }
            else
            {
                ShowMessage("Please check your internet connection");
            }


        });

        //Api calling of categories
        public async void categoriesApiCall()
        {
            try
            {
                
                if (CrossConnectivity.Current.IsConnected)
                {
                    IsBusy = true;
                    //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                    await CategoryApiCall();
                    IsBusy = false;
                    //UserDialogs.Instance.HideLoading();
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

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                //UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                //await CategoryApiCall();
                //UserDialogs.Instance.HideLoading();
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


    }
}
