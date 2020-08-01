using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;

namespace JoinTheQueue.Core.Services
{
    public interface IQueueServices
    {
        Task<SlackResponseDto> JoinQueue(SlashRequest request);
        Task<SlackResponseDto> LeaveQueue(SlashRequest request);
    }

    public class QueueServices : IQueueServices
    {
        public Task<SlackResponseDto> JoinQueue(SlashRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<SlackResponseDto> LeaveQueue(SlashRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}