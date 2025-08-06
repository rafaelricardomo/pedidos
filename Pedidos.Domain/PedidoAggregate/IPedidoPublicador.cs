public interface IPedidosPublicador
{
    Task PublicarAsync(Pedido pedido);
}