using Golf.Models;
using Golf.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace Golf.Views.RoundDetailsView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundPlayerDetailsPopup : PopupPage
    {
        public string ProfileImageValue = string.Empty;
        public string PlayerNameValue = string.Empty;
        public string HCP
        {
            get { return _HCP; }
            set
            {
                _HCP = value;
                OnPropertyChanged(nameof(HCP));
            }
        }
        private string _HCP = "10";

        public string NickName
        {
            get { return _NickName; }
            set
            {
                _NickName = value;
                OnPropertyChanged(nameof(NickName));
            }
        }
        private string _NickName = string.Empty;

        public string EmailValue = string.Empty;
        public string PhoneNumberValue = string.Empty;
        public string GenderValue = string.Empty;

        public RoundPlayerDetailsPopup(AllParticipantsResponse item)
        {
            InitializeComponent();
            BindingContext = this;
            ProfileImageValue = item.profileImage;
            if(string.IsNullOrEmpty(ProfileImageValue))
            {
                ProfileImageValue = "profile_defalut_pic.png";
            }
            else
            {
                ProfileImageValue = item.profileImage;
            }
            ProfileImage.Source = ProfileImageValue;
            PlayerName.Text = item.playerName;
            Gender.Text = item.gender;
            Email.Text = item.email;
            PhoneNumber.Text = "999-444-0000";
            HCP = "10";
            NickName = item.nickName;
        }

        private async void RoundedButton_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}