using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class AllParticipants
    {
        public int teamPlayerListId { get; set; }

        public int teamId { get; set; }

        public string playerName { get; set; }

        public string gender { get; set; }

        public string email { get; set; }

        public string roleType { get; set; }

        public string profileImage { get; set; }
    }
}
