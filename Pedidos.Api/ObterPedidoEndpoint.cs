public static class ObterPedidoEndpoint
{
    public static void MapObterPedidoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/pedidos/{idPedido}", async (
            IObterPedidoUseCase obterPedidoUseCase,
            Guid  idPedido) =>
        {
           var pedido = await obterPedidoUseCase.ExecuteAsync(idPedido);
            return Results.Ok(pedido);
        })
        .WithName("ObterPedido")
        .WithOpenApi()
        .Produces(200);
    }
}