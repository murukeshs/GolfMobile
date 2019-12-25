using Acr.UserDialogs;
using Golf.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Golf.Services
{
    public class APIClient : IAPIClient
    {
        HttpClient _httpClient;

        string BaseURL = "https://golfapp.azurewebsites.net/api/";

        public APIClient()
        {
            _httpClient = new HttpClient();
            _httpClient.MaxResponseContentBufferSize = 256000;
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
        }

        // GetParticipantList
        public async Task<ObservableCollection<AllParticipantsResponse>> GetParticipantsList()
        {
            ObservableCollection<AllParticipantsResponse> response = null;
            try
            {
                var Url = BaseURL + "User/getPlayerList?SearchTerm=";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetRulesList
        public async Task<ObservableCollection<RoundRules>> GetRulesList()
        {
            ObservableCollection<RoundRules> response = null;
            try
            {
                var Url = BaseURL + "Round/getRoundRulesList";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<ObservableCollection<RoundRules>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

         //GetRoundsList
        public async Task<ObservableCollection<RoundList>> GetRoundsList()
            {
                ObservableCollection<RoundList> response = null;
                try
                {
                    var Url = BaseURL + "Round/getRoundList";
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var httpResponse = await _httpClient.GetAsync(Url);
                    var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        response = JsonConvert.DeserializeObject<ObservableCollection<RoundList>>(stringResponse);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(stringResponse);
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = $"An exception occurred: {ex.Message}";
                    UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
                return response;
            }

        //GetJoinRoundsList
        public async Task<ObservableCollection<roundJoinlist>> GetJoinRoundsList(int roundId, int userId)
        {
            ObservableCollection<roundJoinlist> response = null;
            try
            {
                var Url = BaseURL + "Round/getRoundJoinList?roundId=" + roundId + "&userId=" + userId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<ObservableCollection<roundJoinlist>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

         //GetRoundPlayersList
         public async Task<ObservableCollection<AllParticipantsResponse>> GetRoundPlayersList(int roundId, string action)
            {
                ObservableCollection<AllParticipantsResponse> response = null;
                try
                {
                    var Url = BaseURL + "Round/GetRoundPlayers?roundId=" + roundId + "&action=" + action;
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var httpResponse = await _httpClient.GetAsync(Url);
                    var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        response = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(stringResponse);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(stringResponse);
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = $"An exception occurred: {ex.Message}";
                    UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
                return response;
            }

        //GetRoundPlayersList
        public async Task<UserData> GetUserByUserId(int userId)
        {
            UserData response = null;
            try
            {
                var Url = BaseURL + "User/selectUserById/" + userId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<UserData>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetCountryList
        public async Task<List<Country>> GetCountryList()
        {
            List<Country> response = null;
            try
            {
                var Url = BaseURL + "Country/GetCountryList";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<List<Country>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetStateList
        public async Task<List<State>> GetStateList(int countryId)
        {
            List<State> response = null;
            try
            {
                var Url = BaseURL + "Country/GetStateList/" + countryId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<List<State>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetCompetitionTypeList
        public async Task<List<CompetitionType>> GetCompetitionTypeList()
        {
            List<CompetitionType> response = null;
            try
            {
                var Url = BaseURL + "Round/getCompetitionType";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<List<CompetitionType>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //SendRoundNotification
        public async Task<string> SendRoundNotification(int roundId)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Round/sendroundnotification/" + roundId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = stringResponse;
                }
                else
                { 
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetRoundDetailsByRoundId
        public async Task<ObservableCollection<getRoundsDetailsById>> GetRoundDetailsByRoundId(int roundId)
        {
            ObservableCollection<getRoundsDetailsById> response = null;
            try
            {
                var Url = BaseURL + "Round/getRoundDetailsById/" + roundId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<ObservableCollection<getRoundsDetailsById>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetTeamsList
        public async Task<ObservableCollection<RoundTeamItems>> GetTeamsList()
        {
            ObservableCollection<RoundTeamItems> response = null;
            try
            {
                var Url = BaseURL + "Team/listTeam";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<ObservableCollection<RoundTeamItems>>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetRoundByRoundId
        public async Task<getRoundById> GetRoundByRoundId(int roundId)
        {
            getRoundById response = null;
            try
            {
                var Url = BaseURL + "Round/getRoundById/" + roundId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<getRoundById>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

        //GetTeamByTeamId
        public async Task<TeamDetails> GetTeamByTeamId(int TeamId)
        {
            TeamDetails response = null;
            try
            {
                var Url = BaseURL + "Team/selectTeamById?teamId=" + TeamId;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.GetAsync(Url);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<TeamDetails>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An exception occurred: {ex.Message}";
                UserDialogs.Instance.Alert(errorMessage, "Alert", "Ok");
                UserDialogs.Instance.HideLoading();
            }
            return response;
        }

    }
}
