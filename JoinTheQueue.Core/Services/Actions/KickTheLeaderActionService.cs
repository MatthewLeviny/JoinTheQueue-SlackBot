using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Core.Services.Actions
{
    public class KickTheLeaderActionService : IActionService
    {
        private readonly IQueueDatabase _queueDatabase;
        private readonly IWebHookService _hookService;
        private readonly IBlockCreationService _blockCreationService;

        public KickTheLeaderActionService(IQueueDatabase queueDatabase, IWebHookService hookService,
            IBlockCreationService blockCreationService)
        {
            _queueDatabase = queueDatabase;
            _hookService = hookService;
            _blockCreationService = blockCreationService;
        }

        public string Key => "KickAction";

        public async Task PerformAction(Root request)
        {
            var queue = await _queueDatabase.GetQueue(request.channel.id, null);
            if (queue == null)
            {
                var responseNoQueue = new SlackResponseDto
                {
                    Text = "Queue Does not exist please create one",
                    ResponseType = BasicResponseTypes.ephemeral,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseNoQueue);
            }

            //empty queue
            if (queue != null && queue.Queue.Count == 0)
            {
                var responseEmptyQueue = new SlackResponseDto
                {
                    Text = "No one is in the queue bozo",
                    ResponseType = BasicResponseTypes.ephemeral,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseEmptyQueue);
            }

            //Removed from queue
        }
    }
}