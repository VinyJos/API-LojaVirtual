using LojaVirtual.Models;
using System.Data;
using System.Data.SqlClient;

namespace LojaVirtual.Repositories
{
    public class UsuarioRepository : Interfaces.IUsuarioRepository
    {

        private IDbConnection _connection;
        public UsuarioRepository()
        {
            _connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LojaVirtual;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public void Post(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Usuario(Nome, Login, Email, Senha, ChaveVerificacao, IsVerificado, Ativo, Excluido) VALUES (@Nome, @Login, @Email, @Senha, @ChaveVerificacao, @IsVerificado, @Ativo, @Excluido); SELECT CAST(scope_identity() AS int)";
                command.Connection = (SqlConnection)_connection;

                Guid guid = Guid.NewGuid();

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
        public Usuario Update(Usuario usuario)
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
                            usuario.Ativo == true &&
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
    }
}
