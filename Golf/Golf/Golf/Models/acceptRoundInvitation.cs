using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class acceptRoundInvitation
    {
        public int roundId { get; set; }

        public string type { get; set; }

        public int playerId { get; set; }
    }
}
