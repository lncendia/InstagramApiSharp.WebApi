using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Comments
{
    public class Edge
    {
        [JsonProperty("node")] public Comment Comment { get; set; }
    }
}