using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class UpdateTeamModel
    {
        public int teamId { get; set; }

        public string teamName { get; set; }

        public string teamIcon { get; set; }

        public string createdOn { get; set; }

        public int createdBy { get; set; }

        public int startingHole { get; set; }

        public int roundId { get; set; }
    }
}
