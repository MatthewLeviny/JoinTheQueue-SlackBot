using System.Threading.Tasks;
using JoinTheQueue.Core.Authentication;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JoinTheQueue.Api.Controllers
{
    /// <summary>
    /// Controller to deal with actions from blocks
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    [RequestAuth]
    public class ActionsController : ControllerBase
    {
        private readonly IQueueServices _queueServices;

        public ActionsController(IQueueServices queueServices)
        {
            _queueServices = queueServices;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("Nudge")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NudgeTheLeader([FromForm] SlashRequest body)
        {
            var returnMessage = await _queueServices.NudgeTheLeader(body);
            return Ok(returnMessage);
        }
    }
}
