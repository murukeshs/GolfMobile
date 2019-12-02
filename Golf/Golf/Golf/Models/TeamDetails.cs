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

        public List<TeamPlayerDetails> TeamPlayerDetails { get; set; }
    }

    public class TeamPlayerDetails
    { 
        public int teamPlayerListId { get; set; }

        public int teamId { get; set; }

        public string playerName { get; set; }

        public string profileImage { get; set; }

        public string gender { get; set; }

        public string email { get; set; }

        public string RoleType { get; set; }
    }
}
