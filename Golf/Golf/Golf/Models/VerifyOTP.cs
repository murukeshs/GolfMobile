using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Models
{
    public class VerifyOTP
    {
        public string otpValue { get; set; }
        public string emailorPhone { get; set; }
        public string type { get; set; }
        public string sourceType { get; set; }
    }
}
