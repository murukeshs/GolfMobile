using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class createMatchResponse
    {
        public int matchId{ get; set; }
        public string matchCode { get; set; }

        public string competitionTypeName { get; set; }
    }
}
