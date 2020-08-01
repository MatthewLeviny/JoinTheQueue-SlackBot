using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace JoinTheQueue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueServices _queueServices;

        public QueueController(IQueueServices queueServices)
        {
            _queueServices = queueServices;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("Join")]
        [Produces("application/json")]
        public async Task<IActionResult> JoinTheQueue([FromForm] SlashRequest body)
        {
            var returnMessage = await _queueServices.JoinQueue(body);
            return Ok(returnMessage);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("Leave")]
        [Produces("application/json")]
        public async Task<IActionResult> LeaveTheQueue([FromForm] SlashRequest body)
        {
            var returnMessage = await _queueServices.LeaveQueue(body);
            return Ok(returnMessage);
        }
    }
}