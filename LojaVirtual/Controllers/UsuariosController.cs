using LojaVirtual.Models;
using LojaVirtual.Repositories;
using LojaVirtual.Sevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private LojaRepository _repository;

        public UsuariosController()
        {
            _repository= new LojaRepository();
        }

      
        [HttpPost("CriarUsuario")]
        public IActionResult CriarUsuario([FromBody]Usuario usuario)
        {
           
            var loginUsuario = _repository.VerificaUsuario(usuario.Login);
            
            if ( loginUsuario != usuario.Login)
            {
                Guid guid = Guid.NewGuid();

                usuario.ChaveVerificacao = guid.ToString();
                usuario.IsVerificado = false;
                usuario.Ativo = true;
                usuario.Excluido = false;


                _repository.CriarUsuario(usuario);

                return Ok(usuario);

            }
            else
            {
                return NotFound("Usuário já existe");
            }

            

        }

       
        [HttpPut("VerificaUsuario")]
        public IActionResult EditarVerificacaoUsuario(Usuario usuario)
        {
            
           
            var user = _repository.VerificaUsuarioValidacao(usuario);

            if (user[0] == usuario.Email && user[1] == usuario.ChaveVerificacao)
            {
                usuario.IsVerificado = true;
                var retorno = _repository.EditarVerificacaoUsuario(usuario);
                return Ok(retorno.IsVerificado);
            }
            else
            {
                return NotFound("Usuário não encontrado");
            }


        }

        [HttpPut("AutenticaUsuario")]
        public IActionResult AutenticaUsuario(int id, string login, string senha)
        {
            if (id != null && login != null && senha != null)
            {
                var resposta = _repository.AutenticaUsuario(id, login, senha);

                switch (resposta)
                {
                    case "OKTokenGerado" : return Ok(); break;
                    case "NaoVerificado": return BadRequest("Usuário não verificado,ou não ativo, ou excluído!"); break;
                    case "LastTokenJaContem" : return BadRequest("Usuário já estava autenticado!"); break;
                    case "nulo" : return BadRequest("Usuário não encontrado"); break;
                    default: return NotFound(); break;
                }
            }
            return BadRequest();
        }


    }
}
