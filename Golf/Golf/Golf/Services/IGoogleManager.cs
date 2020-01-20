using Golf.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Services
{
    ///This interface is used to access the native google signin method. 
    //Use this Login method to login when user click the Google Sign in. 
    //When user is logged in via Google in my application we have set automatically logged out.
    //Our only need is get Gmail provider key and logout the gmail.
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
