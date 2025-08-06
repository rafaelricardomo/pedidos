public class PedidoModel
{
    public Guid Id { get; set; }
    public Guid IdCliente { get; set; }
    public EStatusPedidoEnum Status { get; set; }
    public decimal Total { get; set; }
    public DateTime Data { get; set; }
    public List<ItemPedidoModel> Itens { get; set; } = new();
}