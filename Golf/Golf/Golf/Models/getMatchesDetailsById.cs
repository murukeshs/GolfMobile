using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class getMatchesDetailsById
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string teamIcon { get; set; }
        public string scoreKeeperName { get; set; }
        public string matchPlayerList { get; set; }
    }

    public class matchPlayerList
    {
        public int userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public bool isInvitationAccept { get; set; }

        public bool isPaymentMade { get; set; }

        public string profileImage { get; set; }
    }

    public class MatchDetailsListTeamList : BaseViewModel
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string teamIcon { get; set; }
        public string scoreKeeperName { get; set; }
        public List<matchPlayerList> matchPlayerList { get; set; }

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
                    return "arrow_down.png";
                }
                else
                {
                    return "arrow_up.png";
                }
            }
        }
    }
}
