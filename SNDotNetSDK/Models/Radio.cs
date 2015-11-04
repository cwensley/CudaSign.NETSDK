using Newtonsoft.Json;

namespace SNDotNetSDK.Models
{
    /**
     * Created by Deepak on 5/14/2015
     * 
     * This model object is used to place the radio button on the document.
     */
    public class Radio : Field
    {
        [JsonProperty("checked")]
        public int Check { get; set; }

        public string Value { get; set; }
    }
}
