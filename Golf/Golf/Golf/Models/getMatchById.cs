using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class getMatchById
    {
        public int matchId { get; set; }
        public string matchCode { get; set; }
        public string matchName { get; set; }

        public string matchRuleId { get; set; }
        public string ruleName { get; set; }
        public string matchStartDate { get; set; }
        public string matchEndDate { get; set; }
        public string matchLocation { get; set; }
        public int createdBy { get; set; }
        public string createdDate { get; set; }
        public string matchStatus { get; set; }
        public int competitionTypeId { get; set; }
        public string competitionName { get; set; }

    }
}
