using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


internal class Program
{
    public static async Task Main(string[] args)
    {
        await Consumer();
    }
    static async Task Consumer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>  // ea => event argument
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine("Received Message : " + message);
            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: "q1", autoAck: true, consumer: consumer);
        await channel.BasicConsumeAsync("q2", true, consumer);
        await channel.BasicConsumeAsync("q3", true, consumer);

        Console.WriteLine("Press enter to exist");
        Console.ReadKey();
    }
}