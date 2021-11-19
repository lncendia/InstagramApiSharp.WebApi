using Newtonsoft.Json;

namespace InstagramApiSharp.WebApi.Classes
{
    public class NextPageInfo
    {
        [JsonProperty("has_next_page")] public bool HasNextPage { get; set; }

        [JsonProperty("end_cursor")] public string NextMaxId { get; set; }
    }
}