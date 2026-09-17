using System.Linq;
using PizzariaApp.Models;

namespace PizzariaApp.Data
{
    /// <summary>
    /// Popula o banco com um cardápio inicial, caso ainda esteja vazio.
    /// </summary>
    public static class DbInitializer
    {
        public static void Seed(PizzariaContext context)
        {
            if (context.Produtos.Any())
                return; // já populado

            context.Produtos.AddRange(
                new Produto { Nome = "Mussarela", Descricao = "Molho, mussarela e orégano", PrecoPequena = 28.00m, PrecoMedia = 38.00m, PrecoGrande = 48.00m },
                new Produto { Nome = "Calabresa", Descricao = "Molho, mussarela, calabresa e cebola", PrecoPequena = 30.00m, PrecoMedia = 40.00m, PrecoGrande = 50.00m },
                new Produto { Nome = "Portuguesa", Descricao = "Presunto, ovos, cebola, ervilha e azeitona", PrecoPequena = 32.00m, PrecoMedia = 42.00m, PrecoGrande = 54.00m },
                new Produto { Nome = "Frango com Catupiry", Descricao = "Frango desfiado e catupiry", PrecoPequena = 34.00m, PrecoMedia = 44.00m, PrecoGrande = 56.00m },
                new Produto { Nome = "Quatro Queijos", Descricao = "Mussarela, provolone, parmesão e gorgonzola", PrecoPequena = 35.00m, PrecoMedia = 46.00m, PrecoGrande = 58.00m },
                new Produto { Nome = "Chocolate", Descricao = "Chocolate ao leite e granulado", PrecoPequena = 30.00m, PrecoMedia = 40.00m, PrecoGrande = 50.00m }
            );

            context.SaveChanges();
        }
    }
}
