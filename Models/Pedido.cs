using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PizzariaApp.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public DateTime DataHora { get; set; } = DateTime.Now;

        public StatusPedido Status { get; set; } = StatusPedido.Pendente;

        public List<ItemPedido> Itens { get; set; } = new();

        [NotMapped]
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
    }
}
