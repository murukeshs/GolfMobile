using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class AllParticipantsResponse : BaseViewModel
    {
        public int userId { get; set; }
        public string playerName { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string roleType { get; set; }
        public string userType { get; set; }
        public bool isScoreKeeper { get; set; }
        public string profileImage { get; set; }
    }

    public class userTypeList
    {
        public string RoleName { get; set; }
    }
}
