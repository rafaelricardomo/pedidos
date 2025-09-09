using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

public interface IItemPedidoMongoRepository
{
    Task<List<ItemPedido>?> ListarPorIdPedidoAsync(Guid pedidoId);
    Task AdicionarAsync(List<ItemPedido> itensPedido);
    Task RemoverPorIdPedidoAsync(Guid pedidoId);
}
public class ItemPedidoMongoRepository : IItemPedidoMongoRepository
{
    private readonly IMongoCollection<ItemPedido> _collection;

    public ItemPedidoMongoRepository(MongoConfiguration mongoConfiguration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var client = new MongoClient(mongoConfiguration.ConnectionString);
        var database = client.GetDatabase("ItensPedidosDb");
        _collection = database.GetCollection<ItemPedido>("ItensPedidos");
    }

    public async Task AdicionarAsync(List<ItemPedido> itensPedido)
    {
        await _collection.InsertManyAsync(itensPedido);
    }

    public async Task<List<ItemPedido>?> ListarPorIdPedidoAsync(Guid pedidoId)
    {
        return await _collection.Find(x => x.PedidoId == pedidoId).ToListAsync();
    }
    
     public async Task RemoverPorIdPedidoAsync(Guid pedidoId)
    {
        await _collection.DeleteManyAsync(x => x.PedidoId == pedidoId);
    }

}