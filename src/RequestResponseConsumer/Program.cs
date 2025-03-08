using MassTransit;
using Model.Share;

namespace RequestResponseConsumer
{
    // Consumer
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            Console.WriteLine($"Received request: {context.Message.Content}");

            await context.RespondAsync(new ResponseMessage
            {
                Content = $"Response to: {context.Message.Content}",
                Timestamp = DateTime.UtcNow
            });

            Console.WriteLine("Response sent back");
        }
    }

    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Request/Response Consumer";
            Console.WriteLine("Starting Request/Response Consumer...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "nhatnguyen", h =>
                {
                    h.Username("sa");
                    h.Password("pass");
                });

                cfg.ReceiveEndpoint("request-queue", e =>
                {
                    e.Consumer<RequestMessageConsumer>();
                });
            });

            await busControl.StartAsync();

            Console.WriteLine("Consumer started. Press Enter to exit");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }
}