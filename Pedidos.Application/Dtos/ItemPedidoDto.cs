public record ItemPedidoDto(
    Guid Id,
    Guid PedidoId, 
    string Nome,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Valor
);