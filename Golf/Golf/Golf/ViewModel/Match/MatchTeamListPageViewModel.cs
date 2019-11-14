using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchTeamListPageViewModel : BaseViewModel
    {
        public MatchTeamListPageViewModel()
        {
            LoadTeamListAsync();
        }

        public ObservableCollection<MatchTeamItems> MatchTeamsItemsList
        {
            get { return _MatchTeamsItemsList; }
            set
            {
                _MatchTeamsItemsList = value;
                OnPropertyChanged(nameof(MatchTeamsItemsList));
            }
        }
        private ObservableCollection<MatchTeamItems> _MatchTeamsItemsList = null;


        #region SaveTeamButtonCLicked Command Functionality
      
        public ICommand SaveTeamsCommand => new AsyncCommand(SaveTeamButtonCLicked);

        private async Task SaveTeamButtonCLicked()
        {
            //ViewModelNavigation.PushAsync(new ItemPageViewModel() { Item = parameter as RssItem });
            await createMatchplayersAsync();
        }
        #endregion SaveTeamButtonCLicked Command Functionality


        public List<int> TeamIdList = new List<int>();
        public ICommand CheckBoxSelectedCommand => new Command(CheckBoxChanged);

        void CheckBoxChanged(object parameter)
        {
            var item = parameter as MatchTeamItems;
            var teamId = item.teamId;
            if (TeamIdList.Count > 0)
            {
                bool UserIdAleradyExists = TeamIdList.Contains(teamId);
                if (UserIdAleradyExists)
                {
                    TeamIdList.Remove(teamId);
                }
                else
                {
                    TeamIdList.Add(teamId);
                }
            }
            else
            {
                TeamIdList.Add(teamId);
            }
        }


        async void LoadTeamListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "Team/listTeam";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                   
                    //Assign the Values to Listview
                    if(response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<MatchTeamItems>>(content);
                        MatchTeamsItemsList = Items;
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
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }


        async Task createMatchplayersAsync()
        {
            try
            {
                var teamPlayerId = string.Join(",", TeamIdList);

                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Match/createMatchplayer";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateMatchPlayers
                    {
                        type = "Match",
                        eventId = App.User.CreateMatchId,
                        teamId = teamPlayerId,
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var Item = JsonConvert.DeserializeObject<createMatchResponse>(responJsonText);
                        var view = new SendInvitePoppup();
                        await PopupNavigation.Instance.PushAsync(view);
                        //After the success full api process clear all the values
                        //Clear();
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

    }
}
