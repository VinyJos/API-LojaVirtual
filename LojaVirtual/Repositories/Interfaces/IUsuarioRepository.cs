using LojaVirtual.Models;

namespace LojaVirtual.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        public void Post(Usuario usuario); // Post Usuario - cria usuario
        public Usuario Update(Usuario usuario); // Update usuario - edita a verificação do usuário
        public string AutenticaUsuario(int id, string login, string senha); // Update - autentica usuario
        public Usuario VerificaUsuarioPorId(int usuarioId); //Post pedido - verifica usuario por id
    }
}
