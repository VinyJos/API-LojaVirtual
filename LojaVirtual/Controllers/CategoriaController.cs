using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private LojaRepository _repository;

        public CategoriaController()
        {
            _repository = new LojaRepository();
        }

   
        [HttpGet("ProdutosPorCategoria/{url}")]
        public IActionResult ProdutosPorCategoria(string url)
        {

            if (url == null)
            {
                return NotFound();
            }
            
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

        //[Route("ListagemDeCategorias")]
        [HttpGet("ListagemDeCategoriasAtivas")]
        public IActionResult ListagemDeCategoriasAtivas()
        {
            var categoria = _repository.ListagemDeCategorias();

            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }
    }
}
