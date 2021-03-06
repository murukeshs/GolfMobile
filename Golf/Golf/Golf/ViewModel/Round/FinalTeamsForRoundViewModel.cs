﻿using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Views;
using Golf.Views.CreateRoundView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class FinalTeamsForRoundViewModel : BaseViewModel
    {
        public FinalTeamsForRoundViewModel()
        {
            GetRoundsDetailsById();
        }

        #region PropertyDeclaration

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

        #endregion

        #region CreateAnotherTeam Command
        public ICommand CreateAnotherTeamCommand => new Command(CreateAnotherTeam);
        public async void CreateAnotherTeam()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                var view = new CreateTeamPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Complete Round Command

        public ICommand CompleteRoundCommand => new Command(CompleteRound);

        public async void CompleteRound()
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.SendRoundNotification(App.User.CreateRoundId);

                    if (result != null)
                    {
                        await PopupNavigation.Instance.PushAsync(new SendInvitePoppup());
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Round/sendroundnotification/" + App.User.CreateRoundId;
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    UserDialogs.Instance.HideLoading();
                    //    await PopupNavigation.Instance.PushAsync(new SendInvitePoppup());
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

        #region GetRoundDetails

        async void GetRoundsDetailsById()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetRoundDetailsByRoundId(App.User.CreateRoundId);

                    if (result != null)
                    {
                        var Items = result;

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

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Round/getRoundDetailsById/" + App.User.CreateRoundId;
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    ////Assign the Values to Listview
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    var Items = JsonConvert.DeserializeObject<ObservableCollection<getRoundsDetailsById>>(content).ToList();


                    //    roundTeamsItemsList.AddRange((from item in Items
                    //                                  select new RoundDetailsListTeamList
                    //                                  {
                    //                                      teamId = item.teamId,
                    //                                      roundPlayerList = JsonConvert.DeserializeObject<List<roundPlayerList>>(item.roundPlayerList),
                    //                                      createdByName = item.createdByName,
                    //                                      NoOfPlayers = item.NoOfPlayers,
                    //                                      teamIcon = item.teamIcon,
                    //                                      teamName = item.teamName,
                    //                                      Expanded = false
                    //                                  }).ToList());
                    //    ObservableCollection<RoundDetailsListTeamList> myCollection = new ObservableCollection<RoundDetailsListTeamList>(roundTeamsItemsList as List<RoundDetailsListTeamList>);
                    //    RoundTeamsItemsList = myCollection;
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}

                    //UserDialogs.Instance.HideLoading();
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

        #region Team Item Tabbed Command Functionality

        //To Hide and UnHide Players details Page
        private RoundDetailsListTeamList _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            try
            {
                var Item = parameter as RoundDetailsListTeamList;

                if (_LastSelectedItem == Item)
                {
                    Item.Expanded = !Item.Expanded;
                    await UpdateItems(Item);
                }
                else
                {
                    if (_LastSelectedItem != null)
                    {
                        //hide the previous selected item
                        _LastSelectedItem.Expanded = false;
                        await UpdateItems(_LastSelectedItem);
                    }
                    //Or show the selected item
                    Item.Expanded = true;
                    await UpdateItems(Item);
                }
                _LastSelectedItem = Item;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        async Task UpdateItems(RoundDetailsListTeamList Items)
        {
            try
            {
                var index = RoundTeamsItemsList.IndexOf(Items);
                RoundTeamsItemsList.Remove(Items);
                RoundTeamsItemsList.Insert(index, Items);
                OnPropertyChanged(nameof(RoundTeamsItemsList));
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }


        public ObservableCollection<RoundTeamItems> RoundTeamsItemsWithPlayers
        {
            get { return _RoundTeamsItemsWithPlayers; }
            set
            {
                _RoundTeamsItemsWithPlayers = value;
                OnPropertyChanged(nameof(RoundTeamsItemsWithPlayers));
            }
        }
        private ObservableCollection<RoundTeamItems> _RoundTeamsItemsWithPlayers = null;
        async void LoadTeamListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetTeamsList();

                    if (result != null)
                    {
                        RoundTeamsItemsWithPlayers = result;
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Team/listTeam";
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();

                    ////Assign the Values to Listview
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    RoundTeamsItemsWithPlayers = JsonConvert.DeserializeObject<ObservableCollection<RoundTeamItems>>(content);
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

        #endregion Team Item Tabbed Command Functionality
    }
}
