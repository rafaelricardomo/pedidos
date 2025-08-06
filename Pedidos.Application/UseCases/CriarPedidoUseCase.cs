

public class CriarPedidoUseCase : ICriarPedidoUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPedidosPublicador _pedidosPublicador;

    public CriarPedidoUseCase(
        IPedidoRepository pedidoRepository,
        IPedidosPublicador pedidosPublicador
        )
    {
        _pedidoRepository = pedidoRepository;
        _pedidosPublicador = pedidosPublicador;
    }
    public async Task<CriarPedidoSaidaDto> ExecuteAsync(CriarPedidoDto pedidoDto)
    {
        try
        {
            var novoPedido = new Pedido(pedidoDto.IdCliente);

            foreach (var item in pedidoDto.Itens)
                novoPedido.AdicionarItem(item.Nome, item.Quantidade, item.PrecoUnitario);

            if (novoPedido.Itens == null || !novoPedido.Itens.Any())
                throw new Exception("Pedido sem itens.");

            await _pedidoRepository.AdicionarAsync(novoPedido);
            await _pedidosPublicador.PublicarAsync(novoPedido);
            
            return new CriarPedidoSaidaDto(true, novoPedido.Id);
        }
        catch 
        {
            return new CriarPedidoSaidaDto(false);
        }
    }
}