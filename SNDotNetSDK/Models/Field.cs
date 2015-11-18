using System.Collections.Generic;
using Newtonsoft.Json;

namespace SNDotNetSDK.Models
{
    /**
     * Created by Deepak on 5/14/2015
     * 
     * This model is useful to build the Fields Object to have different types of files like, CheckBox, Texts and Initials.
     */
    public class Field
    {
		List<Radio> radio = new List<Radio>();

        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("required")]
        public bool Required { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
		[JsonProperty("role_id")]
		public string RoleId { get; set; }
		[JsonProperty("user_id")]
		public string UserId { get; set; }
		[JsonProperty("created")]
		public string Created { get; set; }
		[JsonProperty("radio")]
		public List<Radio> Radio { get { return radio; } }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}