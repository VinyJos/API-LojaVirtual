namespace LojaVirtual.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public Usuario UsuarioId { get; set; }
        public DateTime DataPedido { get; set; }

    }
}
