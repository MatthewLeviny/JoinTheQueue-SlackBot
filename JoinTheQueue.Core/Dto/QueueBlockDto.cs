using JoinTheQueue.Core.Enum;

namespace JoinTheQueue.Core.Dto
{
    public class QueueBlockDto
    {
        public Block[] Blocks { get; set; }
    }

    public class Block
    {
        public BlockTypes Type { get; set; }
        public BlockText Text { get; set; }
        public BlockElements Elements { get; set; }
    }

    public class BlockElements
    {
        public ElementTypes Type { get; set; }
        public BlockText Text { get; set; }
        public BlockText[] Options { get; set; }
    }

    public class BlockText
    {
        public TextTypes Type { get; set; }
        public string Text { get; set; }
        public bool Emoji { get; set; }
    }
}