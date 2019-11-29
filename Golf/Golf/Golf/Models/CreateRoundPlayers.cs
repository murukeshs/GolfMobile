using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class CreateRoundPlayers
    {
        public string type { get; set; }

        public int eventId { get; set; }

        public string teamId { get; set; }

        public string playerId { get; set; }
    }
}
