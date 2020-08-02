using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;

namespace JoinTheQueue.Core.Repository
{
    public interface IQueueDatabase
    {
        Task<QueueDto> GetQueue(string chanelId, string enterpriseId);
        Task<QueueDto> UpdateQueue(QueueDto queue);
    }
}