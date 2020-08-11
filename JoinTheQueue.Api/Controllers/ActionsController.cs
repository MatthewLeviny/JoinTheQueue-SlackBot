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
    [Consumes("application/x-www-form-urlencoded")]
    [ApiController]
    //[RequestAuth]
    public class ActionsController : ControllerBase
    {
        private readonly IQueueServices _queueServices;

        public ActionsController(IQueueServices queueServices)
        {
            _queueServices = queueServices;
        }

        [HttpPost]
        //[Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> JoinTheQueue([FromForm] InteractionRequest body)
        {
            //var returnMessage = await _queueServices.JoinQueue(body);
            return Ok();
        }
    }
}
