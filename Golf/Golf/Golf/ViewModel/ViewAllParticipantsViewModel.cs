using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ViewAllParticipantsViewModel : BaseViewModel
    {

        public ViewAllParticipantsViewModel()
        {
            GetParticipantsList();
        }

        #region Property Declaration

        public ObservableCollection<AllParticipantsResponse> ParticipantItems
        {
            get { return _ParticipantItems; }
            set
            {
                _ParticipantItems = value;
                OnPropertyChanged(nameof(ParticipantItems));
            }
        }
        private ObservableCollection<AllParticipantsResponse> _ParticipantItems = null;

        private ObservableCollection<AllParticipantsResponse> OriginalPlayersList = new ObservableCollection<AllParticipantsResponse>();

        public bool NoRecordsFoundLabel
        {
            get { return _NoRecordsFoundLabel; }
            set
            {
                _NoRecordsFoundLabel = value;
                OnPropertyChanged("NoRecordsFoundLabel");
            }
        }
        private bool _NoRecordsFoundLabel = false;

        public bool ListViewIsVisible
        {
            get { return _ListViewIsVisible; }
            set
            {
                _ListViewIsVisible = value;
                OnPropertyChanged("ListViewIsVisible");
            }
        }
        private bool _ListViewIsVisible = false;

        #endregion

        #region Getparticipant List

        async void GetParticipantsList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "User/getPlayerList?SearchTerm=";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();

                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        ParticipantItems = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);
                        OriginalPlayersList = ParticipantItems;

                        if (ParticipantItems.Count > 0)
                        {
                            ListViewIsVisible = true;
                            NoRecordsFoundLabel = false;
                        }
                        else
                        {
                            ListViewIsVisible = false;
                            NoRecordsFoundLabel = true;
                        }
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

        #region Serach Command

        private ObservableCollection<AllParticipantsResponse> PlayersListItems = new ObservableCollection<AllParticipantsResponse>();

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    ParticipantItems = new ObservableCollection<AllParticipantsResponse>();

                    PlayersListItems = new ObservableCollection<AllParticipantsResponse>();

                    var query = OriginalPlayersList.Where(x => x.email.StartsWith(keyword.ToLower()) || x.playerName.ToLower().Contains(keyword.ToLower()));

                    foreach (var item in query)
                    {
                        var value = new AllParticipantsResponse() { email = item.email, gender = item.gender, ImageIcon = item.ImageIcon, isChecked = item.isChecked, IsChecked = item.IsChecked, isPublicProfile = item.isPublicProfile, isScoreKeeper = item.isScoreKeeper, nickName = item.nickName, playerName = item.playerName, profileImage = item.profileImage, roleType = item.roleType, userId = item.userId, userType = item.userType };

                        PlayersListItems.Add(value);
                    }

                    ParticipantItems = PlayersListItems;
                }
                else
                {
                    ParticipantItems = OriginalPlayersList;
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion
    }
}
