using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using PizzariaApp.Data;
using PizzariaApp.Models;

namespace PizzariaApp.Views
{
    public partial class PainelCozinhaWindow : Window
    {
        private readonly PizzariaContext _context = new();
        private readonly ObservableCollection<PedidoCardViewModel> _pedidos = new();
        private readonly DispatcherTimer _timer;

        public PainelCozinhaWindow()
        {
            InitializeComponent();

            ListaPedidos.ItemsSource = _pedidos;

            // Timer de polling: atualiza a lista de pedidos periodicamente.
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += (_, _) => CarregarPedidos();
            _timer.Start();

            CarregarPedidos();
        }

        /// <summary>
        /// Busca no banco os pedidos ainda não entregues e atualiza a lista exibida.
        /// </summary>
        private void CarregarPedidos()
        {
            try
            {
                var pedidosDoBanco = _context.Pedidos
                    .AsNoTracking()
                    .Include(p => p.Cliente)
                    .Include(p => p.Itens)
                        .ThenInclude(i => i.Produto)
                    .Where(p => p.Status != StatusPedido.Entregue)
                    .OrderBy(p => p.DataHora)
                    .ToList();

                // Reconstrói a lista de ViewModels mantendo a experiência simples.
                _pedidos.Clear();
                foreach (var pedido in pedidosDoBanco)
                {
                    _pedidos.Add(new PedidoCardViewModel(pedido, AvancarStatus));
                }

                TxtUltimaAtualizacao.Text = $"Atualizado às {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar pedidos: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Atualiza o status do pedido diretamente no banco de dados.
        /// </summary>
        private void AvancarStatus(Pedido pedido, StatusPedido novoStatus)
        {
            try
            {
                var pedidoNoBanco = _context.Pedidos.First(p => p.Id == pedido.Id);
                pedidoNoBanco.Status = novoStatus;
                _context.SaveChanges();

                CarregarPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar status: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer.Stop();
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}
