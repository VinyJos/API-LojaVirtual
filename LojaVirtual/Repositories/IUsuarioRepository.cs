﻿using LojaVirtual.Models;

namespace LojaVirtual.Repositories
{
    public interface IUsuarioRepository
    {
        public void CriarUsuario(Usuario usuario); // Post Usuario
        public void EditarVerificacaoUsuario(Usuario usuario); // Update usuario
        public List<Categoria> ListagemDeCategorias(); // Get categoria
        public List<Produto> ProdutosPorCategoria(string url); // Get com ULR
        public Produto GetProduto(string url); // Get Produto 
        public void AutenticaUsuario(); // Update 
        public void NovoPedido(Pedido pedido); //Post pedido
        public List<Pedido> ListaPedidos(); // Get pedidos

    }
}
