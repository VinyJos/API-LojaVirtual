using LojaVirtual.Models;
using System.Data;
using System.Data.SqlClient;

namespace LojaVirtual.Repositories
{
    public class CategoriaRepository : Interfaces.ICategoriaRepository
    {
        private IDbConnection _connection;
        public CategoriaRepository()
        {
            _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public List<Categoria> Get()
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
    }
}
