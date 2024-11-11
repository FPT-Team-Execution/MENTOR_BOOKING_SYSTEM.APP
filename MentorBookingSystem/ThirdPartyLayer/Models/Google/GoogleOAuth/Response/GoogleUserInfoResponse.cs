using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Models.Google.GoogleOAuth.Response
{
    public class GoogleUserInfoResponse : GoogleResponse
    {
        public string sub { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
    }
}
