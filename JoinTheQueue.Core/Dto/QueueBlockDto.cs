using JoinTheQueue.Core.Enum;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Dto
{
    public class QueueBlockDto
    {
        [JsonProperty("blocks")] public Block[] Blocks { get; set; }
    }

    public class Block
    {
        [JsonProperty("type")] public BlockTypes Type { get; set; }
        [JsonProperty("text")] public BlockText Text { get; set; }
        [JsonProperty("elements")] public BlockElements Elements { get; set; }
    }

    public class BlockElements
    {
        [JsonProperty("type")] public ElementTypes Type { get; set; }
        [JsonProperty("text")] public BlockText Text { get; set; }
        [JsonProperty("options")] public BlockText[] Options { get; set; }
    }

    public class BlockText
    {
        [JsonProperty("type")] public TextTypes type { get; set; }
        [JsonProperty("text")] public string text { get; set; }
    }
}