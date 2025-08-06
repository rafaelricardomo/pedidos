public interface IObterPedidoUseCase
{
    Task<PedidoDto?> ExecuteAsync(Guid idPedido);
}