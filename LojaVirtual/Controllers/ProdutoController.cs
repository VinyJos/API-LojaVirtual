using LojaVirtual.Models;
using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private LojaRepository _repository;

        public ProdutoController()
        {
            _repository = new LojaRepository();
        }

        


        [HttpGet("GetProduto/{url}")]
        public IActionResult GetProduto(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var decodedUrl = Uri.UnescapeDataString(url);
            var produto = _repository.GetProduto(decodedUrl);


            if (produto == null)
            {
                return NotFound("Erro na Url");
            }
            else if (produto.Ativo == false)
            {
                return Ok("Não está disponível");
            }
            else
            {
                return Ok(produto);
            }
        }

        
    }
}
