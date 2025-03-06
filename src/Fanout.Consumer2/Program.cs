// See husing RabbitMQ.Client;
using Model.Share;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost", Password = "pass", UserName = "sa", VirtualHost = "nhatnguyen" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: MyExchange.Fanount, type: ExchangeType.Fanout);
await channel.QueueDeclareAsync(MyQueue.FanoutQueue2,
    durable: true,      // Giữ queue tồn tại sau khi restart
    exclusive: false,   // Cho phép nhiều kết nối
    autoDelete: false   // Không tự động xóa khi không còn consumer
);

await channel.QueueBindAsync(queue: MyQueue.FanoutQueue2, exchange: MyExchange.Fanount, routingKey: string.Empty);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" {ea.Exchange} --- {ea.RoutingKey} ----- [Message] ::: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(MyQueue.FanoutQueue2, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();