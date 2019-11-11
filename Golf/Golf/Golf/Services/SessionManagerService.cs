using System;

namespace Golf.Services
{
    internal class SessionManagerService
    {
        private bool _isAlertVisible = false;
        private int _showWindowInSeconds = 70;
        private bool _isMonitorActive = false;

        public bool CheckSessionAndLogout()
        {
            bool isSessionInvalid = false;
            var currentTime = DateTime.UtcNow;

            //if (App.User != null && App.User.TokenCreateDateTimeUtc != null)
            //{
            //    // Logout time = Login time + seconds until token expiration.
            //    var logoutTime = App.User.TokenCreateDateTimeUtc + TimeSpan.FromSeconds(App.User.AccessTokenExpireSeconds); // TimeSpan.FromSeconds(60);
            //    // if the current time >= Logout Time, log user out.

            //    if (currentTime >= logoutTime)
            //    {
            //        isSessionInvalid = true;
            //        Logout();
            //    }
            //}
            //else
            //{
            //    isSessionInvalid = true;
            //    Logout();
            //}

            return isSessionInvalid;
        }

        public void EnableMonitor()
        {
            _isMonitorActive = true;
        }

        public void Logout()
        {
            //_isMonitorActive = false;
            //App.User = new UserModel();
            //((App)App.Current).MainPage = new NavigationPage(new LoginView(true));
        }

        //internal Action StartMonitoringSession = async () => {
        //    while (true)
        //    {
        //        try
        //        {

        //            await Task.Delay(20 * 1000);

        //            if (App.SessionManager._isMonitorActive)
        //            {

        //                // It is possible the session died, run checks and logout if needed.
        //                if (!App.SessionManager.CheckSessionAndLogout())
        //                {

        //                    var currentTime = DateTime.UtcNow;
        //                    // Logout time = Login time + seconds until token expiration.
        //                    var logoutTime = App.User.TokenCreateDateTimeUtc + TimeSpan.FromSeconds(App.User.AccessTokenExpireSeconds);// TimeSpan.FromSeconds(60);
        //                                                                                                                               // if the current time >= Logout Time - 70, show window.
        //                    if (!App.SessionManager._isAlertVisible && currentTime >= logoutTime - TimeSpan.FromSeconds(App.SessionManager._showWindowInSeconds))
        //                    {
        //                        App.SessionManager._isAlertVisible = true;
        //                        UserDialogs.Instance.Confirm(new ConfirmConfig()
        //                        {
        //                            Message = "Your session is about to expire.\n\nWould you like to continue your session?",
        //                            CancelText = "Logout",
        //                            OkText = "Continue",
        //                            OnAction = async (isPositiveAction) => {
        //                                if (isPositiveAction)
        //                                {
        //                                    var refreshTokenRequest = new CxauRefreshTokenRequest()
        //                                    {
        //                                        RefreshToken = App.User.RefreshToken,
        //                                    };

        //                                    var refreshTokenResponse = await App.ApiClient.RefreshToken(refreshTokenRequest);
        //                                    if (refreshTokenResponse != null && !refreshTokenResponse.Error && !string.IsNullOrWhiteSpace(refreshTokenResponse.access_token))
        //                                    {
        //                                        App.User.AccessToken = refreshTokenResponse.access_token;
        //                                        App.User.AccessTokenExpireSeconds = refreshTokenResponse.expires_in;
        //                                        App.User.TokenCreateDateTimeUtc = DateTime.UtcNow;
        //                                        App.User.RefreshToken = refreshTokenResponse.refresh_token;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    App.SessionManager.Logout();
        //                                }
        //                                App.SessionManager._isAlertVisible = false;
        //                            }
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            App.SessionManager._isAlertVisible = false;
        //        }
        //    }
        //};
    }
}
