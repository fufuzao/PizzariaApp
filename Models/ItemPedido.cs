using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzariaApp.Models
{
    /// <summary>
    /// Um item (linha) dentro de um pedido: um sabor, em um tamanho e quantidade específicos.
    /// </summary>
    public class ItemPedido
    {
        [Key]
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public TamanhoPizza Tamanho { get; set; }

        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; }

        [NotMapped]
        public decimal Subtotal => PrecoUnitario * Quantidade;

        [NotMapped]
        public string DescricaoResumida =>
            $"{Quantidade}x Pizza {Tamanho} - {Produto?.Nome} (R$ {Subtotal:0.00})";
    }
}
