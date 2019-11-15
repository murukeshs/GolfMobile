using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
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
                        if(Items.Count > 0)
                        {
                            ListViewIsVisible = true;
                            NoRecordsFoundLabel = false;
                        }
                        else
                        {
                            ListViewIsVisible = false;
                            NoRecordsFoundLabel = true;
                        }
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
    }
}
