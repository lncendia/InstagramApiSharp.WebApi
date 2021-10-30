using System.Collections.Generic;
using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers.Classes.Follows
{
    public class Follows
    {
        [JsonProperty("users")] public List<User> Users { get; set; }
        [JsonProperty("next_max_id")] public string NextMaxId { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
    }
}