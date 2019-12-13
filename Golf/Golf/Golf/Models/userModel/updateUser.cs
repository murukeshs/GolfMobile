
namespace Golf.Models.userModel
{
    public class updateUser
    {
        public int userId { get; set; }
        public string address { get; set; }
        public int stateId { get; set; }
        public int countryId { get; set; }
        public bool? isEmailNotification { get; set; }
        public bool? isSMSNotification { get; set; }
        public bool? isPublicProfile { get; set; }
    }
}
