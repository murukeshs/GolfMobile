using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class TeamDetails
    {
        public int teamId { get; set; }

        public string teamName { get; set; }

        public string teamIcon { get; set; }

        public string CreatedOn { get; set; }

        public int createdBy { get; set; }

        public int startingHole { get; set; }

        public string createdByName { get; set; }

        public int scoreKeeperId { get; set; }

        public List<TeamPlayerDetails> TeamPlayerDetails { get; set; }
    }

    public class TeamPlayerDetails : BaseViewModel
    { 
        public int teamPlayerListId { get; set; }

        public int teamId { get; set; }

        public string playerName { get; set; }

        public string profileImage { get; set; }

        public string gender { get; set; }

        public string email { get; set; }

        public string RoleType { get; set; }

        public string nickName { get; set; }

        public bool isChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(isChecked));
            }
        }
        private bool _isChecked { get; set; }

        public int userId { get; set; }

        public string ImageIcon
        {
            get { return _ImageIcon; }
            set
            {
                _ImageIcon = value;
                OnPropertyChanged(nameof(ImageIcon));
            }
        }
        private string _ImageIcon { get; set; } = "unchecked_icon.png";

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
        private bool _IsChecked { get; set; } = false;
    }
}
