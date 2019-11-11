
namespace Golf.Models.userModel
{
    public class updateUser
    {
        public int userId { get; set; }
        public string address { get; set; }
        public int stateId { get; set; }
        public int countryId { get; set; }
        //[DefaultValue(false)]
        public bool? isEmailNotification { get; set; }
        //[DefaultValue(false)]
        public bool? isSMSNotification { get; set; }
        //[DefaultValue(false)]
        public bool? isPublicProfile { get; set; }
    }
}
