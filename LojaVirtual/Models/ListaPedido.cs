namespace LojaVirtual.Models
{
    public class ListaPedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataPedido { get; set; }
        public ICollection<PedidoItem>? ListaProdutos { get; set; }
        
    }
}
