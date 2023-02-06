using LojaVirtual.Models;

namespace LojaVirtual.Repositories
{
    public interface ILojaRepository
    {
        // Métodos principais
        public void CriarUsuario(Usuario usuario); // Post Usuario
        public Usuario EditarVerificacaoUsuario(Usuario usuario); // Update usuario
        public List<Categoria> ListagemDeCategorias(); // Get categoria
        public List<Produto> ProdutosPorCategoria(string url); // Get com ULR
        public Produto GetProduto(string url); // Get Produto 
        public string AutenticaUsuario(int id, string login, string senha); // Update 
        public Usuario VerificaUsuarioPorId(int usuarioId); //Post pedido
        public List<Pedido> ListaPedidos(); // Get pedidos

    }
}
