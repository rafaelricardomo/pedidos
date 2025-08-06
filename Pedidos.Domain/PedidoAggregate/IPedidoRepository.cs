public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Pedido pedido);
}