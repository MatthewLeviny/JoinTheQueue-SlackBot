using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Enum;
using JoinTheQueue.Core.Repository;

namespace JoinTheQueue.Core.Services.Actions
{
    public class RenameActionService : IActionService
    {
        private readonly IQueueDatabase _queueDatabase;
        private readonly IWebHookService _hookService;


        public RenameActionService(IQueueDatabase queueDatabase, IWebHookService hookService)
        {
            _queueDatabase = queueDatabase;
            _hookService = hookService;
        }

        public string Key => "RenameAction";

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

                return;
            }



        }
    }
}