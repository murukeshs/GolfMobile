using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class RoundTeamItems
    {
        public int teamId { get; set; }

        public int scoreKeeperID { get; set; }

        public int noOfPlayers { get; set; }

        public string createdName { get; set; }

        public string teamName { get; set; }

        public string teamIcon { get; set; }

        public int createdBy { get; set; }

        public string createdOn { get; set; }

        public string startingHole { get; set; }

        public string teamplayerList { get; set; }
    }

    public class teamplayerList
    {
        public int userId { get; set; }

        public string username { get; set; }

        public string gender { get; set; }

        public string userType { get; set; }

        public string profileImage { get; set; }
    }

    public class RoundTeamWithPlayers : BaseViewModel
    {
        public int teamId { get; set; }

        public int scoreKeeperID { get; set; }

        public int noOfPlayers { get; set; }

        public string createdName { get; set; }

        public string teamName { get; set; }

        public string teamIcon { get; set; }

        public int createdBy { get; set; }

        public string createdOn { get; set; }

        public string startingHole { get; set; }

        public List<teamplayerList> teamplayerList { get; set; }

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

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        public bool _IsChecked = false;
    }
}
