
public class ObterPedidoUseCase : IObterPedidoUseCase
{
    
    private readonly IPedidoRepository _pedidoRepository;

    public ObterPedidoUseCase(
        IPedidoRepository pedidoRepository
        )
    {
        _pedidoRepository = pedidoRepository;
    }
    public async Task<PedidoDto?> ExecuteAsync(Guid idPedido)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(idPedido);
        if (pedido is null)
            return null;

        return new PedidoDto(
            pedido.Id,
            pedido.IdCliente,
            pedido.Total,
            pedido.Status,
            pedido.Data,
            pedido.Itens.Select(i => new ItemPedidoDto(
                i.Id,
                i.PedidoId,
                i.Nome,
                i.Quantidade,
                i.PrecoUnitario,
                i.Valor
            ))
        );
    }
}