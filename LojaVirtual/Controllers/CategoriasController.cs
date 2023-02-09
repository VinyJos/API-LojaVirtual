using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private LojaRepository _repository;

        public CategoriasController()
        {
            _repository = new LojaRepository();
        }

        //[Route("ListagemDeCategorias")]
        [HttpGet("ListagemDeCategoriasAtivas")]
        public IActionResult ListagemDeCategorias()
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
