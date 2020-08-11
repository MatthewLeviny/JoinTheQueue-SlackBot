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
        Task<SlackResponseDto> ShowCurrentQueue(SlashRequest request);
    }

    public class QueueServices : IQueueServices
    {
        private readonly IQueueDatabase _queueDatabase;
        private readonly IBlockCreationService _blockCreationService;

        public QueueServices(IQueueDatabase queueDatabase, IBlockCreationService blockCreationService)
        {
            _queueDatabase = queueDatabase;
            _blockCreationService = blockCreationService;
        }

        public async Task<SlackResponseDto> JoinQueue(SlashRequest request)
        {
            //get current queue
            var queue = await _queueDatabase.GetQueue(request.Channel_Id, request.Enterprise_Id);
            if (queue == null)
            {
                return new SlackResponseDto
                {
                    Text = "Queue Does not exist",
                    ResponseType = BasicResponseTypes.ephemeral
                };
            }

            if (queue.Queue.Contains(request.User_Name))
            {
                return new SlackResponseDto
                {
                    Text = $"You are already in the queue bozo",
                    ResponseType = BasicResponseTypes.ephemeral
                };
            }

            queue.Queue.Enqueue(request.User_Name);
            await _queueDatabase.UpdateQueue(queue);

            //webhook to show updated and remove old message if too old

            return new SlackResponseDto
            {
                Text = $"@{request.User_Name} has joined the queue",
                ResponseType = BasicResponseTypes.in_channel
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
                    Text = "Queue Does not exist",
                    ResponseType = BasicResponseTypes.ephemeral
                };
            }

            var leaver = queue.Queue.Dequeue();
            await _queueDatabase.UpdateQueue(queue);

            return new SlackResponseDto
            {
                Text = $"@{leaver} has left the queue" + "\n" +
                       $"@{queue.Queue.Peek()} ITS GO TIME",
                ResponseType = BasicResponseTypes.in_channel
            };
        }

        public Task<SlackResponseDto> NudgeTheLeader(SlashRequest body)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SlackResponseDto> ShowCurrentQueue(SlashRequest request)
        {
            var currentQueue = await _queueDatabase.GetQueue(request.Channel_Id, request.Enterprise_Id);
            if (currentQueue == null)
            {
                return new SlackResponseDto
                {
                    Text = "Queue doesn't exist for channel",
                    ResponseType = BasicResponseTypes.ephemeral,
                };
            }

            var blocks = await _blockCreationService.CurrentQueue(currentQueue);
            return new SlackResponseDto
            {
                ResponseType = BasicResponseTypes.in_channel,
                Blocks = blocks.Blocks
            };
        }
    }
}