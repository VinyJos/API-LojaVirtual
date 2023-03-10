using LojaVirtual.Models;
using System.Data;
using System.Data.SqlClient;

namespace LojaVirtual.Repositories
{
    public class ProdutoRepository : Interfaces.IProdutoRepository
    {
        private IDbConnection _connection;
        public ProdutoRepository()
        {
            _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public List<Produto> Get(string url)
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

            return produtos; ;
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
