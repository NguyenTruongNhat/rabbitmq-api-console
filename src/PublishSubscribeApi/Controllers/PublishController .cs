using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model.Share;

namespace PublishSubscribeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublishController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Publish([FromBody] NotificationMessage notification)
        {
            await _publishEndpoint.Publish(notification);
            return Ok(new { Message = "Notification published successfully" });
        }
    }
}
