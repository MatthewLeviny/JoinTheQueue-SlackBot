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
        [JsonProperty("elements")] public BlockElement[] Elements { get; set; }
    }

    public class BlockElement
    {
        [JsonProperty("type")] public ElementTypes Type { get; set; }
        [JsonProperty("text")] public BlockText Text { get; set; }
        [JsonProperty("options")] public BlockText[] Options { get; set; }
        [JsonProperty("value")] public string Value { get; set; }
    }

    public class BlockText
    {
        [JsonProperty("type")] public TextTypes Type { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
    }
}