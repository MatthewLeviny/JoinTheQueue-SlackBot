using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Infrastructure.Database
{

    public class QueueDatabase : IQueueDatabase
    {
        public Task<object> GetQueue(string chanelId, string enterpriseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<QueueDto> UpdateQueue(QueueDto queue)
        {
            throw new System.NotImplementedException();
        }

        Task<QueueDto> IQueueDatabase.GetQueue(string chanelId, string enterpriseId)
        {
            throw new System.NotImplementedException();
        }
    }
}