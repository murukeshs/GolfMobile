using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class getRoundById
    {
        public int roundId { get; set; }
        public string roundCode { get; set; }
        public string roundName { get; set; }

        public string roundRuleId { get; set; }
        public string ruleName { get; set; }
        public string roundStartDate { get; set; }
        public string roundEndDate { get; set; }
        public string roundLocation { get; set; }
        public int createdBy { get; set; }
        public string createdDate { get; set; }
        public string roundStatus { get; set; }
        public int competitionTypeId { get; set; }
        public string competitionName { get; set; }

    }
}
