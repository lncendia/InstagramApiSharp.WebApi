using InstagramApiSharp.Classes.ResponseWrappers;
using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Likes
{
    public class Edge
    {
        [JsonProperty("node")] public InstaUserShortResponse Like { get; set; }
    }
}