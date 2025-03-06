using Model.Share;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost", Password = "pass", UserName = "sa", VirtualHost = "nhatnguyen" };

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();


// Bind queue với exchange sử dụng headers
var headers = new Dictionary<string, object>
            {
                { "type", "audit" },
                { "department", "IT" }
            };

// Sử dụng x-match=all để match tất cả headers
headers.Add("x-match", "all");

await channel.ExchangeDeclareAsync(exchange: MyExchange.Header, type: ExchangeType.Headers);

await channel.QueueDeclareAsync(queue: MyQueue.HeaderQueue2, durable: false, exclusive: false, autoDelete: false);

await channel.QueueBindAsync(queue: MyQueue.HeaderQueue2, exchange: MyExchange.Header, routingKey: string.Empty, arguments: headers!);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" {ea.Exchange} --- {ea.RoutingKey} ----- [Message] ::: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(MyQueue.HeaderQueue2, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();