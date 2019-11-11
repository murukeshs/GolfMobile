using Acr.UserDialogs;
using Golf.Models;
using Golf.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchDetailsPageViewModel : BaseViewModel
    {
        //To Hide and UnHide Math details Page
        public bool IsVisibleMatchDetails
        {
            get { return _IsVisibleMatchDetails; }
            set
            {
                _IsVisibleMatchDetails = value;
                OnPropertyChanged(nameof(IsVisibleMatchDetails));
            }
        }
        private bool _IsVisibleMatchDetails = true;

        //To Hide and UnHide Teams details Page
        public bool IsVisibleTeamsDetails
        {
            get { return _IsVisibleTeamsDetails; }
            set
            {
                _IsVisibleTeamsDetails = value;
                OnPropertyChanged(nameof(IsVisibleTeamsDetails));
            }
        }
        private bool _IsVisibleTeamsDetails = false;

        //To Hide and UnHide Players details Page
        public bool IsVisiblePlayersDetails
        {
            get { return _IsVisiblePlayersDetails; }
            set
            {
                _IsVisiblePlayersDetails = value;
                OnPropertyChanged(nameof(IsVisiblePlayersDetails));
            }
        }
        private bool _IsVisiblePlayersDetails = false;

        public ObservableCollection<MatchDetailsPlayersList> MatchPlayersItems
        {
            get { return _MatchPlayersItems; }
            set
            {
                _MatchPlayersItems = value;
                OnPropertyChanged(nameof(MatchPlayersItems));
            }
        }
        private ObservableCollection<MatchDetailsPlayersList> _MatchPlayersItems = null;


        public ObservableCollection<MatchTeamList> MatchTeamsItems
        {
            get { return _MatchTeamsItems; }
            set
            {
                _MatchTeamsItems = value;
                OnPropertyChanged(nameof(MatchTeamsItems));
            }
        }
        private ObservableCollection<MatchTeamList> _MatchTeamsItems = null;

        public ObservableCollection<MatchTeamList> NewMatchTeamsItems = new ObservableCollection<MatchTeamList>();



        public MatchDetailsPageViewModel()
        {
            MatchPlayersItems = new ObservableCollection<MatchDetailsPlayersList>(new[]
            {
                    new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 1", Id = "ABS12345" ,LinkApproved = true,PaymentSuccess = true},
                    new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 2", Id = "12345" ,LinkApproved = false,PaymentSuccess = true},
                    new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 3", Id = "ABS" ,LinkApproved = true,PaymentSuccess = false},
                    new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 4", Id = "ABS345" ,LinkApproved = true,PaymentSuccess = true},
                    new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 5", Id = "ABS12345" ,LinkApproved = false,PaymentSuccess = false},
            });


            MatchTeamsItems = new ObservableCollection<MatchTeamList>(new[]
            {
                    new MatchTeamList {
                        MatchTeamName = "Golf 1" , MatchCreaedBy = "Rsk" ,MatchNoofPlayers = "10",IsVisible = false,
                        Items = new ObservableCollection<MatchDetailsPlayersList>{
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name = "Name 1", Id = "ABS12345", LinkApproved = true, PaymentSuccess = true },
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 2", Id = "12345" ,LinkApproved = false,PaymentSuccess = true},
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 3", Id = "ABS" ,LinkApproved = true,PaymentSuccess = false},
                        }
                    },
                    new MatchTeamList {
                        MatchTeamName = "Golf 2" , MatchCreaedBy = "Rsk" ,MatchNoofPlayers = "7",IsVisible = false,
                        Items = new ObservableCollection<MatchDetailsPlayersList>{
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name = "Name 1", Id = "ABS12345", LinkApproved = true, PaymentSuccess = true },
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 2", Id = "12345" ,LinkApproved = false,PaymentSuccess = true},
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 3", Id = "ABS" ,LinkApproved = true,PaymentSuccess = false},
                        }
                    },
                    new MatchTeamList {
                        MatchTeamName = "Golf 3" , MatchCreaedBy = "Rsk" ,MatchNoofPlayers = "12",IsVisible = false,
                        Items = new ObservableCollection<MatchDetailsPlayersList>{
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name = "Name 1", Id = "ABS12345", LinkApproved = true, PaymentSuccess = true },
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 2", Id = "12345" ,LinkApproved = false,PaymentSuccess = true},
                            new MatchDetailsPlayersList { Profile = "profile_pic.png", Name  = "Name 3", Id = "ABS" ,LinkApproved = true,PaymentSuccess = false},
                        }
                    },
            });
        }

        #region Match Details Button Command Functionality
        public ICommand MatchDetailsCommand => new AsyncCommand(MatchDetailsAsync);

        async Task MatchDetailsAsync()
        {
            IsVisibleMatchDetails = true;
            IsVisiblePlayersDetails = false;
            IsVisibleTeamsDetails = false;
        }
        #endregion Match Details Button Command Functionality

        #region Players Button Command Functionality
        public ICommand PlayersCommand => new AsyncCommand(PlayersAsync);

        async Task PlayersAsync()
        {
            IsVisibleMatchDetails = false;
            IsVisiblePlayersDetails = true;
            IsVisibleTeamsDetails = false;
        }
        #endregion Players Button Command Functionality

        #region Team Button Command Functionality

        public ICommand TeamCommand => new AsyncCommand(TeamAsync);

        async Task TeamAsync()
        {
            UserDialogs.Instance.ShowLoading();
            IsVisibleMatchDetails = false;
            IsVisiblePlayersDetails = false;
            IsVisibleTeamsDetails = true;
            UserDialogs.Instance.HideLoading();
        }


        #endregion Team Button Command Functionality

        #region Team Item Tabbed Command Functionality

        //To Hide and UnHide Players details Page
        private MatchTeamList _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            var Item = parameter as MatchTeamList;

            if(_LastSelectedItem == Item)
            {
                Item.IsVisible = !Item.IsVisible;
                await UpdateItems(Item);
            }
            else
            {
                if(_LastSelectedItem != null)
                {
                    //hide the previous selected item
                    _LastSelectedItem.IsVisible = false;
                    await UpdateItems(_LastSelectedItem);
                }
                //Or show the selected item
                Item.IsVisible = true;
                await UpdateItems(Item);
            }
            _LastSelectedItem = Item;
        }

        async Task UpdateItems(MatchTeamList Items)
        {
            var index = MatchTeamsItems.IndexOf(Items);
            MatchTeamsItems.Remove(Items);
            MatchTeamsItems.Insert(index, Items);
            OnPropertyChanged(nameof(MatchTeamsItems));
        }
        #endregion Team Item Tabbed Command Functionality

    }
}
