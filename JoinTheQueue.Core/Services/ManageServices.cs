﻿using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
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

        public ManageServices(IWebHookService webHookService)
        {
            _webHookService = webHookService;
        }

        public async Task<SlackResponseDto> CreateQueueForChannel(SlashRequest request)
        {
            // check to see if queue already exists
            var exists = false;
            // send out queue message
            var block = ConstructResponse();

            var message = "";
            // create message to be sent only visible to user


            //output queue to channel
            var hookResult = true; //await _webHookService.TriggerWebHook(request.Response_Url, block);
            if (hookResult)
            {
                message = $"Queue has been added to {request.ChannelId}";
            }

            return new SlackResponseDto
            {
                Text = "test",
                ResponseType = BasicResponseTypes.in_channel,
                Blocks = block.Blocks
            };
        }

        private QueueBlockDto ConstructResponse()
        {
            return new QueueBlockDto
            {
                Blocks = new Block[]
                {
                    new Block
                    {
                        Type = BlockTypes.section,
                        Text = new BlockText
                        {
                            type = TextTypes.mrkdwn,
                            text = "Hello"
                        }
                    }
                }
            };
        }
    }
}