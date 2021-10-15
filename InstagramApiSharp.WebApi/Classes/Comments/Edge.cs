using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Comments
{
    public class Edge
    {
        [JsonProperty("node")] public Comment Comment { get; set; }
    }
}