using Acr.UserDialogs;
using Golf.Models;
using Golf.Utils;
using Golf.Views.CreateRoundView;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class ListofPlayersPageViewModel : BaseViewModel
    {
        #region PropertyDeclaration

        public ObservableCollection<PlayersList> ParticipantItems
        {
            get { return _ParticipantItems; }
            set
            {
                _ParticipantItems = value;
                OnPropertyChanged(nameof(ParticipantItems));
            }
        }
        private ObservableCollection<PlayersList> _ParticipantItems = new ObservableCollection<PlayersList>();

        #endregion

        #region Next ImageButton Command Functionality

        public ICommand NextImageButtonCommand => new AsyncCommand(NextImageButtonAsync);

        async Task NextImageButtonAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new SendInvitePoppup();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Next ImageButton Command Functionality
    }
}
