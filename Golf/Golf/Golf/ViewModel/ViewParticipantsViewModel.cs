using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ViewParticipantsViewModel : BaseViewModel
    {

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


        public ICommand ItemTappedCommand { get; private set; }

        public ViewParticipantsViewModel()
        {
            ItemTappedCommand = new Command<AllParticipantsResponse>(GetViewParticipantsDetails);
            GetParticipantsList();
            //LoadPlayerListAsync();
        }

        async void GetViewParticipantsDetails(AllParticipantsResponse obj)
        {

        }


        #region Toggle Selected Command Functionality
        public ICommand ToggleSelectedCommand => new Command(ToggleChangedEvent);
        //private user _LastSelectedItem;
        void ToggleChangedEvent(object parameter)
        {
            try
            {
                var Item = parameter as AllParticipantsResponse;
                //PlayersList.All(x => x.userId == item.userId ?  x.IsToggled = true : x.IsToggled = false);

                // Get every record that is checked
                // The variable "Items" is the ItemsSource of your ListView
                // var checkedItems = PlayersList.Where(x => x.IsChecked == true).ToList();
                // Do stuff with item
                foreach (var item in ParticipantItems)
                {
                    if (item.userId == Item.userId)
                    {
                        ParticipantItems.Where(x => x.userId == Item.userId).ToList().ForEach(s => s.ImageIcon = "checked_icon.png");
                        //ScoreKeeperId = item.userId;
                        App.User.TeamPreviewScoreKeeperName = item.playerName;
                        App.User.TeamPreviewScoreKeeperProfilePicture = item.profileImage;
                    }
                    else
                    {
                        ParticipantItems.Where(x => x.userId != Item.userId).ToList().ForEach(s => s.ImageIcon = "unchecked_icon.png");
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }



        #endregion Toggle Selected Command Functionality

        async void GetParticipantsList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "Team/selectTeamById?teamId=" + App.User.TeamIdforPlayerListing;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();

                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        ParticipantItems = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);
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
                        await Task.Delay(10);
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


        #region CheckBox Selected Command Functionality
        public List<int> TeamPlayersIds = new List<int>();
        // string joined = string.Join(",", TeamPlayersIds);
        public ObservableCollection<AddPlayersList> TeamPreviewList = new ObservableCollection<AddPlayersList>();
        public ICommand CheckBoxSelectedCommand => new Command(CheckboxChangedEvent);

        async void CheckboxChangedEvent(object parameter)
        {
            var item = parameter as user;
            var userId = item.userId;
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
        #endregion CheckBox Selected Command Functionality



    }
}
