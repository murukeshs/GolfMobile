using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class acceptMatchInvitation
    {
        public int matchId { get; set; }

        public string type { get; set; }

        public int playerId { get; set; }
    }
}
