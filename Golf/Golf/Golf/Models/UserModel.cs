using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class UserModel
    {
        internal string BaseUrl = "https://golfapp.azurewebsites.net/api/"; 

        internal string AccessToken { get; set; } = string.Empty;

        internal string UserName { get; set; } = string.Empty;

        internal string UserEmailId { get; set; } = string.Empty;
    }
}
