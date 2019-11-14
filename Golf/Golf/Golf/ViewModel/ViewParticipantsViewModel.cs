using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ViewParticipantsViewModel : BaseViewModel
    {

        public ObservableCollection<ParticipantList> ParticipantItems
        {
            get { return _ParticipantItems; }
            set
            {
                _ParticipantItems = value;
                OnPropertyChanged(nameof(ParticipantItems));
            }
        }
        private ObservableCollection<ParticipantList> _ParticipantItems = null;
       
        public ICommand ItemTappedCommand { get; private set; }

        public ViewParticipantsViewModel()
        {
            ItemTappedCommand = new Command<ParticipantList>(GetViewParticipantsDetails);
            GetParticipantsList();
        }

        async void GetViewParticipantsDetails(ParticipantList obj)
        {
            try
            {
                //var msg = "Player Name is" + obj.ParticipantName;
                UserDialogs.Instance.Alert("Okay", "View Participants", "Ok");
            }
            catch(Exception ex)
            {

            }
        }

        async void GetParticipantsList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "User/getPlayerList?SearchTerm="+App.User.TeamIdforPlayerListing;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();

                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<ParticipantList>>(content);
                         ParticipantItems = Items;
                    }
                    else
                    {
                        DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
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
    }
}
