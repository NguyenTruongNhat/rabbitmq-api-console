using MassTransit;
using Model.Share;

namespace SecondPublishSubscribeConsumer
{

    // Consumer thứ hai sẽ xử lý message theo cách khác
    public class SecondNotificationConsumer : IConsumer<NotificationMessage>
    {
        public Task Consume(ConsumeContext<NotificationMessage> context)
        {
            var message = context.Message;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">>> SECOND CONSUMER NOTIFICATION <<<");
            Console.WriteLine($"ALERT: {message.Title}");
            Console.WriteLine($"DETAILS: {message.Content}");
            Console.WriteLine($"RECEIVED AT: {DateTime.Now}");
            Console.WriteLine($"ORIGINAL TIMESTAMP: {message.Timestamp}");
            Console.WriteLine(">>> END OF NOTIFICATION <<<");
            Console.ResetColor();

            // Lưu log thông báo (mô phỏng)
            Console.WriteLine("Logging notification to database...");

            return Task.CompletedTask;
        }
    }

    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Second Publish/Subscribe Consumer";
            Console.WriteLine("Starting Second Notification Subscriber...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "nhatnguyen", h =>
                {
                    h.Username("sa");
                    h.Password("pass");
                });

                // Sử dụng một tên endpoint khác để tạo một consumer riêng biệt
                cfg.ReceiveEndpoint("notification-subscriber-logger", e =>
                {
                    e.Consumer<SecondNotificationConsumer>();
                });
            });

            await busControl.StartAsync();

            Console.WriteLine("Second subscriber started. Press Enter to exit");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }
}