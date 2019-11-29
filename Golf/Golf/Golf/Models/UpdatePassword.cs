using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class UpdatePassword
    {
        public string otpValue { get; set; }
        public string emailorPhone { get; set; }
        public string sourceType { get; set; }
        public string password { get; set; }
    }
}
