namespace Golf.Models.userModel
{
    public class getUser : createUser
    {
        public string userType { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string userCreatedDate { get; set; }
        public string userUpdatedDate { get; set; }
        //[DefaultValue(false)]
        public bool? isEmailVerified { get; set; }
        //[DefaultValue(false)]
        public bool? isPhoneVerified { get; set; }

    }
}
