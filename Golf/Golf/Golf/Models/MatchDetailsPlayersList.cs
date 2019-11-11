using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class MatchDetailsPlayersList
    {
        public string Profile { get; set; }
        public string Name { get; set; }

        public string Id { get; set; }

        public bool LinkApproved { get; set; } 

        public bool PaymentSuccess { get; set; } 
    }

    public class MatchTeamList
    {
        public string MatchTeamName { get; set; }

        public string MatchCreaedBy { get; set; }

        public string MatchNoofPlayers { get; set; }

        public bool IsVisible { get; set; } 

        public ObservableCollection<MatchDetailsPlayersList> Items { get; set; }
    }
}
