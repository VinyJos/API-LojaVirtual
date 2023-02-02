using LojaVirtual.Models;
using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

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

        //[Route("CriarUsuario")]
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

        //[Route("EditarVerificacaoUsuario")]
        [HttpPut]
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

        //[Route("ListagemDeCategorias")]
        [HttpGet]
        public IActionResult ListagemDeCategorias()
        {
            var categoria = _repository.ListagemDeCategorias();

            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        //[Route("ProdutosPorCategoria")]
        [HttpGet("{url}")]
        public IActionResult ProdutosPorCategoria(string url)
        {
            var decodedUrl = Uri.UnescapeDataString(url);
            var listaProdutos = _repository.ProdutosPorCategoria(decodedUrl);

            if (listaProdutos.Count() <= 0)
            {
                return Ok("Categoria não disponível.");
                
            }
            else
            {
                return Ok(listaProdutos);
            }
        }
    }
}
