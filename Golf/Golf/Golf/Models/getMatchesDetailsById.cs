using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class getMatchesDetailsById
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string teamIcon { get; set; }
        public string scoreKeeperName { get; set; }
        public ObservableCollection<playerList> Items { get; set; }
    }

    public class playerList
    {
        public int userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string gender { get; set; }

        public string dob { get; set; }

        public string phoneNumber { get; set; }

        public string countryId { get; set; }
    }
}
