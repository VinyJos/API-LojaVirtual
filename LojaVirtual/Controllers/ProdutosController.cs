using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private LojaRepository _repository;

        public ProdutosController()
        {
            _repository = new LojaRepository();
        }

        //[Route("ProdutosPorCategoria")]
        [HttpGet("ProdutosPorCategoria/{url}")]
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


        [HttpGet("GetProduto/{url}")]
        public IActionResult GetProduto(string url)
        {
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
