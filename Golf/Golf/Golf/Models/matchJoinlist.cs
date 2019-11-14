using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class matchJoinlist
    {
        public int userId { get; set; }
        public string ParticipantName { get; set; }
        public int ParticipantId { get; set; }

        public string matchName { get; set; }

        public int matchId { get; set; }

        public string Type { get; set; }

        public string matchFee { get; set; }

        public string CompetitionName { get; set; }

        public string competitionTypeId { get; set; }
    }
}
