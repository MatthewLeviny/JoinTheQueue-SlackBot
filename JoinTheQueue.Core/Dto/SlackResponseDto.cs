using JoinTheQueue.Core.Enum;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Dto
{
    public class SlackResponseDto
    {
        [JsonProperty("response_type")]
        public BasicResponseTypes ResponseType { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("blocks")]
        public Block[] Blocks { get; set; }
    }
}