using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using JoinTheQueue.Core.Dto;

namespace JoinTheQueue.Infrastructure.Entity
{
    [DynamoDBTable("JoinTheQueue")]
    public class QueueEntity
    {

        public string Name { get; set; }
        [DynamoDBHashKey]
        public string ChannelId { get; set; }
        public string Queue { get; set; }
    }
}