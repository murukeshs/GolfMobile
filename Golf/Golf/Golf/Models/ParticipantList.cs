using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class ParticipantList
    {
        public string ParticipantName { get; set; }

        public string ParticipantEmail { get; set; }

        public ObservableCollection<ParticipantTypes> TypeList { get; set; }
    }

    public class ParticipantTypes
    {
        public string ParticipantType { get; set; }
    }
}
