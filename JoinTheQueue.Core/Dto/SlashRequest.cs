using Newtonsoft.Json;

namespace JoinTheQueue.Core.Dto
{
    public class SlashRequest
    {
        public string Token { get; set; }
        [JsonProperty("Team_Id")] public string TeamId { get; set; }
        [JsonProperty("Team_Domain")] public string TeamDomain { get; set; }
        [JsonProperty("Enterprise_id")] public string EnterpriseId { get; set; }
        [JsonProperty("Enterprise_Name")] public string EnterpriseName { get; set; }
        [JsonProperty("Channel_Id")] public string ChannelId { get; set; }
        [JsonProperty("Channel_Name")] public string ChannelName { get; set; }
        [JsonProperty("User_Id")] public string UserId { get; set; }
        [JsonProperty("User_Name")] public string UserName { get; set; }
        [JsonProperty("Command")] public string Command { get; set; }
        [JsonProperty("Text")] public string Text { get; set; }
        [JsonProperty("Response_Url")] public string ResponseUrl { get; set; }
        [JsonProperty("Trigger_Id")] public string TriggerId { get; set; }
    }
}