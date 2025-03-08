using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model.Share;

namespace RequestResponseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestResponseController : ControllerBase
    {
        private readonly IRequestClient<RequestMessage> _requestClient;

        public RequestResponseController(IRequestClient<RequestMessage> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] RequestMessage request)
        {
            var response = await _requestClient.GetResponse<ResponseMessage>(request);
            return Ok(response.Message);
        }
    }
}
