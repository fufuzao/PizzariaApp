using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzariaApp.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Endereco { get; set; } = string.Empty;

        public List<Pedido> Pedidos { get; set; } = new();
    }
}
