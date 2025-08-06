public record CriarPedidoDto(
    Guid IdCliente,    
    List<CriarItemPedidoDto> Itens
    );