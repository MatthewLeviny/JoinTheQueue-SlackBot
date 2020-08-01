using System.Collections.Generic;

namespace JoinTheQueue.Infrastructure.Dto
{
    public class QueueDto
    {
        public string ChannelId { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> UsersInQueue { get; set; }
    }
}