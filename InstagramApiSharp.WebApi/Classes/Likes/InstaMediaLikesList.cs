using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Likes
{
    public class InstaMediaLikesList
    {
        [JsonProperty("count")] public int CountMedia { get; set; }
        [JsonProperty("page_info")] public NextPageInfo NextPageInfo { get; set; }
        [JsonProperty("edges")] public List<Edge> Edges { get; set; }
    }
}