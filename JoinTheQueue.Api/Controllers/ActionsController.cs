using System.Linq;
using System.Threading.Tasks;
using JoinTheQueue.Core.Authentication;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JoinTheQueue.Api.Controllers
{
    /// <summary>
    /// Controller to deal with actions from blocks
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/[controller]")]
    [Consumes("application/x-www-form-urlencoded")]
    [ApiController]
    [RequestAuth]
    public class ActionsController : ControllerBase
    {
        private readonly IActionServiceFactory _factory;

        public ActionsController(IActionServiceFactory factory)
        {
            _factory = factory;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Action([FromForm] string payload)
        {
            var request = JsonConvert.DeserializeObject<Root>(payload);
            var actionService = _factory.GetActionService(request.actions.First());
            await actionService.PerformAction(request);
            return Ok();
        }
    }
}