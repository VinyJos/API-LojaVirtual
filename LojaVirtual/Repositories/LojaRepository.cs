using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;

namespace LojaVirtual.Repositories
{
    public class LojaRepository : ILojaRepository
    {
        private IDbConnection _connection;

        public LojaRepository()
        {
            _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public void CriarUsuario(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Usuario(Nome, Login, Email, Senha, ChaveVerificacao, IsVerificado, Ativo, Excluido) VALUES (@Nome, @Login, @Email, @Senha, @ChaveVerificacao, @IsVerificado, @Ativo, @Excluido); SELECT CAST(scope_identity() AS int)";
                command.Connection = (SqlConnection)_connection;

                Guid guid= Guid.NewGuid();

                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Login", usuario.Login);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Senha", usuario.Senha);
                command.Parameters.AddWithValue("@ChaveVerificacao", usuario.ChaveVerificacao);
                command.Parameters.AddWithValue("@IsVerificado", usuario.IsVerificado);
                command.Parameters.AddWithValue("@Ativo", usuario.Ativo);
                command.Parameters.AddWithValue("@Excluido", usuario.Excluido);

                _connection.Open();
                usuario.Id = (int)command.ExecuteScalar();
            }
            finally
            {

                _connection.Close();
            }
        }

        public Usuario EditarVerificacaoUsuario(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = "UPDATE Usuario SET IsVerificado = @IsVerificado WHERE Email = @Email AND ChaveVerificacao = @ChaveVerificacao";
                command.Connection = (SqlConnection)_connection;

                _connection.Open();
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@ChaveVerificacao", usuario.ChaveVerificacao);
                command.Parameters.AddWithValue("@IsVerificado", usuario.IsVerificado);

                command.ExecuteNonQuery();
                return usuario;
                


                // -----------------------------------------------------

            }
            finally
            {

                _connection.Close();
            }
            
        }

        public List<Categoria> ListagemDeCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Categoria INNER JOIN Produto ON Categoria.Id = Produto.CategoriaId WHERE Quantidade > 0 AND Produto.Ativo = 1";
                command.Connection = (SqlConnection)_connection;

                

                _connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Categoria categoria1 = new Categoria();
                    categoria1.Id = dataReader.GetInt32("Id");
                    categoria1.Nome = dataReader.GetString("Nome");
                    categoria1.Url = dataReader.GetString("Url");
                    categoria1.Ativo = dataReader.GetBoolean("Ativo");
                    categoria1.Excluido = dataReader.GetBoolean("Excluido");

                    if (categorias.FirstOrDefault(a => a.Id == categoria1.Id) == null)
                    {

                        categorias.Add(categoria1); // atribuindo valor de endereços 

                    }

                }

                
            }
            finally
            {

                _connection.Close();
            }

            return categorias;
        }

        public List<Produto> ProdutosPorCategoria(string url)
        {
            List<Produto> produtos = new List<Produto>();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Categoria INNER JOIN  Produto ON Categoria.Id = Produto.CategoriaId WHERE Categoria.Url = @Url";
                command.Parameters.AddWithValue("@Url", url);
                command.Connection = (SqlConnection)_connection;

                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();


                while (dataReader.Read())
                {
                    Categoria categoria = new Categoria();
                    Produto produto = new Produto();

                    categoria.Ativo = dataReader.GetBoolean(3);

                    if (categoria.Ativo == false)
                    {
                        return null;
                    }
                    else
                    {
                        
                        produto.Id = dataReader.GetInt32(5);
                        produto.CategoriaId = dataReader.GetInt32(0);
                        produto.Nome = dataReader.GetString(7);
                        produto.Url = dataReader.GetString(8);
                        produto.Quantidade = dataReader.GetInt32(9);
                        produto.Ativo = dataReader.GetBoolean(10);
                        produto.Excluido = dataReader.GetBoolean(11);

                        produtos.Add(produto);
                        Console.WriteLine(produto.Nome);
                    }

                    
                }
                
                
            }
            finally
            {
               
                _connection.Close();
            }
           
            return produtos;
        }

        public Produto GetProduto(string url)
        {
            try
            {

                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Produto WHERE Produto.Url = @Url";
                command.Parameters.AddWithValue("@Url", url);

                command.Connection = (SqlConnection)_connection;

                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                dataReader.Read();
                Produto produto = new Produto();

                produto.Id = dataReader.GetInt32(0);
                produto.CategoriaId = dataReader.GetInt32(1);
                produto.Nome = dataReader.GetString(2);
                produto.Url = dataReader.GetString(3);
                produto.Quantidade = dataReader.GetInt32(4);
                produto.Ativo = dataReader.GetBoolean(5);
                produto.Excluido = dataReader.GetBoolean(6);



                return produto;

            }
            catch (Exception)
            {
                return null;
            }
            finally 
            {

                _connection.Close();
            }

            
        }
        public string AutenticaUsuario(int id, string login, string senha)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuario WHERE Id = @Id AND Login = @Login AND Senha = @Senha";

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Senha", senha);

                command.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = dataReader.GetInt32("Id"); // ==
                    usuario.Nome = dataReader.GetString("Nome");
                    usuario.Login = dataReader.GetString("Login"); // ==
                    usuario.Email = dataReader.GetString("Email");
                    usuario.Senha = dataReader.GetString("Senha"); // ===
                    usuario.ChaveVerificacao = dataReader.GetString("ChaveVerificacao");
                    usuario.IsVerificado = dataReader.GetBoolean("IsVerificado");
                    usuario.Ativo = dataReader.GetBoolean("Ativo");
                    usuario.Excluido = dataReader.GetBoolean("Excluido");

                    // Verifica se já não foi gerado o Token 
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastToken")))
                        usuario.LastToken = dataReader.GetString("LastToken");
                    else
                        usuario.LastToken = null;
                   

                    if (usuario.LastToken == null)
                    {
                        if (
                            usuario.IsVerificado == true &&
                            usuario.Ativo == true  &&
                            usuario.Excluido == false 
                            )
                        {
                            // GERAR TOKEN
                            var token = LojaVirtual.Sevices.TokenService.GerarToken(usuario);

                            // ADICIONA NO BANCO
                            _connection.Close();
                            _connection.Open();
                            command.Parameters.Clear();

                            command.CommandText = "UPDATE Usuario SET LastToken = @LastToken WHERE Id = @Id";
                            command.Parameters.AddWithValue("@Id", usuario.Id);
                            command.Parameters.AddWithValue("@LastToken", token);


                            command.ExecuteNonQuery();

                            return "OKTokenGerado";

                        }
                        else
                        {
                            return "NaoVerificado";
                        }
                    }
                    else
                    {
                        return "LastTokenJaContem";
                    }
                }
                else
                {
                    return "nulo";
                }
            }
            finally
            {

                _connection.Close();
            }
        }

        public Usuario VerificaUsuarioPorId(int usuarioId)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuario WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", usuarioId);

                command.Connection = (SqlConnection)_connection;

                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = dataReader.GetInt32("Id");

                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("LastToken")))
                        usuario.LastToken = dataReader.GetString("LastToken");
                    else
                        usuario.LastToken = null;

                    return usuario;

                }
                else
                {
                    return null;
                }
                


            }
            finally
            {

                _connection.Close();
            }
        }
        public List<ListaPedido> ListaPedidos()
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

        public string VerificaUsuario(string login)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = $"SELECT Login FROM Usuario WHERE Login = @Login";
                command.Parameters.AddWithValue("@Login", login);

                command.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    int loginIndex = dataReader.GetOrdinal("Login");
                    var log = dataReader.GetString(loginIndex).ToString();
                    return log;
                }
                else
                {
                    return null;
                }


            }
            finally
            {

                _connection.Close();
            }

           
        }

        public List<string> VerificaUsuarioValidacao(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuario WHERE Email = @Email AND ChaveVerificacao = @ChaveVerificacao";
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@ChaveVerificacao", usuario.ChaveVerificacao);

                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                var log = new List<string>();
                if (dataReader.Read())
                {
                    int email = dataReader.GetOrdinal("Email");
                    int chave = dataReader.GetOrdinal("ChaveVerificacao");

                    log.Add(dataReader.GetString(email).ToString());
                    log.Add(dataReader.GetString(chave).ToString());

                    return log;
                    
                }
                else
                {
                    log.Add("null");
                    log.Add("null");
                    return log;
                }
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

        public Produto VerificaProduto(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Produto WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                command.Connection = (SqlConnection)_connection;
                _connection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                Produto produto = new Produto();
                dataReader.Read();
                
                    
                produto.Id = dataReader.GetInt32(0);
                produto.CategoriaId = dataReader.GetInt32(1);
                produto.Nome = dataReader.GetString(2);
                produto.Url = dataReader.GetString(3);
                produto.Quantidade = dataReader.GetInt32(4);
                produto.Ativo = dataReader.GetBoolean(5);
                produto.Excluido = dataReader.GetBoolean(6);

                return produto;
                
            }
            catch (Exception ex)
            {
                // Tratar a exceção aqui
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {

                _connection.Close();
            }


        }
    }
}
