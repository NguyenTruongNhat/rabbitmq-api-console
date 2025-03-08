using MassTransit;
using Model.Share;

namespace SendReceiveConsumer
{

    // Consumer
    public class CommandConsumer : IConsumer<CommandMessage>
    {
        public Task Consume(ConsumeContext<CommandMessage> context)
        {
            var message = context.Message;

            Console.WriteLine("=== COMMAND RECEIVED ===");
            Console.WriteLine($"ID: {message.CorrelationId}");
            Console.WriteLine($"Command: {message.Command}");
            Console.WriteLine($"Parameters: {message.Parameters}");
            Console.WriteLine("========================");

            // Xử lý lệnh
            Console.WriteLine($"Processing command: {message.Command}...");

            return Task.CompletedTask;
        }
    }

    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Send/Receive Consumer";
            Console.WriteLine("Starting Command Processor...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "nhatnguyen", h =>
                {
                    h.Username("sa");
                    h.Password("pass");
                });

                cfg.ReceiveEndpoint("command-queue", e =>
                {
                    e.Consumer<CommandConsumer>();
                });
            });

            await busControl.StartAsync();

            Console.WriteLine("Command processor started. Press Enter to exit");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }
}