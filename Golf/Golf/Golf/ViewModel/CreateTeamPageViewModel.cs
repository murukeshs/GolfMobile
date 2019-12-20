﻿using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class CreateTeamPageViewModel : BaseViewModel
    {

        public CreateTeamPageViewModel()
        {
            StartingHoleList = new List<int>
            {
                1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18
            };
        }

        #region Property Declaration

        public List<int> StartingHoleList
        {
            get { return _StartingHoleList; }
            set
            {
                _StartingHoleList = value;
                OnPropertyChanged("StartingHoleList");
            }
        }
        private List<int> _StartingHoleList;
       
        public Plugin.Media.Abstractions.MediaFile file = null;
        ImageSource srcThumb = null;
        public byte[] imageData = null;
        public bool IsValid { get; set; }
        public string TeamNameText
        {
            get
            {
                return _TeamNameText;
            }
            set
            {
                _TeamNameText = value;
                OnPropertyChanged(nameof(TeamNameText));
            }
        }
        private string _TeamNameText = string.Empty;

        public string UserNameText
        {
            get
            {
                return _UserNameText;
            }
            set
            {
                _UserNameText = value;
                OnPropertyChanged(nameof(UserNameText));
            }
        }
        private string _UserNameText = App.User.UserName;

        public string TeamProfilePicture
        {
            get
            {
                return _TeamProfilePicture;
            }
            set
            {
                _TeamProfilePicture = value;
                OnPropertyChanged(nameof(TeamProfilePicture));
            }
        }
        private string _TeamProfilePicture = "profile_defalut_pic.png";

        public string TeamIcon = string.Empty;

        public int StartingHole
        {
            get { return _StartingHole; }
            set
            {
                _StartingHole = value;
                OnPropertyChanged("StartingHole");
            }
        }
        private int _StartingHole = -1;

        #endregion

        #region Round Picker Selected Command Functionality

        public ICommand PickerSelectedCommand => new Command<int>(SelectedIndexChangedEvent);

        void SelectedIndexChangedEvent(int shole)
        {
                StartingHole = Convert.ToInt16(shole);
        }

        #endregion Round Picker Selected Command Functionality

        #region CreateTeam Procced Button Command Functionality

        public ICommand CreateTeamProccedButtonCommand => new AsyncCommand(CreateTeamButtonAsync);

        async Task CreateTeamButtonAsync()
        {
            IsValid = Validate();
            CheckProfilePicture();
            if (IsValid)
            {
                await CreateTeamAsync();
            }
        }

        void CheckProfilePicture()
        {
            if (TeamProfilePicture == "profile_defalut_pic.png")
            {
                TeamIcon = string.Empty;
            }
            
            else
            {
                TeamIcon = TeamProfilePicture;
            }
        }

        bool Validate()
        {
            if (string.IsNullOrEmpty(TeamNameText))
            {
                UserDialogs.Instance.AlertAsync("Team Name should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (StartingHole < 0)
            {
                UserDialogs.Instance.AlertAsync("Please select starting hole.", "Alert", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        async Task CreateTeamAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Team/createTeam";
                    Uri requestUri = new Uri(RestURL);

                    var data = new TeamModel
                    {
                        createdBy = App.User.UserId,
                        teamIcon = TeamIcon,
                        teamName = TeamNameText,
                        startingHole = StartingHole,
                        roundId = App.User.CreateRoundId
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                        App.User.CreateTeamId = Item.teamId;
                        App.User.TeamName = TeamNameText;
                        var view = new AddParticipantPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                        //After the success full api process clear all the values
                        App.User.TeamPreviewList.Clear();
                        App.User.TeamPreviewScoreKeeperName = string.Empty;
                        App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                        Clear();
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

        void Clear()
        {
            TeamNameText = string.Empty;
            UserNameText = string.Empty;
            TeamIcon = string.Empty;
        }
        #endregion CreateTeam Procced Button Command Functionality

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
                    TeamProfilePicture = file.Path;
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
                    TeamProfilePicture = file.Path;
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

        async Task SendIssueImageToCloud()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "UploadFile/UploadFileBytes";
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
                    formDataContent.Add(new ByteArrayContent(imageData), "files", "Image.png");

                    var objClint = new HttpClient();
                    objClint.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    objClint.Timeout = TimeSpan.FromMilliseconds(360000);
                    objClint.MaxResponseContentBufferSize = 2147483647;
                    HttpResponseMessage response = await objClint.PostAsync(requestUri, formDataContent);
                    string responJsonText = await response.Content.ReadAsStringAsync();


                    if (response.IsSuccessStatusCode)
                    {
                        //Asign the Image URL repsonse to the Image
                        TeamProfilePicture = responJsonText;
                        CheckProfilePicture();
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
    }
}
