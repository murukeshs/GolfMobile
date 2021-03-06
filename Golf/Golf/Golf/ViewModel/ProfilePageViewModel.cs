﻿using Acr.UserDialogs;
using Golf.Models;
using Golf.Models.userModel;
using Golf.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using System;
using Golf.Utils;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Golf.Views;
using System.Globalization;

namespace Golf.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {

        public ProfilePageViewModel()
        {
            SetPersonalInfo();

            if (!string.IsNullOrEmpty(App.User.UserProfileImage))
            {
                ProfileImage = App.User.UserProfileImage;
            }
            else
            {
                ProfileImage = "profile_defalut_pic.png";
            }

            GenderList = new List<GenderType>()
            {
                new GenderType{gender = "Male", genderId = 1},
                new GenderType{gender = "Female", genderId = 2}
            };

            loadProfileData();
        }

        #region Tab Functionality

        bool personalInfo;
        bool communicationInfo;

        public bool PersonalInfo
        {
            get => personalInfo;
            set => SetProperty(ref personalInfo, value);
        }

        public bool CommunicationInfo
        {
            get => communicationInfo;
            set => SetProperty(ref communicationInfo, value);
        }

        public ICommand PersonalInfoCommand => new Command(SetPersonalInfo);

        public ICommand CommunicationInfoCommand => new Command(SetCommunicationInfo);

        void SetPersonalInfo()
        {
            PersonalInfo = true;
            CommunicationInfo = false;
        }

        void SetCommunicationInfo()
        {
            PersonalInfo = false;
            CommunicationInfo = true;
        }

        #endregion

        #region Property Declaration

        public bool IsValid { get; set; }
       
        public Plugin.Media.Abstractions.MediaFile file = null;
        ImageSource srcThumb = null;
        public byte[] imageData = null;

        public List<GenderType> GenderList { get; set; }

    
        public string ProfileImage
        {
            get { return _ProfileImage; }
            set
            {
                _ProfileImage = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }
        private string _ProfileImage = string.Empty;

        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _Email = string.Empty;

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string _FirstName = string.Empty;

        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string _LastName = string.Empty;

        public string NickName
        {
            get { return _NickName; }
            set
            {
                _NickName = value;
                OnPropertyChanged(nameof(NickName));
            }
        }
        private string _NickName = string.Empty;

        public DateTime? Dob
        {
            get { return _Dob; }
            set
            {
                _Dob = value;
                OnPropertyChanged(nameof(Dob));
            }
        }
        private DateTime? _Dob = null;

        public DateTime? NullableDob
        {
            get { return _NullableDob; }
            set
            {
                _NullableDob = value;
                OnPropertyChanged(nameof(NullableDob));
            }
        }
        private DateTime? _NullableDob = null;

        public string Gender
        {
            get { return _Gender; }
            set
            {
                _Gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        private string _Gender = string.Empty;

        public int GenderId
        {
            get { return _GenderId; }
            set
            {
                _GenderId = value;
                OnPropertyChanged(nameof(GenderId));
            }
        }
        private int _GenderId;

        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        private string _Address = string.Empty;

        public string City
        {
            get { return _City; }
            set
            {
                _City = value;
                OnPropertyChanged(nameof(City));
            }
        }
        private string _City = string.Empty;

        public string EmailName
        {
            get { return _EmailName; }
            set
            {
                _EmailName = value;
                OnPropertyChanged(nameof(EmailName));
            }
        }
        private string _EmailName = string.Empty;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set
            {
                _PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        private string _PhoneNumber = string.Empty;

        public int StateID
        {
            get { return _StateID; }
            set
            {
                _StateID = value;
                OnPropertyChanged(nameof(StateID));
            }
        }
        private int _StateID = 0;

        public int? stateID
        {
            get { return _stateID; }
            set
            {
                _stateID = value;
                OnPropertyChanged(nameof(stateID));
            }
        }
        private int? _stateID = -1;

        public int CountryID
        {
            get { return _CountryID; }
            set
            {
                _CountryID = value;
                OnPropertyChanged(nameof(CountryID));
            }
        }
        private int _CountryID = 0;

        public int countryID
        {
            get { return _countryID; }
            set
            {
                _countryID = value;
                OnPropertyChanged(nameof(countryID));
            }
        }
        private int _countryID;

        public bool? IsPublicProfile
        {
            get { return _IsPublicProfile; }
            set
            {
                _IsPublicProfile = value;
                OnPropertyChanged(nameof(IsPublicProfile));
            }
        }
        private bool? _IsPublicProfile = false;

        public bool? IsEmailNotification
        {
            get { return _IsEmailNotification; }
            set
            {
                _IsEmailNotification = value;
                OnPropertyChanged(nameof(IsEmailNotification));
            }
        }
        private bool? _IsEmailNotification = false;

        public bool? IsSmsNotification
        {
            get { return _IsSmsNotification; }
            set
            {
                _IsSmsNotification = value;
                OnPropertyChanged(nameof(IsSmsNotification));
            }
        }
        private bool? _IsSmsNotification = false;

        public List<Country> CountryList
        {
            get { return _CountryList; }
            set
            {
                _CountryList = value;
                OnPropertyChanged(nameof(CountryList));
            }
        }
        private List<Country> _CountryList = null;

        public List<State> StateList
        {
            get { return _StateList; }
            set
            {
                _StateList = value;
                OnPropertyChanged(nameof(StateList));
            }
        }
        private List<State> _StateList = null;

        #endregion

        #region Image Upload Functionality

        #region TakePicture Command Functionality

        public ICommand TakeCaptureCommand => new AsyncCommand(CaptureImageButton_Clicked);

        async Task CaptureImageButton_Clicked()
        {
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    UserDialogs.Instance.Alert("No camera available.", "No Camera", "OK");
                    return;
                }

                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    Name = "test.jpg",
                    SaveToAlbum = true,
                    CompressionQuality = 20,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear
                });

                if (file == null)
                    return;

                if (file != null)
                {
                    srcThumb = ImageSource.FromStream(() =>
                    {
                        var streamImg = file.GetStream();
                        return streamImg;
                    });
                    ProfileImage = file.Path;
                    await SendIssueImageToCloud();
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }


        #endregion

        #region Gallery Command Functionality
        public ICommand GalleryCommand => new AsyncCommand(GalleryImageButton_Clicked);
        private async Task GalleryImageButton_Clicked()
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    UserDialogs.Instance.Alert("Permission not granted to photos.", "Photos Not Supported", "OK");
                    return;
                }

                file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {

                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 20

                });

