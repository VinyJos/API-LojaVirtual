using LojaVirtual.Models;
using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosControllers : ControllerBase
    {
        private UsuarioRepository _repository;

        public UsuariosControllers()
        {
            _repository= new UsuarioRepository();
        }


        [HttpPost]
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

        [HttpPut]
        public IActionResult EditarVerificacaoUsuario(Usuario usuario)
        {
            try
            {
                _repository.EditarVerificacaoUsuario(usuario);
                return Ok(usuario);
            }
            catch (Exception a)
            {

                return BadRequest(a.Message);

            }



        }
    }
}
