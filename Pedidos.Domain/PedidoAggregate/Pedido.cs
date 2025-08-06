public class Pedido
{
    public Guid Id { get; }
    public Guid IdCliente { get; }
    public DateTime Data { get; }
    public EStatusPedidoEnum Status { get; private set; }
    public List<ItemPedido> Itens { get; private set; }
    public decimal Total => Itens?.Sum(i => i.Valor) ?? 0;
    public Pedido(Guid idCliente)
    {
        Id = Guid.NewGuid();
        Data = DateTime.Now;
        Status = EStatusPedidoEnum.Recebido;
        IdCliente = idCliente;
    }

    public Pedido(Guid id, Guid idCliente, DateTime data, EStatusPedidoEnum status, List<ItemPedido> itens)
    {
        Id = id;
        IdCliente = idCliente;
        Data = data;
        Status = status;
        Itens = itens;
    }

    protected Pedido() { }

    public void AdicionarItem(string nome, int quantidade, decimal precoUnitario)
    {
        var item = new ItemPedido(Id, nome, quantidade, precoUnitario);
        if (Itens is null)
            Itens = new List<ItemPedido>();
        Itens.Add(item);
    }
    public void CarregarItens(List<ItemPedido> itens)
    {
        if (Itens is null)
            Itens = new List<ItemPedido>();

        Itens.Clear();
        Itens.AddRange(itens);
    }

    public void MarcarStatusComoEnviado()
    {
        Status = EStatusPedidoEnum.Enviado;
    }
}