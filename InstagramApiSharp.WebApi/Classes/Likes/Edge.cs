using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Likes
{
    public class Edge
    {
        [JsonProperty("node")] public Like Like { get; set; }
    }
}