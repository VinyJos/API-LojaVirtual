namespace LojaVirtual.Models
{
    public class PedidoItem
    {
        public int Id { get; set; }
        public Pedido PedidoId { get; set; }
        public Produto ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
