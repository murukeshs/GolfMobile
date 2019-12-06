using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Golf.Models
{
    public class UserModel
    {
        internal string BaseUrl = "https://golfapp.azurewebsites.net/api/";

        internal string EVENT_REFRESH_PROFILE_ICON = "EVENT_REFRESH_PROFILE_ICON";

        internal string ISPLAYERLISTREFRESH = "false";

        internal string ISPARTICIPANTLISTREFRESH = "false";

        internal string AccessToken { get; set; } = string.Empty;

        internal string UserName { get; set; } = string.Empty;

        internal string UserProfileImage { get; set; } = string.Empty;

        internal int UserId { get; set; } = 0;

        internal int UserWithTypeId { get; set; } = 0;

        internal string UserEmail { get; set; } = string.Empty;

        internal int CreateTeamId { get; set; } = 0;

        internal string RoundCode { get; set; } = string.Empty;

        internal string CompetitionType { get; set; } = string.Empty;

        internal int CreateRoundId { get; set; } = 0;

        internal int TeamIdforPlayerListing { get; set; } = 0;

        internal string RoundId { get; set; } = string.Empty;

        internal bool IsModerator { get; set; } = false;

        internal string OtpEmail { get; set; }

        internal string TeamName { get; set; }

        public ObservableCollection<AddPlayersList> TeamPreviewList { get; set; } = new ObservableCollection<AddPlayersList>();

        public ObservableCollection<AllParticipantsResponse> PlayersList { get; set; } = new ObservableCollection<AllParticipantsResponse>();

        internal string TeamPreviewName { get; set; }

        internal string TeamPreviewScoreKeeperName { get; set; }

        internal string TeamPreviewScoreKeeperProfilePicture { get; set; }

        internal bool FromEmailNotValid { get; set; } = false;

        internal bool IsRadioButton { get; set; } = false;

        internal int ScoreKeeperId { get; set; }

        public ObservableCollection<AddPlayersList> PlayersPreviewList { get; set; } = new ObservableCollection<AddPlayersList>();

    }
}
