using Golf.Models;
using Golf.Models.userModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Golf.Services
{
    public interface IAPIClient
    {
        // Get API Methods
        Task<ObservableCollection<AllParticipantsResponse>> GetParticipantsList();

        Task<ObservableCollection<RoundRules>> GetRulesList();

        Task<ObservableCollection<RoundList>> GetRoundsList();

        Task<ObservableCollection<roundJoinlist>> GetJoinRoundsList(int roundId, int userId);

        Task<ObservableCollection<AllParticipantsResponse>> GetRoundPlayersList(int roundId, string action);

        Task<UserData> GetUserByUserId(int userId);

        Task<List<Country>> GetCountryList();

        Task<List<State>> GetStateList(int countryId);

        Task<List<CompetitionType>> GetCompetitionTypeList();

        Task<string> SendRoundNotification(int roundId);

        Task<ObservableCollection<getRoundsDetailsById>> GetRoundDetailsByRoundId(int roundId);

        Task<ObservableCollection<RoundTeamItems>> GetTeamsList();

        Task<getRoundById> GetRoundByRoundId(int roundId);

        Task<TeamDetails> GetTeamByTeamId(int TeamId);

        // Put API Methods

        Task<string> JoinRound(acceptRoundInvitation data);

        Task<string> UpdateUserCommunicationInfo(createUser data);

        Task<string> UpdateUser(createUser data);

        Task<string> CreateTeamPlayers(TeamPlayer data);

        Task<string> GenerateOTP(GenerateOTPEmail data);

        Task<string> UpdatePassword(UpdatePassword data);

        Task<string> VerifyOTP(VerifyOTP data);

        Task<string> UpdateRound(CreateRound data);

        Task<string> UpdateTeam(UpdateTeamModel data);

        // Post API Methods

        Task<string> InviteParticipant(createUser data);

        Task<string> AddRoundRule(Rule data);

        Task<CreateTeamResponse> CreateTeam(TeamModel data);

        Task<LoginResponse> Login(Login data);

        Task<RegisterResponse> CreateUser(createUser data);

        Task<createRoundResponse> CreateRound(CreateRound data);

        Task<string> AddRoundPlayer(RoundPlayers data);

        Task<string> CreateRoundPlayer(CreateRoundPlayers data);

        Task<string> UploadFile(byte[] data);

    } 
}
