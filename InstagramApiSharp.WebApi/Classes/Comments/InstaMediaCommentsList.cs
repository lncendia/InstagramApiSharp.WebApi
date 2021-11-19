using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes.Comments
{
    public class InstaMediaCommentsList
    {
        [JsonProperty("count")] public int CountMedia { get; set; }
        [JsonProperty("page_info")] public NextPageInfo NextPageInfo { get; set; }
        [JsonProperty("edges")] public List<Edge> Edges { get; set; }
    }
}