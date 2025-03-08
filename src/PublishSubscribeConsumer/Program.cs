using MassTransit;
using Model.Share;

namespace PublishSubscribeConsumer
{

    // Consumer
    public class NotificationConsumer : IConsumer<NotificationMessage>
    {
        public Task Consume(ConsumeContext<NotificationMessage> context)
        {
            var message = context.Message;

            Console.WriteLine("=== NOTIFICATION RECEIVED ===");
            Console.WriteLine($"Title: {message.Title}");
            Console.WriteLine($"Content: {message.Content}");
            Console.WriteLine($"Time: {message.Timestamp}");
            Console.WriteLine("=============================");

            return Task.CompletedTask;
        }
    }

    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Publish/Subscribe Consumer";
            Console.WriteLine("Starting Notification Subscriber...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "nhatnguyen", h =>
                {
                    h.Username("sa");
                    h.Password("pass");
                });

                cfg.ReceiveEndpoint("notification-subscriber", e =>
                {
                    e.Consumer<NotificationConsumer>();
                });
            });

            await busControl.StartAsync();

            Console.WriteLine("Subscriber started. Press Enter to exit");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }
}