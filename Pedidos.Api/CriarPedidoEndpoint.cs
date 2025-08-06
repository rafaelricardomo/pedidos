public static class CriarPedidoEndpoint
{
    public static void MapCriarPedidoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/pedidos", async (
            ICriarPedidoUseCase criarPedidoUseCase,
            CriarPedidoDto pedido) =>
        {
           var result = await criarPedidoUseCase.ExecuteAsync(pedido);
            return result.sucesso ? Results.Created(string.Empty,result.idPedido.ToString()) : Results.UnprocessableEntity("Pedido não processado.");
        })
        .WithName("CriarPedido")
        .WithOpenApi()
        .Produces(201);
    }
}