using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Core.Services
{
    public interface IQueueServices
    {
        Task<SlackResponseDto> JoinQueue(SlashRequest request);
        Task<SlackResponseDto> LeaveQueue(SlashRequest request);
        Task<SlackResponseDto> NudgeTheLeader(SlashRequest body);
    }

    public class QueueServices : IQueueServices
    {
        private readonly IQueueDatabase _queueDatabase;

        public QueueServices(IQueueDatabase queueDatabase)
        {
            _queueDatabase = queueDatabase;
        }

        public async Task<SlackResponseDto> JoinQueue(SlashRequest request)
        {
            //get current queue
            var queue = await _queueDatabase.GetQueue(request.Channel_Id, request.Enterprise_Id);
            if (queue == null)
            {
                return new SlackResponseDto
                {
                    text = "Queue Does not exist",
                    response_type = BasicResponseTypes.ephemeral
                };
            }

            queue.Queue.Enqueue(request.User_Id);
            await _queueDatabase.UpdateQueue(queue);
            return new SlackResponseDto
            {
                text = $"@{request.User_Id} has joined the queue",
                response_type = BasicResponseTypes.in_channel
            };
        }

        public async Task<SlackResponseDto> LeaveQueue(SlashRequest request)
        {
            //get current queue
            var queue = await _queueDatabase.GetQueue(request.Channel_Id, request.Enterprise_Id);
            if (queue == null)
            {
                return new SlackResponseDto
                {
                    text = "Queue Does not exist",
                    response_type = BasicResponseTypes.ephemeral
                };
            }

            var leaver = queue.Queue.Dequeue();
            await _queueDatabase.UpdateQueue(queue);

            return new SlackResponseDto
            {
                text = $"@{leaver} has left the queue" + "\n" +
                       $"@{queue.Queue.Peek()} ITS GO TIME",
                response_type = BasicResponseTypes.in_channel
            };
        }

        public Task<SlackResponseDto> NudgeTheLeader(SlashRequest body)
        {
            throw new System.NotImplementedException();
        }
    }
}