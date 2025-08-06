using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

public class PedidoRepositoryCache : IPedidoRepository
{
    private readonly IPedidoRepository _inner;
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _cacheOptions;

    public PedidoRepositoryCache(IPedidoRepository inner, IDistributedCache cache)
    {
        _inner = inner;
        _cache = cache;
        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };
    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id)
    {
       var cacheKey = $"pedido:{id}";

        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached is not null)
        {
            var dto = JsonSerializer.Deserialize<PedidoModel>(cached);
            return MapToDomain(dto!);
        }

        var pedido = await _inner.ObterPorIdAsync(id);
        if (pedido is null) return null;

        var modelToCache = MapToModel(pedido);
        var serialized = JsonSerializer.Serialize(modelToCache);

        await _cache.SetStringAsync(cacheKey, serialized, _cacheOptions);

        return pedido;
    }

    public Task AdicionarAsync(Pedido pedido) => _inner.AdicionarAsync(pedido);

    private PedidoModel MapToModel(Pedido pedido) =>
        new()
        {
            Id = pedido.Id,
            IdCliente = pedido.IdCliente,
            Data = pedido.Data,
            Status = pedido.Status,
            Itens = pedido.Itens.Select(i => new ItemPedidoModel
            {
                Id = i.Id,
                Nome = i.Nome,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };

    private Pedido MapToDomain(PedidoModel model) =>
        new Pedido(        
             model.Id,
             model.IdCliente,
           model.Data,
            model.Status,
            model.Itens.Select(i => new ItemPedido
            (
                i.Id,
                model.Id,
                i.Nome,
                i.Quantidade,
                i.PrecoUnitario
            )).ToList()
        );

}
