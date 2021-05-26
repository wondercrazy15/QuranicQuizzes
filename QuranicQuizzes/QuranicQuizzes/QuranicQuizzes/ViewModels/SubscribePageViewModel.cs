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
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.Xaml;
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
    //Subscribe PageViewModel
    public class SubscribePageViewModel : ViewModelBase
    {

        INavigationService _navigationService;
        IClientAPI _clientAPI;

        public SubscribePageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SubscribeAmount.Add(new SubscribeAmount { ID = 1, IsMember = true, Name = "Pay Monthly : £1.99 GBP - monthly", DisplayName = "£1.99", Android_pid = "com.droid.quran.subscription.memberpack.1", iOS_pid = "com.ios.quran.subscription.memberpack.1" });
                    SubscribeAmount.Add(new SubscribeAmount { ID = 2, IsMember = true, Name = "Pay Yearly : £23.99 GBP - yearly", DisplayName = "£23.99", Android_pid = "com.droid.quran.subscription.memberpack.2", iOS_pid = "com.ios.quran.subscription.memberpack.2" });

                    break;
                case Device.Android:
                    SubscribeAmount.Add(new SubscribeAmount { ID = 1, IsMember = true, Name = "Pay Monthly : £2.00 GBP - monthly", DisplayName = "£2", Android_pid = "com.droid.quran.subscription.memberpack.1", iOS_pid = "com.ios.quran.subscription.memberpack.1" });
                    SubscribeAmount.Add(new SubscribeAmount { ID = 2, IsMember = true, Name = "Pay Yearly : £24.00 GBP - yearly", DisplayName = "£24", Android_pid = "com.droid.quran.subscription.memberpack.2", iOS_pid = "com.ios.quran.subscription.memberpack.2" });
                    break;
            }

            SubscribeAmount obj = SubscribeAmount.Where(s => s.ID.Equals(1)).FirstOrDefault();
            LvIndividualAmount = obj;

            SubscribeFamilyAmount.Add(new SubscribeAmount { ID = 1, IsMember = false, Name = "1 : £2.99 GBP - monthly", DisplayName = "£2.99", Android_pid = "com.droid.quran.subscription.familypack.1", iOS_pid = "com.ios.quran.subscription.familypack.1" });
            SubscribeFamilyAmount.Add(new SubscribeAmount { ID = 2, IsMember = false, Name = "2 : £3.99 GBP - monthly", DisplayName = "£3.99", Android_pid = "com.droid.quran.subscription.familypack.2", iOS_pid = "com.ios.quran.subscription.familypack.2" });
            SubscribeFamilyAmount.Add(new SubscribeAmount { ID = 3, IsMember = false, Name = "3 : £5.99 GBP - monthly", DisplayName = "£5.99", Android_pid = "com.droid.quran.subscription.familypack.3", iOS_pid = "com.ios.quran.subscription.familypack.3" });
            SubscribeFamilyAmount.Add(new SubscribeAmount { ID = 5, IsMember = false, Name = "5 : £7.99 GBP - monthly", DisplayName = "£7.99", Android_pid = "com.droid.quran.subscription.familypack.4", iOS_pid = "com.ios.quran.subscription.familypack.4" });
            SubscribeFamilyAmount.Add(new SubscribeAmount { ID = 10, IsMember = false, Name = "10 : £15.99 GBP - monthly", DisplayName = "£15.99", Android_pid = "com.droid.quran.subscription.familypack.5", iOS_pid = "com.ios.quran.subscription.familypack.5" });

            SubscribeAmount obj1 = SubscribeFamilyAmount.Where(s => s.ID.Equals(1)).FirstOrDefault();
            LvscribeFamilyAmount = obj1;
        }

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

        public int _selectedIndex1 { get; set; }
        public int SelectedIndex1
        {
            get
            {
                return _selectedIndex1;
            }

            set
            {
                _selectedIndex1 = value;
                RaisePropertyChanged(nameof(SelectedIndex1));
            }
        }

        public string _IndividualAmountLblName { get; set; }
        public string IndividualAmountLblName
        {
            get
            {
                return _IndividualAmountLblName;
            }

            set
            {
                _IndividualAmountLblName = value;
                RaisePropertyChanged(nameof(IndividualAmountLblName));
            }
        }

        //Terms and condition Click 
        public DelegateCommand TermsClickCommand => new DelegateCommand(async () =>
        {
            await Browser.OpenAsync("https://quranicquizzes.com/Legal/Terms", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,

            });
        });


        //Provacy Policy Click 
        public DelegateCommand ProvacyPolicyClickCommand => new DelegateCommand(async () =>
        {
            await Browser.OpenAsync("https://quranicquizzes.com/Legal/Privacy", new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Color.FromHex("#353A40"),
                PreferredControlColor = Color.White,

            });
        });

        public int _NofStudent { get; set; }
        public int NofStudent
        {
            get
            {
                return _NofStudent;
            }

            set
            {
                _NofStudent = value;
                RaisePropertyChanged(nameof(NofStudent));
            }
        }

        public string _IndividualAmountMonthYear { get; set; }
        public string IndividualAmountMonthYear
        {
            get
            {
                return _IndividualAmountMonthYear;
            }
            set
            {
                _IndividualAmountMonthYear = value;
                RaisePropertyChanged(nameof(IndividualAmountMonthYear));
            }
        }
        public string _FamilyAmountLblName { get; set; }
        public string FamilyAmountLblName
        {
            get
            {
                return _FamilyAmountLblName;
            }
            set
            {
                _FamilyAmountLblName = value;
                RaisePropertyChanged(nameof(FamilyAmountLblName));
            }
        }

        public SubscribeAmount _LvscribeFamilyAmount { get; set; }
        public SubscribeAmount LvscribeFamilyAmount
        {
            get
            {
                return _LvscribeFamilyAmount;
            }

            set
            {
                _LvscribeFamilyAmount = value;
                IFamilyAmountLbl();
                RaisePropertyChanged(nameof(LvscribeFamilyAmount));
            }
        }

        private void IFamilyAmountLbl()
        {
            FamilyAmountLblName = LvscribeFamilyAmount.DisplayName;
            NofStudent = LvscribeFamilyAmount.ID;
        }

        public SubscribeAmount _LvIndividualAmount { get; set; }
        public SubscribeAmount LvIndividualAmount
        {
            get
            {
                return _LvIndividualAmount;
            }

            set
            {
                _LvIndividualAmount = value;
                IndividualAmountLbl();
                RaisePropertyChanged(nameof(LvIndividualAmount));
            }
        }

        private void IndividualAmountLbl()
        {
            IndividualAmountLblName = LvIndividualAmount.DisplayName;
            IndividualAmountMonthYear = LvIndividualAmount.Name.Contains("Monthly") ? "/mo" : "/yr";
        }

        private ObservableCollection<SubscribeAmount> _SubscribeAmount { get; set; } = new ObservableCollection<SubscribeAmount>();
        public ObservableCollection<SubscribeAmount> SubscribeAmount
        {
            get
            {
                return _SubscribeAmount;

            }
            set
            {
                _SubscribeAmount = value;
                RaisePropertyChanged(nameof(SubscribeAmount));
                //SelectedItem();
            }
        }
        private ObservableCollection<SubscribeAmount> _SubscribeFamilyAmount { get; set; } = new ObservableCollection<SubscribeAmount>();
        public ObservableCollection<SubscribeAmount> SubscribeFamilyAmount
        {
            get
            {
                return _SubscribeFamilyAmount;

            }
            set
            {
                _SubscribeFamilyAmount = value;
                RaisePropertyChanged(nameof(SubscribeFamilyAmount));
                //SelectedItem();
            }
        }

        public DelegateCommand ContactusClickCommand => new DelegateCommand(async () =>
        {
            await Launcher.OpenAsync(new Uri("mailto:info@quranicquizzes.com?subject=Institution Account Enquiry:" + UserSettings.UserId));
        });

        //For IndividualAccount
        public DelegateCommand IndividualAccountCommand => new DelegateCommand(async () =>
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        await PurchaseItemAsync(LvIndividualAmount, LvIndividualAmount.iOS_pid);
                        break;
                    case Device.Android:
                        await PurchaseItemAsync(LvIndividualAmount, LvIndividualAmount.Android_pid);
                        break;
                }

            }
            else
            {
                ShowMessage("Please check your internet connection");
            }

        });

        //For FamilyAccount
        public DelegateCommand FamilyAccountCommand => new DelegateCommand(async () =>
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        await PurchaseItemAsync(LvscribeFamilyAmount, LvscribeFamilyAmount.iOS_pid);
                        break;
                    case Device.Android:
                        await PurchaseItemAsync(LvscribeFamilyAmount, LvscribeFamilyAmount.Android_pid);
                        break;
                }
            }
            else
            {
                ShowMessage("Please check your internet connection");
            }
        });

        //Purchase Item API call
        InAppBillingPurchase purchase;
        string platform = "";
        private async System.Threading.Tasks.Task PurchaseItemAsync(SubscribeAmount lvscribe, string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                var connected = await billing.ConnectAsync(ItemType.Subscription);
                if (!connected)
                {

                    Analytics.TrackEvent("we are offline or can't connect, don't try to purchase ", new Dictionary<string, string>
                    {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    });
                    //we are offline or can't connect, don't try to purchase
                    return;
                }

                //var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);

                //check for null just incase
                //if (purchases?.Any() ?? false)
                //{
                //    var ownedPurchase = purchases.Where(x => x.ProductId == productId).FirstOrDefault();
                //    if (ownedPurchase != null)
                //    {
                //        var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(productId, ownedPurchase.PurchaseToken);
                //    }
                //}
                //check purchases
                Analytics.TrackEvent("In app purchase Clicked", new Dictionary<string, string>
                {
                    { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                });

                purchase = await billing.PurchaseAsync(productId, ItemType.Subscription, "apppayload");
                
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        platform = "apple";
                        break;
                    case Device.Android:
                        platform = "google";
                        break;
                }
                //possibility that a null came through.
                if (purchase == null)
                {
                    //did not purchase
                    Analytics.TrackEvent("Did not purchase", new Dictionary<string, string>
                    {
                         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    });
                    DependencyService.Get<IToast>().Show("Did not purchase, please try again.");
                }
                //else if (purchase.State == PurchaseState.Purchased || platform == "google")
                else
                {
                    //purchased, we can now consume the item or do it later
                    //If we are on iOS we are done, else try to consume the purchase
                    //Device.RuntimePlatform comes from Xamarin.Forms, you can also use a conditional flag or the DeviceInfo plugin

                    //var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(purchase.ProductId, purchase.PurchaseToken);

                    //if (consumedItem != null)
                    //{
                    Analytics.TrackEvent("In app purchase Enter Sucessfully", new Dictionary<string, string>
                        {
                             { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                        });

                   // if(platform = "apple")
                    //var purchaseItem = await billing.GetPurchasesAsync(ItemType.InAppPurchase);

                    //check for null just incase
                    
                    //if (purchaseItem?.Any(p => p.ProductId == productId) ?? false)
                    //{
                        Analytics.TrackEvent("Purchase Item found", new Dictionary<string, string>
                        {
                             { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                        });
                    //Consumed!!
                    //Toast.MakeText(this, _thisViewModel.Label_Successfully_Purchase, ToastLength.Long).Show();
                       PurchaseCheck(lvscribe, productId);

                    //}

                    //else
                    //{
                    //      Analytics.TrackEvent("Purchase Item Not found", new Dictionary<string, string>
                    //    {
                    //         { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    //    });
                    //}

                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                var message = string.Empty;
                
                
                var properties = new Dictionary<string, string>
                {
                    { "Messge", purchaseEx.Message },
                    { "StackTrace", purchaseEx.StackTrace }
                };
                Analytics.TrackEvent("In app purchase Error", new Dictionary<string, string>
                {
                       { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                });

                //Crashes.TrackError(purchaseEx, properties);

                //var connected = await billing.ConnectAsync(ItemType.InAppPurchase);
                //string[] ProductLists = new string[]{"com.droid.quran.subscription.memberpack.1", "com.droid.quran.subscription.memberpack.2","com.droid.quran.subscription.familypack.1","com.droid.quran.subscription.familypack.2","com.droid.quran.subscription.familypack.3","com.droid.quran.subscription.familypack.4","com.droid.quran.subscription.familypack.5","com.ios.quran.subscription.memberpack.1",
                //    "com.ios.quran.subscription.memberpack.2","com.ios.quran.subscription.familypack.1","com.ios.quran.subscription.familypack.2","com.ios.quran.subscription.familypack.3","com.ios.quran.subscription.familypack.4","com.ios.quran.subscription.familypack.5"};


                ////check purchases
                //var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);
               
                ////check for null just incase
                //bool Purchase = false;
                //for (int i = 0; i < ProductLists.Length; i++)
                //{
                //    if (purchases?.Any(p => p.ProductId == ProductLists[i]) ?? false)
                //    {
                //        //Purchase restored
                //        Purchase = true;
                //        break;
                //    }
                //}

                //if (Purchase)
                //{
                //    //if(UserSettings.IsPurchased)
                //       PurchaseCheck(lvscribe, productId);
                //}

                switch (purchaseEx.PurchaseError)
                {                    
                    case PurchaseError.GeneralError:
                        ShowMessage("Cannot connect to iTunes Store");
                        break;
                    case PurchaseError.AppStoreUnavailable:
                        ShowMessage("Please check your Internet Connection");
                        break;
                    case PurchaseError.BillingUnavailable:
                        ShowMessage("Currently the app store seems to be unavailble. Try again later.");
                        break;
                    case PurchaseError.PaymentInvalid:
                        ShowMessage("Payment seems to be invalid, please try again.");
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        ShowMessage("Payment does not seem to be enabled/allowed, please try again.");
                        break;
                }
                UserDialogs.Instance.HideLoading();
                //if (string.IsNullOrWhiteSpace(message))
                //{
                //    return;
                //}

                //Display message to user
                // Toast.MakeText(this, message.ToString(), ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                // Debug.WriteLine("Issue connecting: " + ex);
                // Toast.MakeText(this, ex.Message.ToString(), ToastLength.Long).Show();
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
                DependencyService.Get<IToast>().Show(ex.Message.ToString());
            }
            finally
            {
                
                UserDialogs.Instance.HideLoading();
                await billing.DisconnectAsync();
            }
        }

        public async void PurchaseCheck(SubscribeAmount lvscribe, string productId)
        {
            try
            {
                var id = purchase.Id;
                var token = purchase.PurchaseToken;
                var pid = purchase.ProductId;
                bool ispurchase = false;
              
                if (CrossConnectivity.Current.IsConnected)
                {
                    //await Task.Delay(10000);
                    if (lvscribe.IsMember)
                    {
                        if (lvscribe.ID == 1)
                        {
                            ispurchase = await _clientAPI.PurchaseItem(token, "96b3efec-d266-4895-a819-c45bcc591ee1", "1 M", null, platform);
                        }
                        else
                        {
                            ispurchase = await _clientAPI.PurchaseItem(token, "96b3efec-d266-4895-a819-c45bcc591ee1", "1 Y", null, platform);
                        }
                    }
                    else
                    {
                        ispurchase = await _clientAPI.PurchaseItem(token, "7d80a0e5-2636-498a-abd7-174aa4d5b46b", "1 M", lvscribe.ID, platform);
                    }
                    //if (ispurchase)
                    //{
                    Analytics.TrackEvent("In app purchase Done Successfully From Device", new Dictionary<string, string>
                            {
                                 { "Event_Property","Name : " +UserSettings.Name + "Date ="+ DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                            });

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
                    ShowMessage("Your subscription is now active. Please login again for changes to take effect. ");
                    await _navigationService.NavigateAsync(nameof(Signup_Login_HomePage));

                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check your Internet Connection");
                    //Toast.MakeText(this, _thisViewModel.Error_Please_check_your_Internet_Connection, ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
