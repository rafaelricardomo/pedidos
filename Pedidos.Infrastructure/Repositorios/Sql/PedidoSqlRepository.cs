using Dapper;
using System.Data.SqlClient;

public interface IPedidoSqlRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Pedido pedido);
}
public class PedidoSqlRepository : IPedidoSqlRepository
{
    private readonly string _connectionString;

    public PedidoSqlRepository(SqlConfiguration configuration)
    {
        _connectionString = configuration.ConnectionString;
    }

    public async Task AdicionarAsync(Pedido pedido)
    {
        const string query = @"INSERT INTO Pedidos (Id, IdCliente, Total, Status, Data) VALUES (@Id, @IdCliente, @Total, @Status, @Data)";

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            query,
            new { pedido.Id, pedido.IdCliente, pedido.Total, pedido.Status, pedido.Data }
            );

        await connection.CloseAsync();

    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var pedido = await connection.QueryFirstOrDefaultAsync<Pedido>(
            "SELECT Id, IdCliente, Total, Status, Data FROM Pedidos WHERE Id = @Id;",
            new { Id = id });
        await connection.CloseAsync();
        return pedido;
    }

    public async Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var pedidos = await connection.QueryAsync<Pedido>("SELECT Id, IdCliente, Total, Status, Data FROM Pedidos;");
        await connection.CloseAsync();
        return pedidos;
    }
}