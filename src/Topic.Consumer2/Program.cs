using Model.Share;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost", Password = "pass", UserName = "sa", VirtualHost = "nhatnguyen" };

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: MyExchange.Topic, type: ExchangeType.Topic);

await channel.QueueDeclareAsync(queue: MyQueue.TopicQueue2, durable: false, exclusive: false, autoDelete: false);

await channel.QueueBindAsync(queue: MyQueue.TopicQueue2, exchange: MyExchange.Topic, routingKey: "*.important.*");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" {ea.Exchange} --- {ea.RoutingKey} ----- [Message] ::: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(MyQueue.TopicQueue2, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();