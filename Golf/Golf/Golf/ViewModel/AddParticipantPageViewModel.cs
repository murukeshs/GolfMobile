using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.MenuView;
using Golf.Views.PoppupView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class AddParticipantPageViewModel : BaseViewModel
    {
        public int ScoreKeeperId;
        public AddParticipantPageViewModel()
        {
            LoadPlayerListAsync();
        }


        #region PlayerList API Functionality
        public ObservableCollection<user> PlayersList
        {
            get { return _PlayersList;}
            set
            {
                _PlayersList = value;
                OnPropertyChanged(nameof(PlayersList));
            }
        }
        public ObservableCollection<user> _PlayersList = null;

        async void LoadPlayerListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "User/listUser?userType=" + 1;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<ObservableCollection<user>>(content);
                    //Assign the Values to Listview
                    PlayersList = Items;
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
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }

        #endregion PlayerList API Functionality


        #region TeamPreview Command Functionality
        public ICommand TeamPreviewCommand => new AsyncCommand(TeamPreviewButtonAsync);
        async Task TeamPreviewButtonAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                await PopupNavigation.Instance.PushAsync(new TeamPreviewPage());
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion TeamPreview Command Functionality

        #region CheckBox Selected Command Functionality
        public List<int> TeamPlayersIds = new List<int>();
       // string joined = string.Join(",", TeamPlayersIds);

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


        #region CreateTeamPlayers Button Command Functionality

        public ICommand CreateTeamButtonCommand => new AsyncCommand(CreateTeamPlayersAsync);
        //Create Team Players API Function
        async Task CreateTeamPlayersAsync()
        {
            try
            {
                var PlayerId = string.Join(",", TeamPlayersIds);
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Team/createTeamPlayers";
                    Uri requestUri = new Uri(RestURL);

                    var data = new TeamPlayer
                    {
                       teamId = App.User.CreateTeamId,
                       scoreKeeperID = ScoreKeeperId,
                       playerId = PlayerId,
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                        //App.User.CreateTeamId = Item.teamId;
                        var view = new MenuPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                        ////After the success full api process clear all the values
                        //Clear();
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                        UserDialogs.Instance.HideLoading();
                        DependencyService.Get<IToast>().Show(Item.ErrorMessage);
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


        #region Toggle Selected Command Functionality

        public ICommand ToggleSelectedCommand => new Command(ToggleChangedEvent);
       //private user _LastSelectedItem;
        void ToggleChangedEvent(object parameter)
        {
            var item = parameter as user;
            ScoreKeeperId = item.userId;

            //if (_LastSelectedItem == item)
            //{
            //    item.IsToggled = !item.IsToggled;
            //    UpdateItems(item);
            //}
            //else
            //{
            //    if (_LastSelectedItem != null)
            //    {
            //        //hide the previous selected item
            //        _LastSelectedItem.IsToggled = false;
            //         UpdateItems(_LastSelectedItem);
            //    }
            //    //Or show the selected item
            //    item.IsToggled = true;
            //    UpdateItems(item);
            //}
            //_LastSelectedItem = item;
        }


        //To Hide and UnHide Players details Page
       


        //async void UpdateItems(user Items)
        //{
        //    var index = PlayersList.IndexOf(Items);
        //    PlayersList.Remove(Items);
        //    PlayersList.Insert(index, Items);
        //    OnPropertyChanged(nameof(PlayersList));
        //}
        #endregion Toggle Selected Command Functionality

    }

}


