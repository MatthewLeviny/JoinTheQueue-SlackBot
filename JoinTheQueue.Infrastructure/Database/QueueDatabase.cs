using System.Threading.Tasks;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Infrastructure.Database
{

    public class QueueDatabase : IQueueDatabase
    {
        public Task<object> GetQueue(string chanelId, string enterpriseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> UpdateQueue(string chanelId, string enterpriseId)
        {
            throw new System.NotImplementedException();
        }
    }
}