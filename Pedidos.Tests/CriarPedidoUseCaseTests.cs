using Moq;

public class CriarPedidoUseCaseTests
{

    private readonly Mock<IPedidosPublicador> _pedidosPublicador;
    private readonly Mock<IPedidoRepository> _pedidoRepository;

    public CriarPedidoUseCaseTests()
    {
        _pedidosPublicador = new Mock<IPedidosPublicador>();
        _pedidoRepository = new Mock<IPedidoRepository>();
    }

    private CriarPedidoUseCase GetUseCase()
        => new CriarPedidoUseCase(_pedidoRepository.Object, _pedidosPublicador.Object);

    [Fact]
    public async Task Deve_criar_um_pedido_com_sucesso()
    {
        var dto = new CriarPedidoDto
        (
            Guid.NewGuid(),
            new List<CriarItemPedidoDto>
            {
                new CriarItemPedidoDto ("Teclado", 2, 89.90m ),
                new CriarItemPedidoDto ("Mouse", 1, 19.90m )
            }
        );

        var useCase = GetUseCase();
        // Act
        var result = await useCase.ExecuteAsync(dto);

        Assert.NotNull(result);
        Assert.True(result.sucesso);
        Assert.NotNull(result.idPedido);

        // Assert
        _pedidoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
        _pedidosPublicador.Verify(r => r.PublicarAsync(It.IsAny<Pedido>()), Times.Once);
    }

    [Fact]
    public async Task Nao_deve_criar_um_pedido_com_erro_na_quantidade()
    {
        var dto = new CriarPedidoDto
        (
            Guid.NewGuid(),
            new List<CriarItemPedidoDto>
            {
                new CriarItemPedidoDto ("Teclado", 0, 89.90m ),
                new CriarItemPedidoDto ("Mouse", 0, 19.90m )
            }
        );

        var useCase = GetUseCase();
        // Act
        var result = await useCase.ExecuteAsync(dto);

        Assert.NotNull(result);
        Assert.False(result.sucesso);
        Assert.Null(result.idPedido);
    }

    [Fact]
    public async Task Nao_deve_criar_um_pedido_com_erro_no_preco()
    {
        var dto = new CriarPedidoDto
        (
            Guid.NewGuid(),
            new List<CriarItemPedidoDto>
            {
                new CriarItemPedidoDto ("Teclado", 1, 89.90m ),
                new CriarItemPedidoDto ("Mouse", 1, 0m )
            }
        );

        var useCase = GetUseCase();
        // Act
        var result = await useCase.ExecuteAsync(dto);

        Assert.NotNull(result);
        Assert.False(result.sucesso);
        Assert.Null(result.idPedido);
    }

    [Fact]
    public async Task Nao_deve_criar_um_pedido_com_erro_sem_itens()
    {
        var dto = new CriarPedidoDto
        (
            Guid.NewGuid(),
            new List<CriarItemPedidoDto>
            {
                
            }
        );

        var useCase = GetUseCase();
        // Act
        var result = await useCase.ExecuteAsync(dto);

        Assert.NotNull(result);
        Assert.False(result.sucesso);
        Assert.Null(result.idPedido);
    }

    [Fact]
    public async Task Nao_deve_criar_um_pedido_com_erro_inesperado()
    {
        var dto = new CriarPedidoDto
        (
            Guid.NewGuid(),
            new List<CriarItemPedidoDto>
            {
                new CriarItemPedidoDto ("Teclado", 2, 89.90m ),
                new CriarItemPedidoDto ("Mouse", 2, 19.90m )
            }
        );

        _pedidoRepository
            .Setup(x => x.AdicionarAsync(It.IsAny<Pedido>()))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var useCase = GetUseCase();
        // Act
        var result = await useCase.ExecuteAsync(dto);

        Assert.NotNull(result);
        Assert.False(result.sucesso);
        Assert.Null(result.idPedido);
    }
}
