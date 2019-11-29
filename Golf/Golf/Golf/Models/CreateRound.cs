using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class CreateRound
    {
        public string roundName { get; set; }
        public string roundType { get; set; }

        public string roundCode { get; set; }

        public string roundRuleId { get; set; }

        public string roundStartDate { get; set; }
        public string roundEndDate { get; set; }

        public int roundFee { get; set; }

        public string roundLocation { get; set; }

        public int createdBy { get; set; }

        public int competitionTypeId { get; set; }
        public int roundId { get; set; }

        public string roundStatus { get; set; }

        public bool isSaveAndNotify { get; set; }
    }
}
