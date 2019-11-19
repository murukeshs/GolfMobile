using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class UserModel
    {
        internal string BaseUrl = "https://golfapp.azurewebsites.net/api/"; 

        internal string AccessToken { get; set; } = string.Empty;

        internal string UserName { get; set; } = string.Empty;

        internal string UserProfileImage { get; set; } = string.Empty;

        internal int UserId { get; set; } = 0;

        internal int UserWithTypeId { get; set; } = 0;

        internal string UserEmail { get; set; } = string.Empty;

        internal int CreateTeamId { get; set; } = 0;

        internal string MatchCode { get; set; } = string.Empty;

        internal string CompetitionType { get; set; } = string.Empty;

        internal int CreateMatchId { get; set; } = 0;

        internal int TeamIdforPlayerListing { get; set; } = 0;

        internal string MatchId { get; set; } = string.Empty;

        internal bool IsModerator { get; set; } = false;

        public ObservableCollection<AddPlayersList> TeamPreviewList { get; set; } = new ObservableCollection<AddPlayersList>();
    }
}
