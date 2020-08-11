using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Repository;
using JoinTheQueue.Infrastructure.Entity;

namespace JoinTheQueue.Infrastructure.Database
{
    public class QueueDatabase : IQueueDatabase
    {
        private readonly DynamoDBContext _context;


        public QueueDatabase(IAmazonDynamoDB amazonDynamoDb)
        {
            _context = new DynamoDBContext(amazonDynamoDb);
        }

        public async Task<QueueDto> GetQueue(string chanelId, string enterpriseId)
        {
            var result = await _context.LoadAsync<QueueEntity>(chanelId);
            if (result == null) return null;
            var list = result.Queue == null?  new List<string>(): new List<string>(result.Queue.Split(','));
            var queue = new Queue<string>();
            foreach (var item in list)
            {
                queue.Enqueue(item);
            }

            return new QueueDto
            {
                Queue = queue,
                Name = result.Name,
                ChannelId = result.ChannelId
            };
        }

        public async Task RemoveQueue(string chanelId, string enterpriseId)
        {
            await _context.DeleteAsync<QueueEntity>(chanelId);
        }

        public async Task<QueueDto> UpdateQueue(QueueDto queue)
        {
            await _context.SaveAsync(new QueueEntity
            {
                Queue = string.Join(",", queue.Queue.ToArray()),
                Name = queue.Name,
                ChannelId = queue.ChannelId
            });
            return await GetQueue(queue.ChannelId, null);
        }
    }
}