using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class createRoundResponse
    {
        public int roundId{ get; set; }

        public string roundCode { get; set; }

        public string competitionTypeName { get; set; }
    }
}
