using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class AddPlayersList
    {
        public string PlayerName { get; set; }

        public string PlayerHCP { get; set; }

        public string PlayerType { get; set; }
        public bool IsStoreKeeper { get; set; }
    }
}
