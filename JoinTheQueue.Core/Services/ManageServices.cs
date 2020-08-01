using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using Newtonsoft.Json;

namespace JoinTheQueue.Core.Services
{
    public interface IManageServices
    {
        Task<SlackResponseDto> CreateQueueForChannel(SlashRequest request);
        Task<SlackResponseDto> NudgeTheLeader(SlashRequest body);
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
                message = $"Queue has been added to {request.Channel_Id}";
            }

            return new SlackResponseDto
            {
                response_type = BasicResponseTypes.in_channel,
                text = JsonConvert.SerializeObject(block)
            };
        }

        public Task<SlackResponseDto> NudgeTheLeader(SlashRequest body)
        {
            throw new System.NotImplementedException();
        }

        private QueueBlockDto ConstructResponse()
        {
            return new QueueBlockDto
            {
                Blocks = new Block[]
                {
                    new Block
                    {
                        Type = BlockTypes.Section,
                        Text = new BlockText
                        {
                            Type = TextTypes.Mrkdwn,
                            Text = "Hello"
                        }
                    }
                }
            };
        }
    }
}