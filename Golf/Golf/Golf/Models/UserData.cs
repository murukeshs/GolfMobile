using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class UserData
    {
        public int userId { get; set; }
        public int userWithTypeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nickName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string profileImage { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public int countryId { get; set; }
        public int stateId { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string pinCode { get; set; }
        public bool? isEmailNotification { get; set; }
        public bool? isSMSNotification { get; set; }
        public bool? isPublicProfile { get; set; }
        public string userTypeId { get; set; }

        public string userType { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string userCreatedDate { get; set; }
        public string userUpdatedDate { get; set; }
        public bool? isEmailVerified { get; set; }
        public bool? isPhoneVerified { get; set; }
    }
}
