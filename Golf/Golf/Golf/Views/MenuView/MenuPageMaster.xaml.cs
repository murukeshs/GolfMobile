using Golf.Views.CreateMatchView;
using Golf.Views.MatchDetailsView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.MenuView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPageMaster : ContentPage
    {
        public ListView ListView;

        public MenuPageMaster()
        {
            InitializeComponent();

            BindingContext = new MenuPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        public class MenuPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuPageMenuItem> MenuItems { get; set; }
            
            public MenuPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuPageMenuItem>(new[]
                {
                    new MenuPageMenuItem { Id = 0, Title = "Home" ,TargetType = typeof(HomePage),Icon = "home.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 1, Title = "My Profile" ,TargetType = typeof(ProfilePage),Icon = "profile.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Team" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "List of Teams" ,TargetType = typeof(MatchTeamListPage),Icon = "listofteams.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Create a Team" ,TargetType = typeof(CreateTeamPage),Icon = "createteam.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Match" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "List of Matches" ,TargetType = typeof(MatchListPage),Icon = "listofteams.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Create a Match" ,TargetType = typeof(CreateMatchPage),Icon = "createteam.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Participant" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Invite a Participant" ,TargetType = typeof(InviteParticipantPage),Icon = "inviteaparticipant.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "View Participants" ,TargetType = typeof(ViewParticipantPage),Icon = "viewparticipant.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]}
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}