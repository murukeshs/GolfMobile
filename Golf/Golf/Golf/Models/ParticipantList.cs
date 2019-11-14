using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class ParticipantList
    {
        public int userId { get; set; }

        public string playerName { get; set; }

        public string gender { get; set; }

        public string email { get; set; }

        public ObservableCollection<userType> TypeList { get; set; }

        public bool isScoreKeeper { get; set; }
    }

    public class userType
    {
        public string ParticipantType { get; set; }
    }
}
