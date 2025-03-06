using Microsoft.AspNetCore.Mvc;
using RabbitMQProducer.Services;

namespace Producer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly ExchangeService service;
        public ProducerController(ExchangeService _service)
        {
            service = _service;
        }

        [Route("fanout")]
        [HttpPost]
        public async Task Fanout([FromBody] string input)
        {
            await service.Fanout(input);
        }

        [Route("topic")]
        [HttpPost]
        public async Task Topic([FromBody] string input)
        {
            await service.Topic(input);
        }

        [Route("direct")]
        [HttpPost]
        public async Task Direct([FromBody] string input)
        {
            await service.Direct(input);
        }

        [Route("header")]
        [HttpPost]
        public async Task Header([FromBody] string input)
        {
            await service.Header(input);
        }
    }
}
