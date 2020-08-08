using System.Threading.Tasks;
using JoinTheQueue.Core.Authentication;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace JoinTheQueue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RequestAuth]
    public class ManageController : Controller
    {
        private readonly IManageServices _manageServices;

        public ManageController(IManageServices manageServices)
        {
            this._manageServices = manageServices;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("AddQueue")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InitialiseQueue([FromForm] SlashRequest body)
        {
            var returnMessage = await _manageServices.CreateQueueForChannel(body);
            return Ok(returnMessage);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("Nudge")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NudgeTheLeader([FromForm] SlashRequest body)
        {
            var returnMessage = await _manageServices.NudgeTheLeader(body);
            return Ok(returnMessage);
        }
    }
}