using System.Threading.Tasks;
using JoinTheQueue.Core.Authentication;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace JoinTheQueue.Api.Controllers
{
    /// <summary>
    /// Controller to deal with actions from slash commands
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Consumes("application/x-www-form-urlencoded")]
    [ApiController]
    [RequestAuth]
    public class QueueController : ControllerBase
    {
        private readonly IQueueServices _queueServices;

        public QueueController(IQueueServices queueServices, IManageServices manageServices)
        {
            _queueServices = queueServices;
        }

        [HttpPost]
        [Route("Join")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> JoinTheQueue([FromForm] SlashRequest body)
        {
            var returnMessage = await _queueServices.JoinQueue(body);
            return Ok(returnMessage);
        }

        [HttpPost]
        [Route("Leave")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LeaveTheQueue([FromForm] SlashRequest body)
        {
            var returnMessage = await _queueServices.LeaveQueue(body);
            return Ok(returnMessage);
        }

        [HttpPost]
        [Route("Nudge")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NudgeTheLeader([FromForm] SlashRequest body)
        {
            var returnMessage = await _queueServices.NudgeTheLeader(body);
            return Ok(returnMessage);
        }
    }
}