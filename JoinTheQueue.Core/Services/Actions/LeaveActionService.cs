using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Core.Services.Actions
{
    public class LeaveActionService : IActionService
    {
        private readonly IQueueDatabase _queueDatabase;
        private readonly IWebHookService _hookService;
        private readonly IBlockCreationService _blockCreationService;

        public LeaveActionService(IQueueDatabase queueDatabase, IWebHookService hookService,
            IBlockCreationService blockCreationService)
        {
            _queueDatabase = queueDatabase;
            _hookService = hookService;
            _blockCreationService = blockCreationService;
        }

        public string Key => "LeaveAction";

        public async Task PerformAction(Root request)
        {
            var queue = await _queueDatabase.GetQueue(request.channel.id, null);
            if (queue == null)
            {
                var responseNoQueue = new SlackResponseDto()
                {
                    Text = "Queue Does not exist please create one",
                    ResponseType = BasicResponseTypes.in_channel,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseNoQueue);
                return;
            }

            if (!queue.Queue.Contains(request.user.username))
            {
                var responseNotInQueue = new SlackResponseDto()
                {
                    Text = "you're not in the queue numpty",
                    ResponseType = BasicResponseTypes.ephemeral,
                    DeleteOriginal = false,

                };

                await _hookService.TriggerWebHook(request.response_url, responseNotInQueue);
                return;
            }

            var leaver = request.user.username;
            queue.Queue = new Queue<string>(queue.Queue.Where(x => x != leaver));
            await _queueDatabase.UpdateQueue(queue);

            var responseWebHook = new SlackResponseDto()
            {
                ResponseType = BasicResponseTypes.in_channel,
                DeleteOriginal = true,
                Blocks = _blockCreationService.CurrentQueue(queue).Result.Blocks
            };

            await _hookService.TriggerWebHook(request.response_url, responseWebHook);

            var text = $"@{request.user.name} has left the queue" + "\n";
            text += queue.Queue.Any() ? $"@{queue.Queue.Peek()} ITS GO TIME" : "Queue is empty";

            var slackMessage = new SlackResponseDto
            {
                Text = text,
                ResponseType = BasicResponseTypes.in_channel,
            };
            await _hookService.TriggerWebHook(request.response_url, slackMessage);
        }
    }
}