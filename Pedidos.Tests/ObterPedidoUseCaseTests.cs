using Moq;

public class ObterPedidoUseCaseTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepository;

    public ObterPedidoUseCaseTests()
    {
        _pedidoRepository = new Mock<IPedidoRepository>();
    }

    private ObterPedidoUseCase GetUseCase()
        => new ObterPedidoUseCase(_pedidoRepository.Object);

    [Fact]
    public async Task Deve_retornar_pedido_existente()
    {

        var pedidoId = Guid.NewGuid();
        var esperado = new Pedido(
             pedidoId,
             idCliente: Guid.NewGuid(),
            DateTime.UtcNow,
            EStatusPedidoEnum.Recebido,
            new List<ItemPedido>
            {
                new ItemPedido (Guid.NewGuid(), pedidoId,  "Notebook",  2 , 1500m)
            }
        );

        _pedidoRepository
            .Setup(r => r.ObterPorIdAsync(pedidoId))
            .ReturnsAsync(esperado);

        var useCase = GetUseCase();

        // Act
        var resultado = await useCase.ExecuteAsync(pedidoId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pedidoId, resultado.Id);
        Assert.Equal(EStatusPedidoEnum.Recebido, resultado.Status);
        Assert.NotNull(resultado.Itens);
        Assert.True(resultado.Itens.Any());
    }

    [Fact]
    public async Task Deve_retornar_null_para_pedido_inexistente()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        _pedidoRepository
            .Setup(r => r.ObterPorIdAsync(pedidoId))
            .ReturnsAsync((Pedido?)null);

        var useCase = GetUseCase();

        // Act
        var resultado = await useCase.ExecuteAsync(pedidoId);

        // Assert
        Assert.Null(resultado);
    }
}
