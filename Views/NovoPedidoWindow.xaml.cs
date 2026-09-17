using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using PizzariaApp.Data;
using PizzariaApp.Models;

namespace PizzariaApp.Views
{
    public partial class NovoPedidoWindow : Window
    {
        private readonly PizzariaContext _context = new();
        private readonly ObservableCollection<ItemCarrinho> _carrinho = new();

        public NovoPedidoWindow()
        {
            InitializeComponent();

            GridCarrinho.ItemsSource = _carrinho;
            _carrinho.CollectionChanged += (_, _) => AtualizarTotal();

            CarregarSabores();
        }

        /// <summary>
        /// Carrega os sabores de pizza cadastrados no banco de dados no ComboBox.
        /// </summary>
        private void CarregarSabores()
        {
            var produtos = _context.Produtos
                .Where(p => p.Ativo)
                .OrderBy(p => p.Nome)
                .ToList();

            CmbSabor.ItemsSource = produtos;
            if (produtos.Any())
                CmbSabor.SelectedIndex = 0;

            AtualizarPrecoUnitarioExibido();
        }

        private TamanhoPizza TamanhoSelecionado()
        {
            if (CmbTamanho.SelectedItem is ComboBoxItem item && item.Tag is string tag)
                return Enum.Parse<TamanhoPizza>(tag);
            return TamanhoPizza.Media;
        }

        private void CmbTamanho_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AtualizarPrecoUnitarioExibido();
        }

        private void AtualizarPrecoUnitarioExibido()
        {
            if (TxtPrecoUnitario == null) return; // ainda inicializando o XAML

            if (CmbSabor.SelectedItem is Produto produto)
            {
                var preco = produto.PrecoPorTamanho(TamanhoSelecionado());
                TxtPrecoUnitario.Text = $"Preço unitário: {preco:C}";
            }
            else
            {
                TxtPrecoUnitario.Text = string.Empty;
            }
        }

        private void AdicionarAoCarrinho_Click(object sender, RoutedEventArgs e)
        {
            if (CmbSabor.SelectedItem is not Produto produtoSelecionado)
            {
                MessageBox.Show("Selecione um sabor de pizza.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tamanho = TamanhoSelecionado();
            var preco = produtoSelecionado.PrecoPorTamanho(tamanho);

            _carrinho.Add(new ItemCarrinho
            {
                Produto = produtoSelecionado,
                Tamanho = tamanho,
                Quantidade = quantidade,
                PrecoUnitario = preco
            });

            TxtQuantidade.Text = "1";
        }

        private void RemoverItem_Click(object sender, RoutedEventArgs e)
        {
            if (GridCarrinho.SelectedItem is ItemCarrinho item)
                _carrinho.Remove(item);
        }

        private void AtualizarTotal()
        {
            var total = _carrinho.Sum(i => i.Subtotal);
            TxtValorTotal.Text = $"Total: {total:C}";
        }

        /// <summary>
        /// Valida os dados, grava (ou reaproveita) o cliente e cria o pedido com status PENDENTE.
        /// </summary>
        private void EnviarPedido_Click(object sender, RoutedEventArgs e)
        {
            var nome = TxtNomeCliente.Text.Trim();
            var telefone = TxtTelefoneCliente.Text.Trim();
            var endereco = TxtEnderecoCliente.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(telefone))
            {
                MessageBox.Show("Informe ao menos o nome e o telefone do cliente.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_carrinho.Any())
            {
                MessageBox.Show("Adicione ao menos uma pizza ao carrinho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Reaproveita o cadastro do cliente se o telefone já existir, senão cria um novo.
                var cliente = _context.Clientes.FirstOrDefault(c => c.Telefone == telefone);
                if (cliente == null)
                {
                    cliente = new Cliente { Nome = nome, Telefone = telefone, Endereco = endereco };
                    _context.Clientes.Add(cliente);
                }
                else
                {
                    cliente.Nome = nome;
                    cliente.Endereco = endereco;
                }

                var pedido = new Pedido
                {
                    Cliente = cliente,
                    DataHora = DateTime.Now,
                    Status = StatusPedido.Pendente
                };

                foreach (var itemCarrinho in _carrinho)
                {
                    pedido.Itens.Add(new ItemPedido
                    {
                        ProdutoId = itemCarrinho.Produto.Id,
                        Tamanho = itemCarrinho.Tamanho,
                        Quantidade = itemCarrinho.Quantidade,
                        PrecoUnitario = itemCarrinho.PrecoUnitario
                    });
                }

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                MessageBox.Show($"Pedido #{pedido.Id} enviado para a cozinha com sucesso!",
                    "Pedido enviado", MessageBoxButton.OK, MessageBoxImage.Information);

                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar o pedido: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimparFormulario()
        {
            TxtNomeCliente.Clear();
            TxtTelefoneCliente.Clear();
            TxtEnderecoCliente.Clear();
            _carrinho.Clear();
            AtualizarTotal();
        }

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }
    }
}
