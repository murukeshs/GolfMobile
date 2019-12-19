using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.RoundDetailsView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class UpdateTeamViewModel : BaseViewModel
    {

        public UpdateTeamViewModel()
        {
            SetTeamDetails();
            StartingHoleList = new List<int>
            {
                1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18
            };
            TeamName = App.User.TeamName;
            LoadTeamDetails();
        }

        #region Tab Functionality

        bool isVisibleTeamDetails;

        bool isVisibleParticipantsDetails;

        public bool IsVisibleTeamDetails
        {
            get => isVisibleTeamDetails;
            set => SetProperty(ref isVisibleTeamDetails, value);
        }

        public bool IsVisibleParticipantsDetails
        {
            get => isVisibleParticipantsDetails;
            set => SetProperty(ref isVisibleParticipantsDetails, value);
        }

        public ICommand TeamDetailsCommand => new Command(SetTeamDetails);

        public ICommand ParticipantListCommand => new Command(SetParticipantList);

        void SetTeamDetails()
        {
            IsVisibleTeamDetails = true;
            IsVisibleParticipantsDetails = false;
        }

        void SetParticipantList()
        {
            IsVisibleTeamDetails = false;
            IsVisibleParticipantsDetails = true;
        }

        #endregion

        #region Property  Declartion

        public int ScoreKeeperId = 0;

        public int DefaultScoreKeeperId = 0;  

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
        private string _UserNameText = string.Empty;

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

        public int DefaultStartingHole
        {
            get { return _DefaultStartingHole; }
            set
            {
                _DefaultStartingHole = value;
                OnPropertyChanged("DefaultStartingHole");
            }
        }
        private int _DefaultStartingHole = -1;

        public string TeamName
        {
            get { return _TeamName; }
            set
            {
                _TeamName = value;
                OnPropertyChanged("TeamName");
            }
        }
        private string _TeamName = string.Empty;

        public List<AllParticipantsResponse> PlayersList
        {
            get { return _PlayersList; }
            set
            {
                _PlayersList = value;
                OnPropertyChanged(nameof(PlayersList));
            }
        }
        public List<AllParticipantsResponse> _PlayersList = new List<AllParticipantsResponse>();

        #endregion

        #region Round Picker Selected Command Functionality

        public ICommand PickerSelectedCommand => new Command(SelectedIndexChangedEvent);

        void SelectedIndexChangedEvent(object parameter)
        {
            try
            {
                var item = parameter as int?;
                StartingHole = Convert.ToInt16(item);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Round Picker Selected Command Functionality

        #region UpdateTeam Procced Button Command Functionality

        public bool IsValid { get; set; }

        public ICommand UpdateTeamProccedButtonCommand => new AsyncCommand(UpdateTeamButtonAsync);

        async Task UpdateTeamButtonAsync()
        {
            IsValid = Validate();
            CheckProfilePicture();
            if (IsValid)
            {
                await UpdateTeamAsync();
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
        async Task UpdateTeamAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Team/updateTeam";
                    Uri requestUri = new Uri(RestURL);

                    var data = new UpdateTeamModel
                    {
                        teamIcon = TeamIcon,
                        teamName = TeamNameText,
                        startingHole = StartingHole,
                        roundId = App.User.CreateRoundId,
                        teamId = App.User.CreateTeamId
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.Alert("TeamDetails Successfully Updated", "Alert", "Ok");
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

        #endregion

        #region SendImageToCloud

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

        #region Participant Button Command Functionality

        public List<RoundDetailsListTeamList> roundTeamsItemsList = new List<RoundDetailsListTeamList>();

        public ObservableCollection<RoundDetailsListTeamList> RoundTeamsItemsList
        {
            get { return _RoundTeamsItemsList; }
            set
            {
                _RoundTeamsItemsList = value;
                OnPropertyChanged(nameof(RoundTeamsItemsList));
            }
        }
        private ObservableCollection<RoundDetailsListTeamList> _RoundTeamsItemsList = null;

        async void getRoundsDetailsById()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "Round/getRoundDetailsById/" + App.User.CreateRoundId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    //List<RoundDetailsListTeamList> roundTeamsItemsListssss = new List<RoundDetailsListTeamList>();
                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<getRoundsDetailsById>>(content).ToList();


                        roundTeamsItemsList.AddRange((from item in Items
                                                      select new RoundDetailsListTeamList
                                                      {
                                                          teamId = item.teamId,
                                                          roundPlayerList = JsonConvert.DeserializeObject<List<roundPlayerList>>(item.roundPlayerList),
                                                          createdByName = item.createdByName,
                                                          NoOfPlayers = item.NoOfPlayers,
                                                          teamIcon = item.teamIcon,
                                                          teamName = item.teamName,
                                                          Expanded = false
                                                      }).ToList());

                        ObservableCollection<RoundDetailsListTeamList> myCollection = new ObservableCollection<RoundDetailsListTeamList>(roundTeamsItemsList as List<RoundDetailsListTeamList>);
                        RoundTeamsItemsList = myCollection;
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(content);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }

                    UserDialogs.Instance.HideLoading();
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

        #endregion Team Button Command Functionality

        #region LoadTeamDetails

        public async void LoadTeamDetails()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Team/selectTeamById?teamId=" + App.User.CreateTeamId;
                    Uri requestUri = new Uri(RestURL);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(requestUri);
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var Item = JsonConvert.DeserializeObject<TeamDetails>(responJsonText);
                        App.User.CreateTeamId = Item.teamId;
                        TeamNameText = Item.teamName;
                        StartingHole = Item.startingHole;
                        await LoadStartingHole(Item.startingHole);
                        TeamProfilePicture = Item.teamIcon;
                        UserNameText = Item.createdByName;
                        PlayersList = Item.TeamPlayerDetails;
                        DefaultScoreKeeperId = Item.scoreKeeperId;
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

        public async Task LoadStartingHole(int id)
        {
            try
            {
                int index = StartingHoleList.FindIndex(c => c == id);
                DefaultStartingHole = index;
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion

        #region CheckBox Selected Command Functionality

        public List<int> TeamPlayersIds = new List<int>();

        public ICommand CheckBoxSelectedCommand => new Command(CheckboxChangedEvent);

        async void CheckboxChangedEvent(object parameter)
        {
            try
            {
                var item = parameter as AllParticipantsResponse;
                var userId = item.userId;
                if (ScoreKeeperId != userId)
                {
                    if (TeamPlayersIds.Count > 0)
                    {
                        bool UserIdAleradyExists = TeamPlayersIds.Contains(userId);
                        if (UserIdAleradyExists)
                        {
                            TeamPlayersIds.Remove(userId);
                        }
                        else
                        {
                            TeamPlayersIds.Add(userId);
                        }
                    }
                    else
                    {
                        TeamPlayersIds.Add(userId);
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("score keeper can't be a Player .", "Alert", "Ok");
                    PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion CheckBox Selected Command Functionality

        #region Invite Participant Button Selected Command Functionality
        public ICommand InviteParticipantCommand => new AsyncCommand(AddParticipantsAsync);

        async Task AddParticipantsAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new InviteParticipantPage();
                await PopupNavigation.Instance.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var a = ex.Message;
            }
        }
        #endregion AddParticipants Button Command Functionality

        #region Toggle Selected Command Functionality
        public ICommand ToggleSelectedCommand => new Command(ToggleChangedEvent);
        void ToggleChangedEvent(object parameter)
        {
            try
            {
                var Item = parameter as AllParticipantsResponse;
                if (DefaultScoreKeeperId == 0)
                {
                    bool UserIdAleradyExists = TeamPlayersIds.Contains(Item.userId);
                    if (UserIdAleradyExists)
                    {
                        UserDialogs.Instance.Alert("Player can't be added as a score keeper.", "Alert", "Ok");
                    }
                    else
                    {
                        PlayersList.Where(x => x.userId == Item.userId).ToList().ForEach(s => s.ImageIcon = "checked_icon.png");
                        PlayersList.Where(x => x.userId != Item.userId).ToList().ForEach(s => s.ImageIcon = "unchecked_icon.png");
                        ScoreKeeperId = Item.userId;
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("This Team Already have a ScoreKeeper !", "Alert", "Ok");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Toggle Selected Command Functionality

        #region UpdateTeamPlayers Button Command Functionality

        public ICommand UpdateTeamButtonCommand => new AsyncCommand(UpdateTeamPlayersAsync);
        async Task UpdateTeamPlayersAsync()
        {
            try
            {
                var PlayerId = string.Join(",", TeamPlayersIds);
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (DefaultScoreKeeperId == 0 && ScoreKeeperId == 0)
                    {
                        UserDialogs.Instance.Alert("Please Select ScoreKeeperID", "Alert", "Ok");
                    }
                    else {
                        UserDialogs.Instance.ShowLoading();
                        string RestURL = App.User.BaseUrl + "Team/createTeamPlayers";
                        Uri requestUri = new Uri(RestURL);

                        var data = new TeamPlayer
                        {
                            teamId = App.User.CreateTeamId,
                            scoreKeeperID = ScoreKeeperId != 0 ? ScoreKeeperId : DefaultScoreKeeperId,
                            playerId = PlayerId,
                            roundId = App.User.CreateRoundId
                        };

                        string json = JsonConvert.SerializeObject(data);
                        var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                        var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                        string responJsonText = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            await UserDialogs.Instance.AlertAsync("Team Players Successfully Added", "Success", "Ok");
                            UserDialogs.Instance.HideLoading();
                            var view = new RoundDetailsPage();
                            var navigationPage = ((NavigationPage)App.Current.MainPage);
                            await navigationPage.PushAsync(view);
                            await Task.Delay(20);
                        }
                        else
                        {
                            var error = JsonConvert.DeserializeObject<error>(responJsonText);
                            UserDialogs.Instance.HideLoading();
                            UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                        }
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
        #endregion CreateTeamPlayers Button Command Functionality

        #region RemoveParticipant Command
     
       public ICommand RemoveParticipantCommand => new Command<AllParticipantsResponse>(RemoveParticipantAsync);

        public async void RemoveParticipantAsync(AllParticipantsResponse item)
        {

            var confirmdialog = new ConfirmConfig()
            {
                CancelText = "No",
                OkText = "Yes",
                Message = "Are sure you want to Delete?."
            };

            var result = await UserDialogs.Instance.ConfirmAsync(confirmdialog);

            if (result)
            {
                RemoveParticipant(item);
            }
            else
            {
                return;
            }
        }

        public async void RemoveParticipant(AllParticipantsResponse item)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    if (item.RoleType != "Score Keeper")
                    {
                        string RestURL = App.User.BaseUrl + "Team/deleteTeamPlayers?teamPlayerListId=" + item.teamPlayerListId;
                        Uri requestUri = new Uri(RestURL);
                        var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                        var response = await httpClient.DeleteAsync(requestUri);
                        string responJsonText = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            LoadTeamDetails();
                            UserDialogs.Instance.Alert("Participant Successfully Removed", "Success", "Ok");
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
                        UserDialogs.Instance.Alert("ScoreKeeper Successfully Removed", "Success", "Ok");
                        PlayersList.Where(x => x.userId == item.userId).ToList().ForEach(s => s.isChecked = false);
                        DefaultScoreKeeperId = 0;
                        UserDialogs.Instance.HideLoading();
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
        #endregion
    }
}
