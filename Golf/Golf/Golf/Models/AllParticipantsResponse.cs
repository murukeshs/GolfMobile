using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class AllParticipantsResponse 
    {
        public int userId { get; set; }
        public string playerName { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string userType { get; set; }
        public bool isScoreKeeper { get; set; }
    }
}
