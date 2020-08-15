using System.Linq;
using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Core.Services.Actions
{
    public class NudgeAction : IActionService
    {
        private readonly IQueueDatabase _queueDatabase;
        private readonly IWebHookService _hookService;

        public NudgeAction(IQueueDatabase queueDatabase, IWebHookService hookService)
        {
            _queueDatabase = queueDatabase;
            _hookService = hookService;
        }

        public string Key => "NudgeAction";

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

            if (queue.Queue.Any())
            {
                var responseConfirmation = new SlackResponseDto()
                {
                    Text = $"Nudging @{queue.Queue.Peek()}",
                    ResponseType = BasicResponseTypes.in_channel,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseConfirmation);

                var responseNudge = new SlackResponseDto()
                {
                    Text = $"OI MAKE SURE YOU ARE STILL USING THIS @{queue.Queue.Peek()}",
                    ResponseType = BasicResponseTypes.ephemeral,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseNudge);
                return;
            }

            var responseEmpty = new SlackResponseDto()
            {
                Text = $"Queue is empty",
                ResponseType = BasicResponseTypes.ephemeral,
                DeleteOriginal = true,
            };

            await _hookService.TriggerWebHook(request.response_url, responseEmpty);
        }
    }
}