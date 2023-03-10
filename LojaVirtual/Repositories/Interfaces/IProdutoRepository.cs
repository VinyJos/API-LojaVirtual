using LojaVirtual.Models;

namespace LojaVirtual.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        public List<Produto> Get(string url); // Get com ULR categoria
        public Produto GetProduto(string url); // Get Produto 
    }
}
