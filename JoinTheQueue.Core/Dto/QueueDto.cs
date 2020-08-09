using System.Collections.Generic;

namespace JoinTheQueue.Core.Dto
{
    public class QueueDto
    {
        public string ChannelId { get; set; }
        public Queue<string> Queue { get; set; }
    }
}