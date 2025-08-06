using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

public class RabbitMqPublisher<T> where T : class
{
    private readonly string _hostname;
    private readonly string _username;
    private readonly string _password;
    private readonly string _queue;

    public RabbitMqPublisher(string hostname, string username, string password, string queue)
    {
        _hostname = hostname;
        _username = username;
        _password = password;
        _queue = queue;
    }

    public async Task PublicarAsync(T mensagem)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);
        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent 
        };


        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: _queue,
            mandatory: false,
            basicProperties: props,
            body: body);
    }
}
