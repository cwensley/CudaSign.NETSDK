using Newtonsoft.Json;

namespace SNDotNetSDK.Models
{
    /**
     * Created by Deepak on 5/14/2015
     *
     * This model class is used to place the Texts on the documents for a given document ID.
     */
    public class Text : Field
    {
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("font")]
        public string Font { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("line_height")]
        public double LineHeight { get; set; }
    }
}
