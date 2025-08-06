public class ItemPedido
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; }
    public string Nome { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public decimal Valor => CalcularValor(Quantidade, PrecoUnitario);
    
    public ItemPedido(Guid pedidoId, string nome, int quantidade, decimal precoUnitario)
    {
        Validar(quantidade, precoUnitario);
       
        PedidoId  = pedidoId;
        Id = Guid.NewGuid();
        Nome = nome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public ItemPedido(Guid id, Guid pedidoId, string nome, int quantidade, decimal precoUnitario)
    {
        PedidoId  = pedidoId;
        Id = id;
        Nome = nome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    private void Validar(int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida");
        if (precoUnitario <= 0)
            throw new ArgumentException("Preço inválido");

    }
    private decimal CalcularValor(int quantidade, decimal precoUnitario)
    {
        return precoUnitario * quantidade;
    }
}