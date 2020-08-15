using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Services.Actions
{
    public class JoinActionService : IActionService
    {
        private readonly IWebHookService _hookService;
        private readonly IQueueDatabase _queueDatabase;
        private readonly IBlockCreationService _blockCreationService;

        public JoinActionService(IWebHookService hookService, IQueueDatabase queueDatabase,
            IBlockCreationService blockCreationService)
        {
            _hookService = hookService;
            _queueDatabase = queueDatabase;
            _blockCreationService = blockCreationService;
        }

        public string Key => "JoinAction";

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

            if (queue.Queue.Contains(request.user.username))
            {
                var queueBlock = await _blockCreationService.CurrentQueue(queue);
                var responseAlreadyInQueueBlock = new SlackResponseDto()
                {
                    Text = "test",
                    ResponseType = BasicResponseTypes.in_channel,
                    Blocks = queueBlock.Blocks,
                    DeleteOriginal = true,
                    ReplaceOriginal = false
                };

                await _hookService.TriggerWebHook(request.response_url, responseAlreadyInQueueBlock);

                var responseAlreadyInQueue = new SlackResponseDto
                {
                    Text = "Already in the queue",
                    ResponseType = BasicResponseTypes.ephemeral
                };

                await _hookService.TriggerWebHook(request.response_url, responseAlreadyInQueue);
            }
            else
            {
                queue.Queue.Enqueue(request.user.username);
                await _queueDatabase.UpdateQueue(queue);

                var queueBlock = await _blockCreationService.CurrentQueue(queue);
                var responseJoinBlock = new SlackResponseDto
                {
                    Text = "Queue",
                    ResponseType = BasicResponseTypes.in_channel,
                    Blocks = queueBlock.Blocks,
                    DeleteOriginal = false,
                    ReplaceOriginal = true
                };
                await _hookService.TriggerWebHook(request.response_url, responseJoinBlock);

                var responseJoinMessage = new SlackResponseDto
                {
                    Text = $"@{request.user.username} Has joined the queue",
                    ResponseType = BasicResponseTypes.in_channel,
                };
                await _hookService.TriggerWebHook(request.response_url, responseJoinMessage);
            }
        }
    }
}