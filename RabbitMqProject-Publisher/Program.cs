using System.Text;
using RabbitMQ.Client;


internal class Program
{
    public static async Task Main(string[] args)
    {
        await GenerateLog();
    }
    static async Task GenerateLog()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        for (int i = 0; i < 1000; i++)
        {
            Thread.Sleep(900);
            var message = "";
            byte[] body;
            if (i % 2 == 0)
            {
                message = $"Log Info {i}";
                body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync("amq.topic", "log.info", body);
            }
            else
            {
                message = $"Log Error {i}";
                body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync("amq.topic", "log.Error", body);
            }
            Console.WriteLine("Send message is : " + message);
        }
        Console.WriteLine("Press enter to exit");
        Console.ReadKey();
    }
}