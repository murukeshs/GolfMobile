using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using System;
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


        #region CreateTeam Procced Button Command Functionality
        public ICommand CreateTeamProccedButtonCommand => new AsyncCommand(CreateTeamButtonAsync);
        async Task CreateTeamButtonAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
                CheckProfilePicture();
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
            else
            {
                return true;
            }
        }


        ////Team Creatiation API
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
                        createdBy = App.User.UserWithTypeId,
                        teamIcon = TeamIcon,
                        teamName = TeamNameText
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
                    TeamProfilePicture = file.Path;
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
                    var fileName = "ProfileImage" + DateTime.Now.ToString();
                    var formDataContent = new MultipartFormDataContent();
                    formDataContent.Add(new ByteArrayContent(imageData), "files", fileName);
                    //formDataContent.Add(new StringContent(json, System.Text.Encoding.UTF8, "application/json"), "myJsonObject");

                    var objClint = new HttpClient();
                    objClint.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                   // objClint.MaxResponseContentBufferSize = 5000000000;
                    HttpResponseMessage response = await objClint.PostAsync(requestUri, formDataContent);
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var imagedetails = JsonConvert.DeserializeObject<Global>(responJsonText);
                        //Asign the Image URL repsonse to the Image
                        TeamProfilePicture = responJsonText;
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

        #endregion
    }
}
