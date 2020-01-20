using Acr.UserDialogs;
using Golf.Models;
using Golf.Models.userModel;
using Golf.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Golf.Services
{
    public class APIClient : IAPIClient
    {
        HttpClient _httpClient;

        string BaseURL = "https://golfapp.azurewebsites.net/api/";

        public APIClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMilliseconds(360000);
            _httpClient.MaxResponseContentBufferSize = 2147483647;

           // _httpClient.MaxResponseContentBufferSize = 256000;
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

        // Post JoinRound 
        public async Task<string> JoinRound(acceptRoundInvitation data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Round/acceptRoundInvitation";
                var json = JsonConvert.SerializeObject(data);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(Url, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // Update UserCommunicationInfo 
        public async Task<string> UpdateUserCommunicationInfo(createUser data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "User/updateUserCommunicationinfo";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // UpdateUser 
        public async Task<string> UpdateUser(createUser data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "User/updateUser";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // CreateTeamPlayers 
        public async Task<string> CreateTeamPlayers(TeamPlayer data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Team/createTeamPlayers";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // GenerateOTP 
        public async Task<string> GenerateOTP(GenerateOTPEmail data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "User/generateOTP";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // VerifyOTP 
        public async Task<string> VerifyOTP(VerifyOTP data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "User/verifyOTP";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // UpdatePassword 
        public async Task<string> UpdatePassword(UpdatePassword data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "User/updatePassword";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // UpdateRound 
        public async Task<string> UpdateRound(CreateRound data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Round/updateRound";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // UpdateTeam 
        public async Task<string> UpdateTeam(UpdateTeamModel data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Team/updateTeam";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // Post InviteParticipant 
        public async Task<string> InviteParticipant(createUser data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "User/inviteParticipant";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // Post AddRoundRule 
        public async Task<string> AddRoundRule(Rule data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Round/roundRules";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // Post CreateTeam 
        public async Task<CreateTeamResponse> CreateTeam(TeamModel data)
        {
            CreateTeamResponse response = null;
            try
            {
                var Url = BaseURL + "Team/createTeam";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<CreateTeamResponse>(stringResponse); 
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

        // Post Login 
        public async Task<LoginResponse> Login(Login data)
        {
            LoginResponse response = null;
            try
            {
                var Url = BaseURL + "JWTAuthentication/login";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<LoginResponse>(stringResponse);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<error>(stringResponse);
                    if (error.errorMessage == "Email is not verified")
                    {
                        App.User.OtpEmail = data.email;
                        App.User.FromEmailNotValid = true;
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Email is Not Verified", "Alert", "Ok");
                        var view = new OtpVerificationPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
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

        // Post CreateUser 
        public async Task<RegisterResponse> CreateUser(createUser data)
        {
            RegisterResponse response = null;
            try
            {
                var Url = BaseURL + "User/createUser";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<RegisterResponse>(stringResponse);
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

        // Post CreateRound 
        public async Task<createRoundResponse> CreateRound(CreateRound data)
        {
            createRoundResponse response = null;
            try
            {
                var Url = BaseURL + "Round/createRound";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<createRoundResponse>(stringResponse);
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

        // Post AddRoundPlayers 
        public  async Task<string> AddRoundPlayer(RoundPlayers data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Round/SaveRoundPlayer";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // Post CreateRoundPlayers 
        public async Task<string> CreateRoundPlayer(CreateRoundPlayers data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "Round/createRoundplayer";
                string json = JsonConvert.SerializeObject(data);
                Uri requestUri = new Uri(Url);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
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

        // Upload File
        public async Task<string> UploadFile(byte[] data)
        {
            string response = null;
            try
            {
                var Url = BaseURL + "UploadFile/UploadFileBytes";
                Uri requestUri = new Uri(Url);
                var formDataContent = new MultipartFormDataContent();
                formDataContent.Add(new ByteArrayContent(data), "files", "Image.png");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                var httpResponse = await _httpClient.PostAsync(requestUri, formDataContent);
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
    }
}
