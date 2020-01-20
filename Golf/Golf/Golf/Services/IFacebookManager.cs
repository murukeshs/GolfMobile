using Golf.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Services
{
    public interface IFacebookManager
    {
        void Login(Action<FacebookUser, string> onLoginComplete);
        void Logout();
    }
}
