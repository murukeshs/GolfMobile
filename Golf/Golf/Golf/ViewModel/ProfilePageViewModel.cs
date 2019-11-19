using Acr.UserDialogs;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Golf.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public Plugin.Media.Abstractions.MediaFile file = null;
        ImageSource srcThumb = null;
        public byte[] imageData = null;

        public ProfilePageViewModel()
        {
            loadProfileData();
        }

        public string ProfileImage
        {
            get { return _ProfileImage; }
            set
            {
                _ProfileImage = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }
        private string _ProfileImage = "profile_defalut_pic.png";

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

        public string Dob
        {
            get { return _Dob; }
            set
            {
                _Dob = value;
                OnPropertyChanged(nameof(Dob));
            }
        }
        private string _Dob = string.Empty;

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

        public string UserType
        {
            get { return _UserType; }
            set
            {
                _UserType = value;
                OnPropertyChanged(nameof(UserType));
            }
        }
        private string _UserType = string.Empty;

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
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
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
            catch (Exception)
            {
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }


        #endregion

        async Task SendIssueImageToCloud()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "UploadFile/UploadFile";
                    Uri requestUri = new Uri(RestURL);

                    //convert image into bytes
                    string FileName = string.Empty;
                    Stream stream = file.GetStream();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        imageData = ms.ToArray();
                    }
                    var formDataContent = new MultipartFormDataContent();
                    formDataContent.Add(new ByteArrayContent(imageData), "files", "Image");
                    //formDataContent.Add(new StringContent(json, System.Text.Encoding.UTF8, "application/json"), "myJsonObject");

                    var objClint = new HttpClient();
                    objClint.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    objClint.MaxResponseContentBufferSize = 1000000;
                    HttpResponseMessage response = await objClint.PostAsync(requestUri, formDataContent);
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var imagedetails = JsonConvert.DeserializeObject<Global>(responJsonText);
                        //Asign the Image URL repsonse to the Image
                        ProfileImage = responJsonText;
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(responJsonText);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }

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
            await ((NavigationPage)App.Current.MainPage).PopAsync();//this line navigate to previous page of your application
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
                    var RestURL = App.User.BaseUrl + "User/selectUserById/" + App.User.UserId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                      UserData User = JsonConvert.DeserializeObject<UserData>(content);
                        Email = User.email;
                        FirstName = User.firstName;
                        LastName = User.lastName;
                        Dob = User.dob;
                        Gender = User.gender;
                        UserType = User.userType;
                        IsEmailNotification = User.isEmailNotification;
                        IsSmsNotification = User.isSMSNotification;
                        Address = User.address;
                        CountryID = User.countryId;
                        StateID = User.stateId;
                        City = User.city;
                        IsPublicProfile = User.isPublicProfile;
                      UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(content);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }
        #endregion

        public List<string> UserTypeList = new List<string>();
        public ICommand UserTypeCheckBoxCommand => new Command(UserTypeCheckBox);
        public void UserTypeCheckBox(object parameter)
        {
            var item = parameter as string;

            if (UserTypeList.Count > 0)
            {
                bool UserIdAleradyExists = UserTypeList.Contains(item);
                if (UserIdAleradyExists)
                {
                    UserTypeList.Remove(item);
                }
                else
                {
                    UserTypeList.Add(item);
                }
            }
            else
            {
                UserTypeList.Add(item);
            }
        }
    }
}
