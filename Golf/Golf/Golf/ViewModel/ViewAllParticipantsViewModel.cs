using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ViewAllParticipantsViewModel : BaseViewModel
    {

        public ViewAllParticipantsViewModel()
        {
            GetParticipantsList();
            MessagingCenter.Subscribe<App>((App)Application.Current, "VIEWALLPARTICIPANTSPAGEREFRESH", (sender) => {
                GetParticipantsList();
            });
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

        public ICommand GetParticipantsListCommand => new AsyncCommand(GetParticipantsList);

        public async Task GetParticipantsList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetParticipantsList();

                    if(result != null)
                    {
                        ParticipantItems = result;
                        OriginalPlayersList = result;

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

                    

                    //var RestURL = App.User.BaseUrl + "User/getPlayerList?SearchTerm=";
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();

                    ////Assign the Values to Listview
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    ParticipantItems = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);
                    //    OriginalPlayersList = ParticipantItems;

                    //    if (ParticipantItems.Count > 0)
                    //    {
                    //        ListViewIsVisible = true;
                    //        NoRecordsFoundLabel = false;
                    //    }
                    //    else
                    //    {
                    //        ListViewIsVisible = false;
                    //        NoRecordsFoundLabel = true;
                    //    }
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
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

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                ParticipantItems = new ObservableCollection<AllParticipantsResponse>();

                if (!string.IsNullOrEmpty(keyword))
                {
                    ParticipantItems = new ObservableCollection<AllParticipantsResponse>(OriginalPlayersList.Where(x => x.email.StartsWith(keyword.ToLower()) || x.playerName.ToLower().Contains(keyword.ToLower())));
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

        #region Invite Participant 
        public ICommand InviteParticipantCommand => new Command<string>(InviteParticipant);

        public async void InviteParticipant(string keyword)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new InviteParticipantPage("viewallparticipantspage");
                await PopupNavigation.Instance.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
    }
}
