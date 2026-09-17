using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzariaApp.Models
{
    /// <summary>
    /// Representa um sabor de pizza disponível no cardápio, com preço por tamanho.
    /// </summary>
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Descricao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoPequena { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoMedia { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoGrande { get; set; }

        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Retorna o preço do produto de acordo com o tamanho escolhido.
        /// </summary>
        public decimal PrecoPorTamanho(TamanhoPizza tamanho)
        {
            return tamanho switch
            {
                TamanhoPizza.Pequena => PrecoPequena,
                TamanhoPizza.Media => PrecoMedia,
                TamanhoPizza.Grande => PrecoGrande,
                _ => PrecoMedia
            };
        }

        public override string ToString() => Nome;
    }
}
