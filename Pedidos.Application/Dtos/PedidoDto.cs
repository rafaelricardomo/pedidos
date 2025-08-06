public record PedidoDto(
    Guid Id,
    Guid IdCliente,
    decimal Total,
    EStatusPedidoEnum Status,
    DateTime Data,
    IEnumerable<ItemPedidoDto> Itens
    );