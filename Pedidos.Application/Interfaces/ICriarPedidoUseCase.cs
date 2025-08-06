public interface ICriarPedidoUseCase
{
    Task<CriarPedidoSaidaDto> ExecuteAsync(CriarPedidoDto pedidoDto);
}