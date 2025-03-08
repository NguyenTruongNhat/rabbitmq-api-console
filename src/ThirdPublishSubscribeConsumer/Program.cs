using MassTransit;
using Model.Share;

namespace ThirdPublishSubscribeConsumer
{

    // Consumer thứ ba tập trung vào phân tích nội dung
    public class AnalyticsNotificationConsumer : IConsumer<NotificationMessage>
    {
        // Mô phỏng database đơn giản để theo dõi thống kê
        private static Dictionary<string, int> notificationCategories = new Dictionary<string, int>();

        public Task Consume(ConsumeContext<NotificationMessage> context)
        {
            var message = context.Message;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*** ANALYTICS CONSUMER ***");
            Console.WriteLine($"Processing notification: {message.Title}");

            // Phân loại thông báo dựa trên tiêu đề (đơn giản hóa)
            string category = ClassifyNotification(message.Title);

            // Cập nhật số lượng
            if (notificationCategories.ContainsKey(category))
            {
                notificationCategories[category]++;
            }
            else
            {
                notificationCategories[category] = 1;
            }

            // Hiển thị thống kê hiện tại
            Console.WriteLine("Current Notification Statistics:");
            foreach (var item in notificationCategories)
            {
                Console.WriteLine($"- {item.Key}: {item.Value}");
            }

            Console.WriteLine("*** END ANALYTICS ***");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        private string ClassifyNotification(string title)
        {
            // Logic phân loại đơn giản dựa trên từ khóa trong tiêu đề
            string titleLower = title.ToLower();

            if (titleLower.Contains("error") || titleLower.Contains("fail"))
                return "Error";
            else if (titleLower.Contains("warning") || titleLower.Contains("alert"))
                return "Warning";
            else if (titleLower.Contains("important"))
                return "Important";
            else
                return "General";
        }
    }

    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Analytics Notification Consumer";
            Console.WriteLine("Starting Analytics Notification Subscriber...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "nhatnguyen", h =>
                {
                    h.Username("sa");
                    h.Password("pass");
                });

                // Sử dụng endpoint riêng cho consumer phân tích
                cfg.ReceiveEndpoint("notification-subscriber-analytics", e =>
                {
                    e.Consumer<AnalyticsNotificationConsumer>();
                });
            });

            await busControl.StartAsync();

            Console.WriteLine("Analytics subscriber started. Press Enter to exit");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }
}