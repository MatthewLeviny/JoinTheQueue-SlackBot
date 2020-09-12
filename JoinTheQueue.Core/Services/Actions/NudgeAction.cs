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
        private readonly IBlockCreationService _blockCreationService;

        public NudgeAction(IQueueDatabase queueDatabase, IWebHookService hookService,
            IBlockCreationService blockCreationService)
        {
            _queueDatabase = queueDatabase;
            _hookService = hookService;
            _blockCreationService = blockCreationService;
        }

        public string Key => "NudgeAction";

        public async Task PerformAction(Root request)
        {
            var queue = await _queueDatabase.GetQueue(request.channel.id, null);
            var queueBlock = await _blockCreationService.CurrentQueue(queue);
            SlackResponseDto responseBlocks;
            if (queue.Queue.Any())
            {
                var responseConfirmation = new SlackResponseDto
                {
                    Text = $"OI MAKE SURE YOU ARE STILL USING THIS @{queue.Queue.Peek()}",
                    ResponseType = BasicResponseTypes.in_channel,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseConfirmation);

                responseBlocks = new SlackResponseDto
                {
                    Text = $"Nudging @{queue.Queue.Peek()}",
                    ResponseType = BasicResponseTypes.in_channel,
                    Blocks = queueBlock.Blocks,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseBlocks);

                var responseNudge = new SlackResponseDto()
                {
                    Text = $"Nudged the leader",
                    ResponseType = BasicResponseTypes.ephemeral,
                    DeleteOriginal = true,
                };

                await _hookService.TriggerWebHook(request.response_url, responseNudge);
                return;
            }

            var responseEmpty = new SlackResponseDto
            {
                Text = "Queue is empty",
                ResponseType = BasicResponseTypes.ephemeral,
                DeleteOriginal = true,
            };
            await _hookService.TriggerWebHook(request.response_url, responseEmpty);

            responseBlocks = new SlackResponseDto
            {
                ResponseType = BasicResponseTypes.in_channel,
                Blocks = queueBlock.Blocks,
                DeleteOriginal = true,
            };

            await _hookService.TriggerWebHook(request.response_url, responseBlocks);
        }
    }
}