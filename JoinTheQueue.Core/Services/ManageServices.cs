using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Services
{
    public interface IManageServices
    {
        Task<SlackResponseDto> CreateQueueForChannel(SlashRequest request);
    }

    public class ManageServices : IManageServices
    {
        private readonly IWebHookService _webHookService;
        private readonly IBlockCreationService _blockCreationService;
        private readonly IQueueDatabase _queueDatabase;

        public ManageServices(IWebHookService webHookService, IBlockCreationService blockCreationService,
            IQueueDatabase queueDatabase)
        {
            _webHookService = webHookService;
            _blockCreationService = blockCreationService;
            _queueDatabase = queueDatabase;
        }

        public async Task<SlackResponseDto> CreateQueueForChannel(SlashRequest request)
        {
            // check to see if queue already exists
            var currentQueue = await _queueDatabase.GetQueue(request.Channel_Id, request.Enterprise_Id);

            string message;

            //output queue to channel
            QueueBlockDto blocks;
            if (currentQueue == null)
            {
                currentQueue = new QueueDto
                {
                    ChannelId = request.Channel_Id,
                    Name = request.Text,
                    Queue = new Queue<string>()
                };

                await _queueDatabase.UpdateQueue(currentQueue);
                message = $"Queue has been added to {request.Channel_Name}";
            }
            else
            {
                blocks = await _blockCreationService.CurrentQueue(currentQueue);
                message = $"Queue Already exists: {currentQueue.Name}";
                return new SlackResponseDto
                {
                    Text = message,
                    ResponseType = BasicResponseTypes.in_channel,
                    Blocks = blocks.Blocks
                };
            }

            blocks = await _blockCreationService.CurrentQueue(currentQueue);
            return new SlackResponseDto
            {
                Text = message,
                ResponseType = BasicResponseTypes.in_channel,
                Blocks = blocks.Blocks
            };
        }
    }
}