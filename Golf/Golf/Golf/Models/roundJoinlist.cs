using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class roundJoinlist
    {
        public int userId { get; set; }
        public string ParticipantName { get; set; }
        public int ParticipantId { get; set; }

        public string roundName { get; set; }

        public int roundId { get; set; }

        public string Type { get; set; }

        public string roundFee { get; set; }

        public string CompetitionName { get; set; }

        public string competitionTypeId { get; set; }

        public bool isAllowRound { get; set; }
    }
}
