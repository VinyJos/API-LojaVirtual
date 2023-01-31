using LojaVirtual.Models;
using System.Data;
using System.Data.SqlClient;

namespace LojaVirtual.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public UsuarioRepository()
        {
            SqlConnection
        }

        public void CriarUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void EditarVerificacaoUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public List<Categoria> ListagemDeCategorias()
        {
            throw new NotImplementedException();
        }

        public List<Produto> ProdutosPorCategoria(string url)
        {
            throw new NotImplementedException();
        }
        public Produto GetProduto(string url)
        {
            throw new NotImplementedException();
        }
        public void AutenticaUsuario()
        {
            throw new NotImplementedException();
        }

        public void NovoPedido(Pedido pedido)
        {
            throw new NotImplementedException();
        }
        public List<Pedido> ListaPedidos()
        {
            throw new NotImplementedException();
        }

    }
}
