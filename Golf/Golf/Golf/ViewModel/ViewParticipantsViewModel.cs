using Acr.UserDialogs;
using Golf.Models;
using Golf.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ViewParticipantsViewModel : BaseViewModel
    {

        public ObservableCollection<ParticipantList> ParticipantItems
        {
            get { return _ParticipantItems; }
            set
            {
                _ParticipantItems = value;
                OnPropertyChanged(nameof(ParticipantItems));
            }
        }
        private ObservableCollection<ParticipantList> _ParticipantItems = null;
       
        public ICommand ItemTappedCommand { get; private set; }

        public ViewParticipantsViewModel()
        {
            var item1 = new ObservableCollection<ParticipantTypes>()
            {
                new ParticipantTypes{ParticipantType = "Player"},
                new ParticipantTypes{ParticipantType = "Moderator"},
                new ParticipantTypes{ParticipantType = "Score Keeper"},
                new ParticipantTypes{ParticipantType = "Organizer"},
                new ParticipantTypes{ParticipantType = "Spectator"}
            };

            var item2 = new ObservableCollection<ParticipantTypes>()
            {
                new ParticipantTypes{ParticipantType = "Player"},
                new ParticipantTypes{ParticipantType = "Moderator"}
            };

            var item3 = new ObservableCollection<ParticipantTypes>()
            {
                new ParticipantTypes{ParticipantType = "Player"},
                new ParticipantTypes{ParticipantType = "Moderator"},
                new ParticipantTypes{ParticipantType = "Spectator"}
            };

            ParticipantItems = new ObservableCollection<ParticipantList>(new[]
              {
                    new ParticipantList { ParticipantName = "John Mic" ,ParticipantEmail = "johnmic@gmail.com",TypeList= item1 },
                    new ParticipantList { ParticipantName = "Charlotte Mia" ,ParticipantEmail = "Charlotte@gmail.com",TypeList= item2 },
                    new ParticipantList { ParticipantName = "MsDhoni" ,ParticipantEmail = "msd@gmail.com", TypeList= item3 },
                    new ParticipantList { ParticipantName = "John Mic" ,ParticipantEmail = "johnmic@gmail.com",TypeList= item1 },
                    new ParticipantList { ParticipantName = "Charlotte Mia" ,ParticipantEmail = "Charlotte@gmail.com",TypeList= item2 },
                    new ParticipantList { ParticipantName = "MsDhoni" ,ParticipantEmail = "msd@gmail.com", TypeList= item3 }
              });

            ItemTappedCommand = new Command<ParticipantList>(GetViewParticipantsDetails);
        }

        async void GetViewParticipantsDetails(ParticipantList obj)
        {
            try
            {
                var msg = "Player Name is" + obj.ParticipantName;
                UserDialogs.Instance.Alert(msg, "View Participants", "Ok");
            }
            catch(Exception ex)
            {

            }
        }
    }
}
