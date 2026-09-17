using PizzariaApp.Models;

namespace PizzariaApp.Views
{
    /// <summary>
    /// Representa uma linha do carrinho de compras enquanto o pedido ainda
    /// não foi enviado/salvo no banco de dados.
    /// </summary>
    public class ItemCarrinho
    {
        public Produto Produto { get; set; } = null!;
        public TamanhoPizza Tamanho { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal => PrecoUnitario * Quantidade;

        public string TamanhoDescricao => Tamanho switch
        {
            TamanhoPizza.Pequena => "Pequena",
            TamanhoPizza.Media => "Média",
            TamanhoPizza.Grande => "Grande",
            _ => Tamanho.ToString()
        };

        public string DescricaoLinha => $"{Quantidade}x {Produto.Nome} ({TamanhoDescricao})";
    }
}
