using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

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

    }
}
