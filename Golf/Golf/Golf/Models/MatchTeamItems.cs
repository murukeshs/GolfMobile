using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class MatchTeamItems
    {
        public int teamId { get; set; }

        public int scoreKeeperID { get; set; }

        public int noOfPlayers { get; set; }

        public string teamName { get; set; }

        public string teamIcon { get; set; }
        public string createdName { get; set; }
        public string createdOn { get; set; }
        public string startingHole { get; set; }
    }
}
