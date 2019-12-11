using Golf.ViewModel;
using System.Collections.Generic;

namespace Golf.Models
{
    public class getRoundsDetailsById
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string teamIcon { get; set; }
        public string scoreKeeperName { get; set; }
        public string roundPlayerList { get; set; }
        public string NoOfPlayers { get; set; }
        public string createdByName { get; set; }
    }

    public class roundPlayerList
    {
        public int userId { get; set; }

        public string playerName { get; set; }

        public bool isInvitationAccept { get; set; }

        public bool isPaymentMade { get; set; }

        public string profileImage { get; set; }
    }

    public class RoundDetailsListTeamList : BaseViewModel
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string teamIcon { get; set; }
        public string scoreKeeperName { get; set; }

        public string NoOfPlayers { get; set; }
        public string createdByName { get; set; }

        public List<roundPlayerList> roundPlayerList { get; set; }

        private bool _expanded;

        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get
            {
                if (Expanded)
                {
                    return "arrow_up.png";
                }
                else
                {
                    return "arrow_down.png";
                }
            }
        }
    }
}
