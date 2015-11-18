using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace SNDotNetSDK.Models
{
	public class EmailRole
	{
		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("role_id")]
		public string RoleId { get; set; }

		[JsonProperty("role")]
		public string Role { get; set; }

		[JsonProperty("order")]
		public int Order { get; set; }
	}

    /**
     * Created by Deepak on 5/14/2015
     * 
     * This model object is used to set the email for the document invite.
     */
    public class EmailSignature
    {
        [JsonProperty("to")]
        public List<EmailRole> To { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("cc")]
        public string[] CC { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}