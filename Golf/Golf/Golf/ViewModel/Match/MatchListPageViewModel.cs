using Acr.UserDialogs;
using Golf.Models;
using Golf.Utils;
using Golf.Views.MatchDetailsView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchListPageViewModel : BaseViewModel
    {
        public ObservableCollection<MatchList> MatchListItems
        {
            get { return _MatchListItems; }
            set
            {
                _MatchListItems = value;
                OnPropertyChanged(nameof(MatchListItems));
            }
        }
        private ObservableCollection<MatchList> _MatchListItems = null;

        public MatchListPageViewModel()
        {
            MatchListItems = new ObservableCollection<MatchList>(new[]
             {
                    new MatchList { MatchName  = "Match 1", MatchCode = "ABS12345" ,MatchType = "Player",MatchStartDate= "11-05-2019" ,MatchFee = "$20"},
                    new MatchList { MatchName  = "Match 2", MatchCode = "12345678" ,MatchType = "Teams",MatchStartDate= "05-05-2019" ,MatchFee = "$10"},
                    new MatchList { MatchName  = "Match 3", MatchCode = "ABS12345" ,MatchType = "Player",MatchStartDate= "07-07-2019" ,MatchFee = "$50"},
                    new MatchList { MatchName  = "Match 4", MatchCode = "12345678" ,MatchType = "Player",MatchStartDate= "20-08-2019" ,MatchFee = "$30"},
                    new MatchList { MatchName  = "Match 5", MatchCode = "ABS12345" ,MatchType = "Teams",MatchStartDate= "22-09-2019" ,MatchFee = "$40"},
                    new MatchList { MatchName  = "Match 6", MatchCode = "12345678" ,MatchType = "Teams",MatchStartDate= "11-05-2019" ,MatchFee = "$70"},
                    new MatchList { MatchName  = "Match 7", MatchCode = "ABS12345" ,MatchType = "Player",MatchStartDate= "11-05-2019" ,MatchFee = "$100"},
              });
        }

        #region List ItemTabbed Command Functionality
        public ICommand ListItemTabbedCommand => new Command(HandleItemSelected);

        private async void HandleItemSelected(object parameter)
        {
            //ViewModelNavigation.PushAsync(new ItemPageViewModel() { Item = parameter as RssItem });
            UserDialogs.Instance.ShowLoading();
            var Item = parameter as MatchList;
            var name = Item.MatchName;
            var view = new MatchDetailsPage();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }
        #endregion List ItemTabbed Command Functionality
    }
}
