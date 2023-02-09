using LojaVirtual.Models;
using LojaVirtual.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private LojaRepository _repository;

        public PedidoController()
        {
            _repository = new LojaRepository();
        }

        [HttpPost("NovoPedido")]
        public IActionResult NovoPedido([FromBody]List<Produto> produtos, int ususarioId)
        {
            // VERIFICA SE ESTÁ NULO
            if (produtos == null && ususarioId == null)
            {
                return NotFound();
            }
            else
            {
                var usuario = _repository.VerificaUsuarioPorId(ususarioId);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado");
                }
                else if (usuario.LastToken == null)
                {
                    return Unauthorized("Usuário não autenticado");
                }
                else
                {
                    // VERIFICA A QUANTIDADE DE PRODUTO
                    foreach (var produto in produtos!)
                    {
                        var produtoBanco = _repository.VerificaProduto(produto.Id);

                        
                        if (produtoBanco.Nome == null)
                        {
                            return NotFound("Produto não existe");
                        }
                        else if (produtoBanco.Quantidade <= 0)
                        {
                            return BadRequest($"Quantidade não disponível, do produto : {produto.Id}");
                        }
                        else if (produtoBanco.Quantidade < produto.Quantidade)
                        {
                            return BadRequest($"Quantidade não disponível, do produto : Id = {produto.Id}, Nome = {produtoBanco.Nome}, Quantidade = {produtoBanco.Quantidade}");
                        }
                        else if (produtoBanco.Ativo == false || produtoBanco.Excluido == true)
                        {
                            return BadRequest("Produto não Ativo ou excluído");
                        }
                        else
                        {
                            continue;
                        }
                       
                    }
                    // GERAR PEDIDO
                    var pedido = _repository.GerarPedido(usuario.Id);

                    // GERAR PEDIDOITEM
                    foreach (var item in produtos)
                    {
                        var pedidoItem = _repository.GerarPedidoItem(pedido.Id, item.Id, item.Quantidade);
                    }

                    return Ok(pedido);
                }
            }

            
        }

        [HttpGet("ListaDePedidos")] 
        public IActionResult ListaPedidos()
        {
            return Ok(_repository.ListaPedidos());
        }

    }
}
