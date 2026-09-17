using Microsoft.EntityFrameworkCore;
using PizzariaApp.Models;

namespace PizzariaApp.Data
{
  
    public class PizzariaContext : DbContext
    {
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

      
        private const string ConnectionString =
            "Server=10.1.20.242;Port=3306;Database=vinicius;User=vinicius.fusao;Password=VinF@2047;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var versaoServidor = ServerVersion.AutoDetect(ConnectionString);
                optionsBuilder.UseMySql(ConnectionString, versaoServidor);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.Tamanho)
                .HasConversion<string>();
        }
    }
}
