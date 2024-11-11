using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Models.Google.GoogleOAuth.Response
{
    public class GoogleTokenResponse : GoogleResponse
    {
        [JsonPropertyName("access_token")]
        public string access_token { get; set; }
        [JsonPropertyName("refresh_token")]
        public string refresh_token { get; set; }
        [JsonPropertyName("expires_in")]
        public int expires_in { get; set; }
        [JsonPropertyName("token_type")]
        public string token_type { get; set; }
        [JsonPropertyName("scope")]
        public string scope { get; set; }

    }
}
