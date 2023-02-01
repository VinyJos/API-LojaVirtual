using System.Reflection.Metadata.Ecma335;

namespace LojaVirtual.Models
{
    
    public class Usuario
    {
        private bool _Ativo;
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? ChaveVerificacao { get; set; }
        public string? LastToken { get; set; }
        public bool IsVerificado { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get => _Ativo; set => _Ativo = false; }
    }
}
