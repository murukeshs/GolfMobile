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
            public string UserName
            {
                get { return _UserName; }
                set
                {
                    _UserName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
            public string _UserName = App.User.UserName;

            public string UserEmail
            {
                get { return _UserEmail; }
                set
                {
                    _UserEmail = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
            public string _UserEmail = App.User.UserEmail;

            public string UserProfilePic
            {
                get { return _UserProfilePic; }
                set
                {
                    _UserProfilePic = value;
                    OnPropertyChanged(nameof(UserProfilePic));
                }
            }
            public string _UserProfilePic = null;

            public ObservableCollection<MenuPageMenuItem> MenuItems { get; set; }
            
            public MenuPageMasterViewModel()
            {
                if (!string.IsNullOrEmpty(App.User.UserProfileImage))
                {
                    UserProfilePic = App.User.UserProfileImage;
                }
                else
                {
                    UserProfilePic = "profile_defalut_pic.png";
                }
                UserName = App.User.UserName;
                UserEmail = App.User.UserEmail;

                if (App.User.IsModerator)
                {
                    MenuItems = new ObservableCollection<MenuPageMenuItem>(new[]
                    {
                    new MenuPageMenuItem { Id = 0, Title = "Home" ,TargetType = typeof(HomePage),Icon = "home.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 1, Title = "My Profile" ,TargetType = typeof(ProfilePage),Icon = "profile.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Team" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "List of Teams" ,TargetType = typeof(ListOfTeamsPage),Icon = "listofteams.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Create a Team" ,TargetType = typeof(CreateTeamPage),Icon = "createteam.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Match" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "List of Matches" ,TargetType = typeof(MatchListPage),Icon = "listofteams.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Create a Match" ,TargetType = typeof(CreateMatchPage),Icon = "createteam.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Participant" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Invite a Participant" ,TargetType = typeof(InviteParticipantPage),Icon = "inviteaparticipant.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "View Participants" ,TargetType = typeof(ViewAllParticipantsPage),Icon = "viewparticipant.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Logout" ,Icon = "viewparticipant.png",TargetType = typeof(LoginPage),IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]}
                });
                }
                else
                {
                    MenuItems = new ObservableCollection<MenuPageMenuItem>(new[]
                    {
                    new MenuPageMenuItem { Id = 0, Title = "Home" ,TargetType = typeof(HomePage),Icon = "home.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 1, Title = "My Profile" ,TargetType = typeof(ProfilePage),Icon = "profile.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Team" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "List of Teams" ,TargetType = typeof(ListOfTeamsPage),Icon = "listofteams.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Create a Team" ,TargetType = typeof(CreateTeamPage),Icon = "createteam.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Match" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "List of Matches" ,TargetType = typeof(MatchListPage),Icon = "listofteams.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Participant" ,TargetType = typeof(HomePage),Icon = "",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["TextGrayColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Invite a Participant" ,TargetType = typeof(InviteParticipantPage),Icon = "inviteaparticipant.png",IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "View Participants" ,TargetType = typeof(ViewAllParticipantsPage),Icon = "viewparticipant.png",IsUnderlined = true,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]},
                    new MenuPageMenuItem { Id = 2, Title = "Logout" ,Icon = "viewparticipant.png",TargetType = typeof(LoginPage),IsUnderlined = false,Indentation = new Thickness(5,0,0,0),TextColor = (Color)App.Current.Resources["BlackColor"]}
                    });
                }
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