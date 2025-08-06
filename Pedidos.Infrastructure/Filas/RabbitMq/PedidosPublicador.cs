

using Microsoft.Extensions.Options;

public class PedidosPublicador : RabbitMqPublisher<Pedido>, IPedidosPublicador
{
    public PedidosPublicador(
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration
        ) 
        : base(rabbitMqConfiguration.Value.Hostname,
        rabbitMqConfiguration.Value.Username,
        rabbitMqConfiguration.Value.Password,
         "pedidos-queue")
    {
    }
}