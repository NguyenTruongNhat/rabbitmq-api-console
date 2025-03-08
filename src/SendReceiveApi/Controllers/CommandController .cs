using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model.Share;

namespace SendReceiveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public CommandController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] CommandMessage command)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:command-queue"));
            await endpoint.Send(command);
            return Ok(new { Message = "Command sent successfully", Id = command.CorrelationId });
        }
    }
}
