using LojaVirtual.Models;
using System.Data;
using System.Data.SqlClient;

namespace LojaVirtual.Repositories
{
    public class PedidoRepository : Interfaces.IPedidoRepository
    {
        private IDbConnection _connection;
        public PedidoRepository()
        {
            _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public List<ListaPedido> Get()
        {
            List<ListaPedido> pedidos = new List<ListaPedido>();
            //List<PedidoItem> produtos = new List<PedidoItem>();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Pedido INNER JOIN PedidoItem ON Pedido.Id = PedidoItem.PedidoId";

                command.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();



                while (dataReader.Read())
                {
                    int pedidoId = dataReader.GetInt32(0);
                    ListaPedido listaPedido = pedidos.FirstOrDefault(p => p.Id == pedidoId);

                    if (listaPedido == null)
                    {
                        listaPedido = new ListaPedido();
                        listaPedido.Id = pedidoId;
                        listaPedido.UsuarioId = dataReader.GetInt32(1);
                        listaPedido.DataPedido = dataReader.GetDateTime(2);
                        listaPedido.ListaProdutos = new List<PedidoItem>();
                        pedidos.Add(listaPedido);
                    }





                    PedidoItem pedidoItem = new PedidoItem();
                    pedidoItem.Id = dataReader.GetInt32(3);
                    pedidoItem.PedidoId = dataReader.GetInt32(4);
                    pedidoItem.ProdutoId = dataReader.GetInt32(5);
                    pedidoItem.Quantidade = dataReader.GetInt32(6);


                    listaPedido.ListaProdutos.Add(pedidoItem);




                }

                return pedidos;



            }
            finally
            {

                _connection.Close();
            }
        }

        public Pedido GerarPedido(int usuarioId)
        {
            try
            {
                Pedido pedido = new Pedido();
                pedido.UsuarioId = usuarioId;
                pedido.DataPedido = DateTime.Now;

                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Pedido (UsuarioId, DataPedido) VALUES (@UsuarioId, @DataPedido); SELECT CAST(scope_identity() AS int)";
                command.Parameters.AddWithValue("@UsuarioId", pedido.UsuarioId);
                command.Parameters.AddWithValue("@DataPedido", pedido.DataPedido);

                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                pedido.Id = (int)command.ExecuteScalar();

                return pedido;
            }
            finally
            {

                _connection.Close();
            }
        }

        public PedidoItem GerarPedidoItem(int pedidoId, int produtoId, int quantidade)
        {
            try
            {
                PedidoItem pedidoItem = new PedidoItem();
                pedidoItem.PedidoId = pedidoId;
                pedidoItem.ProdutoId = produtoId;
                pedidoItem.Quantidade = quantidade;

                SqlCommand command = new SqlCommand();
                command.CommandText = "UPDATE Produto SET Quantidade = Quantidade - @Quantidade WHERE Id = @ProdutoId; INSERT INTO PedidoItem (PedidoId, ProdutoId, Quantidade) VALUES (@PedidoId, @ProdutoId, @Quantidade); SELECT CAST(scope_identity() AS int)";
                command.Parameters.AddWithValue("@PedidoId", pedidoItem.PedidoId);
                command.Parameters.AddWithValue("@ProdutoId", pedidoItem.ProdutoId);
                command.Parameters.AddWithValue("@Quantidade", pedidoItem.Quantidade);

                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                pedidoItem.Id = (int)command.ExecuteScalar();

                return pedidoItem;
            }
            finally
            {

                _connection.Close();
            }

        }
    }
}
