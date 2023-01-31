﻿using LojaVirtual.Models;
using System.Data;
using System.Data.SqlClient;

namespace LojaVirtual.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public UsuarioRepository()
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
