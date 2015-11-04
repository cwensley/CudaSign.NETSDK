using Newtonsoft.Json;
using System.Collections.Generic;
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

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

		[JsonProperty("page_number")]
        public int PageNumber { get; set; }

        public string Role { get; set; }

        public bool Required { get; set; }

        public string Type { get; set; }

		[JsonProperty("role_id")]
		public string RoleId { get; set; }

		[JsonProperty("user_id")]
		public string UserId { get; set; }

		public string Created { get; set; }

        public List<Radio> Radio { get { return radio; } }

        public string Email { get; set; }
    }
}