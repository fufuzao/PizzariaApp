using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using PizzariaApp.Converters;
using PizzariaApp.Models;

namespace PizzariaApp.Views
{
    /// <summary>
    /// Envolve um Pedido com informações prontas para exibição e os comandos
    /// de avançar o status, usados pelos botões do painel da cozinha.
    /// </summary>
    public class PedidoCardViewModel : INotifyPropertyChanged
    {
        public Pedido Pedido { get; }

        public PedidoCardViewModel(Pedido pedido, Action<Pedido, StatusPedido> avancarStatus)
        {
            Pedido = pedido;

            ComandoEmPreparo = new RelayCommand(
                _ => avancarStatus(Pedido, StatusPedido.EmPreparo),
                _ => Pedido.Status == StatusPedido.Pendente);

            ComandoPronto = new RelayCommand(
                _ => avancarStatus(Pedido, StatusPedido.ProntoParaEntrega),
                _ => Pedido.Status == StatusPedido.EmPreparo);
        }

        public int Id => Pedido.Id;
        public string NomeCliente => Pedido.Cliente?.Nome ?? "(cliente não informado)";
        public string Telefone => Pedido.Cliente?.Telefone ?? string.Empty;
        public string Endereco => Pedido.Cliente?.Endereco ?? string.Empty;
        public string HoraPedido => Pedido.DataHora.ToString("HH:mm");
        public StatusPedido Status => Pedido.Status;
        public decimal ValorTotal => Pedido.ValorTotal;

        public string ItensResumo => string.Join("\n", Pedido.Itens.Select(i => "• " + i.DescricaoResumida));

        public ICommand ComandoEmPreparo { get; }
        public ICommand ComandoPronto { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotificarAtualizacao()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
