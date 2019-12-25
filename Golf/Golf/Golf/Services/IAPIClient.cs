using Golf.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Golf.Services
{
    public interface IAPIClient
    {
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
    } 
}
