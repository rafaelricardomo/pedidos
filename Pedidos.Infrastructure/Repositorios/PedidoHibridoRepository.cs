
public class PedidoHibridoRepository : IPedidoRepository
{
    private readonly IPedidoSqlRepository _sqlRepo;
    private readonly IItemPedidoMongoRepository _mongoRepo;

    public PedidoHibridoRepository(IPedidoSqlRepository sqlRepo, IItemPedidoMongoRepository mongoRepo)
    {
        _sqlRepo = sqlRepo;
        _mongoRepo = mongoRepo;
    }

    public async Task AdicionarAsync(Pedido pedido)
    {

        bool pedidoCriado = false;
        bool itensInseridos = false;

        try
        {

            await _sqlRepo.AdicionarAsync(pedido);
            pedidoCriado = true;

            await _mongoRepo.AdicionarAsync(pedido.Itens);
            itensInseridos = true;

            pedido.MarcarStatusComoRecebido();
            await _sqlRepo.AtualizarStatusAsync(pedido);
        }
        catch
        {
            if (itensInseridos)
                await _mongoRepo.RemoverPorIdPedidoAsync(pedido.Id);

            if (pedidoCriado)
            {
                pedido.MarcarStatusComoCancelado();
                await _sqlRepo.AtualizarStatusAsync(pedido);
            }

            throw;
        }
    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id)
    {
        var pedido = await _sqlRepo.ObterPorIdAsync(id);
        if (pedido is null) return null;

        var itens = await _mongoRepo.ListarPorIdPedidoAsync(id);
        pedido.CarregarItens(itens);

        return pedido;
    }

}
