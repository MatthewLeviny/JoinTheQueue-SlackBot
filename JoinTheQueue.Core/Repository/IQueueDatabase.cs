using System.Threading.Tasks;

namespace JoinTheQueue.Core.Repository
{
    public interface IQueueDatabase
    {
        Task<object> GetQueue(string chanelId, string enterpriseId);
        Task<object> UpdateQueue(string chanelId, string enterpriseId);
    }
}