                if (file != null)
                {
                    srcThumb = ImageSource.FromStream(() =>
                    {
                        var streamImg = file.GetStream();
                        return streamImg;
                    });
                    ProfileImage = file.Path;
                    await SendIssueImageToCloud();
                }
                if (file == null)
                    return;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }


        #endregion

        #region SendIssueImageTo Cloud Functionality

        async Task SendIssueImageToCloud()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    //convert image into bytes
                    Stream stream = file.GetStream();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        imageData = ms.ToArray();
                    }

                    var result = await App.ApiClient.UploadFile(imageData);

                    if (result != null)
                    {
                        //Asign the Image URL repsonse to the Image
                        ProfileImage = result;
                        MessagingCenter.Send<App, string>((App)Application.Current, App.User.EVENT_REFRESH_PROFILE_ICON, result);
                    }

                    UserDialogs.Instance.HideLoading();

                    //string RestURL = App.User.BaseUrl + "UploadFile/UploadFileBytes";
                    //Uri requestUri = new Uri(RestURL);
                    //var formDataContent = new MultipartFormDataContent();
                    //formDataContent.Add(new ByteArrayContent(imageData), "files", "Image.png");
                    //var objClint = new HttpClient();
                    //objClint.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //objClint.Timeout = TimeSpan.FromMilliseconds(360000);
                    //objClint.MaxResponseContentBufferSize = 2147483647;
                    //HttpResponseMessage response = await objClint.PostAsync(requestUri, formDataContent);
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    //Asign the Image URL repsonse to the Image
                    //    ProfileImage = responJsonText;
                    //    MessagingCenter.Send<App, string>((App)Application.Current, App.User.EVENT_REFRESH_PROFILE_ICON, responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }

        #endregion

        #endregion

        #region Logout Command Functionality

        public ICommand LogoutCommand => new  AsyncCommand(LogoutAsync);

        async Task LogoutAsync()
        {
            
            var confirmdialog = new ConfirmConfig()
            {
                CancelText = "No",
                OkText = "Yes",
                Message = "Are sure you want to Logout?."
            };

            var result = await UserDialogs.Instance.ConfirmAsync(confirmdialog);

            if(result)
            {
                LogoutFunction();
            }
            else
            {
                return;
            }
        }
        async void LogoutFunction()
        {
            try
            {
                var LoginView = new LoginPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(LoginView);
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion

        #region loadProfileData

        public async void loadProfileData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetUserByUserId(App.User.UserId);
                    if (result != null)
                    {
                        UserData User = result;
                        Email = User.email;
                        FirstName = User.firstName;
                        LastName = User.lastName;
                        //Date Time Issue
                        var format = "MM/dd/yyyy hh:mm:ss tt";
                        var dateTime = DateTime.ParseExact(User.dob, format, CultureInfo.InvariantCulture);
                        Dob = dateTime;
                        await Task.Delay(500);
                        Gender = User.gender;
                        EmailName = User.email;
                        PhoneNumber = User.phoneNumber;
                        LoadGender();
                        IsEmailNotification = User.isEmailNotification;
                        IsSmsNotification = User.isSMSNotification;
                        Address = User.address;
                        CountryID = User.countryId;
                        loadCountry();
                        City = User.city;
                        IsPublicProfile = User.isPublicProfile;
                        NickName = User.nickName;
                        StateID = User.stateId;
                        CountryOnChange(CountryID);
                        UserDialogs.Instance.HideLoading();
                    }


                    //var RestURL = App.User.BaseUrl + "User/selectUserById/" + App.User.UserId;
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //  UserData User = JsonConvert.DeserializeObject<UserData>(content);
                    //    Email = User.email;
                    //    FirstName = User.firstName;
                    //    LastName = User.lastName;
                    //   //Date Time Issue
                    //    var format = "MM/dd/yyyy hh:mm:ss tt";
                    //    var dateTime = DateTime.ParseExact(User.dob, format, CultureInfo.InvariantCulture);
                    //    Dob = dateTime;
                    //    await Task.Delay(500);
                    //    Gender = User.gender;
                    //    EmailName = User.email;
                    //    PhoneNumber = User.phoneNumber;
                    //    LoadGender();    
                    //    IsEmailNotification = User.isEmailNotification;
                    //    IsSmsNotification = User.isSMSNotification;
                    //    Address = User.address;
                    //    CountryID = User.countryId;
                    //    loadCountry();
                    //    City = User.city;
                    //    IsPublicProfile = User.isPublicProfile;
                    //    NickName = User.nickName;
                    //    StateID = User.stateId;
                    //    CountryOnChange(CountryID);
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }

        void LoadGender()
        {
            if (Gender == "Male")
            {
                GenderId = 0;
            }
            else
            {
                GenderId = 1;
            }
        }

        #endregion

        #region Date Changed Event Command

        public ICommand DateChangedEventCommand => new Command(DateChanged);

        void DateChanged(object parameter)
        {
            try
            {
                var item = parameter as string;
                Dob = Convert.ToDateTime(item);
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion

        #region Round Picker Selected Command Functionality

        public ICommand PickerSelectedCommand => new Command<int>(SelectedIndexChangedEvent);

        void SelectedIndexChangedEvent(int gender)
        {
            if (gender == 0)
            {
                GenderId = 0;
                Gender = "Male";
            }
            else
            {
                GenderId = 1;
                Gender = "Female";
            }
        }
        #endregion Round Picker Selected Command Functionality
      
        #region loadcountry
        public async void loadCountry()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetCountryList();

                    if (result != null)
                    {
                        CountryList = result;

                        foreach (var a in CountryList) //CountryID
                        {
                            var countryIdValue = a.countryId;
                            if (countryIdValue == CountryID)
                            {
                                CountryID = a.countryId;
                                int index = CountryList.FindIndex(aa => aa.countryId == CountryID);
                                countryID = index;
                            }
                        }
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Country/GetCountryList";
                    //HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri(RestURL);
                    //HttpResponseMessage response = await client.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    CountryList = JsonConvert.DeserializeObject<List<Country>>(content);

                    //    foreach (var a in CountryList) //CountryID
                    //    {
                    //        var countryIdValue = a.countryId;
                    //        if (countryIdValue == CountryID)
                    //        {
                    //            CountryID = a.countryId;
                    //            int index = CountryList.FindIndex(aa => aa.countryId == CountryID);
                    //            countryID = index;
                    //        }
                    //    }

                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion

        #region loadstate

        public ICommand CountryChangedCommand => new Command<int>(CountryOnChange);

        public async void CountryOnChange(int countryId)
        {
            try
            {
                CountryID = countryId;
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetStateList(countryId);
                    if (result != null)
                    {
                        StateList = result;
                        int index = StateList.FindIndex(a => a.stateId == StateID);
                        stateID = index;
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Country/GetStateList/" + countryId;
                    //HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri(RestURL);
                    //HttpResponseMessage response = await client.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    StateList = JsonConvert.DeserializeObject<List<State>>(content);
                    //    int index = StateList.FindIndex(a => a.stateId == StateID);
                    //    stateID = index;
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion

        #region State Change Functionality

        public ICommand StateChangedCommand => new Command<int>(StateOnChange);

        void StateOnChange(int id)
        {
            StateID = id;
        }

        #endregion

        #region UpdateCommunicationInfo Command

        public ICommand UpdateCommunicationInfoCommand => new AsyncCommand(UpdateCommunicationInfo);

        async Task UpdateCommunicationInfo()
        {
            IsValid = Validate();
            if (IsValid)
            {
                await Register();
            }
        }

        bool Validate()
        {
            if (string.IsNullOrEmpty(Address))
            {
                //Address Is Empty
                UserDialogs.Instance.AlertAsync("Address should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (CountryID == 0)
            {
                //Country Is Empty
                UserDialogs.Instance.AlertAsync("Country cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (StateID == 0 || stateID == -1)
            {
                //State Is Empty
                UserDialogs.Instance.AlertAsync("State cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(City))
            {
                //City Is Empty
                UserDialogs.Instance.AlertAsync("City cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (IsEmailNotification == false && IsSmsNotification == false)
            {
                //Notification Type Is Empty
                UserDialogs.Instance.AlertAsync("Notification Type cannot be empty.", "Alert", "Ok");
                return false;
            }
            else
            {
                //Validation Is Success
                return true;
            }
        }
        async Task Register()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    

                    var data = new createUser
                    {
                        userId = App.User.UserId,
                        address = Address,
                        stateId = StateID,
                        countryId = CountryID,
                        city = City,
                        isEmailNotification = IsEmailNotification,
                        isSMSNotification = IsSmsNotification,
                        isPublicProfile = IsPublicProfile
                    };

                    var result = await App.ApiClient.UpdateUserCommunicationInfo(data);

                    if (result != null)
                    {
                        UserDialogs.Instance.Alert("Communication Info Updated Successfully", "Success", "Ok");
                    }

                    UserDialogs.Instance.HideLoading();

                    //string RestURL = App.User.BaseUrl + "User/updateUserCommunicationinfo";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    UserDialogs.Instance.Alert("Communication Info Updated Successfully", "Success", "Ok");
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion

        #region UpdateProfileInfo Command

        public ICommand UpdateUserInfoCommand => new AsyncCommand(UpdateUserInfo);

        async Task UpdateUserInfo()
        {
            IsValid = ValidateUserInfo();
            if (IsValid)
            {
                await RegisterUserInfo();
            }
        }

        bool ValidateUserInfo()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                //First Name Is Empty
                UserDialogs.Instance.AlertAsync("FirstName should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(LastName))
            {
                //LastName Is Empty
                UserDialogs.Instance.AlertAsync("LastName cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(NickName))
            {
                //NickName Is Empty
                UserDialogs.Instance.AlertAsync("NickName cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(Email))
            {
                //Email Is Empty
                UserDialogs.Instance.AlertAsync("Email cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (!Regex.IsMatch(Email, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$"))
            {
                UserDialogs.Instance.AlertAsync("Email format is not valid.", "Alert", "Ok");
                return false;
            }
            else if (string.IsNullOrEmpty(Gender))
            {
                //Gender Is Empty
                UserDialogs.Instance.AlertAsync("Gender cannot be empty.", "Alert", "Ok");
                return false;
            }
            else if (Dob == null)
            {
                //dob Is Empty
                UserDialogs.Instance.AlertAsync("DOB cannot be empty.", "Alert", "Ok");
                return false;
            }
            else
            {
                //Validation Is Success
                return true;
            }
        }

        public int typeid;
        async Task RegisterUserInfo()
        {
            try
            {
                var dob = Dob.Value.ToString("yyyy/MM/dd");
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    if(string.IsNullOrEmpty(ProfileImage) || ProfileImage == null || ProfileImage == "profile_defalut_pic.png")
                    {
                        ProfileImage = null;
                    }


                    var data = new createUser
                    {
                        userId = App.User.UserId,
                        firstName = FirstName,
                        lastName = LastName,
                        nickName = NickName,
                        email = EmailName,
                        profileImage = ProfileImage,
                        gender = Gender,
                        dob = dob,
                        phoneNumber = PhoneNumber,
                    };

                    var result = await App.ApiClient.UpdateUser(data);

                    if (result != null)
                    {
                        UserDialogs.Instance.Alert("ProfileInfo SuccessFully Updated", "Alert", "Ok");
                        if (ProfileImage != null)
                        {
                            App.User.UserProfileImage = ProfileImage;
                        }
                    }

                    UserDialogs.Instance.HideLoading();

                    //string RestURL = App.User.BaseUrl + "User/updateUser";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //HttpResponseMessage response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    UserDialogs.Instance.Alert("ProfileInfo SuccessFully Updated", "Alert", "Ok");
                    //    if (ProfileImage != null)
                    //    {
                    //        App.User.UserProfileImage = ProfileImage;
                    //    }
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion

        #region Notification Change Functionality
        public ICommand NotificationTypeChangedCommand => new Command<string>(NotificationTypeChanged);

        void NotificationTypeChanged(string type)
        {
            if (type == "Email")
            {
                IsEmailNotification = true;
            }
            if (type == "SMS")
            {
                IsSmsNotification = true;
            }
        }

        #endregion
    }
}
