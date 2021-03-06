using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Comments
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Response
    {
        [JsonProperty("data")] public ShortcodeMedia Info { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
    }
}