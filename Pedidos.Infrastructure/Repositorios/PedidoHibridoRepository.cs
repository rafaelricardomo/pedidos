
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
        await _sqlRepo.AdicionarAsync(pedido);
        await _mongoRepo.AdicionarAsync(pedido.Itens);
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
