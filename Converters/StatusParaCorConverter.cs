using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using PizzariaApp.Models;

namespace PizzariaApp.Converters
{
    /// <summary>
    /// Converte o status do pedido em uma cor de destaque para o painel da cozinha.
    /// </summary>
    public class StatusParaCorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusPedido status)
            {
                return status switch
                {
                    StatusPedido.Pendente => new SolidColorBrush(Color.FromRgb(0xE6, 0x39, 0x46)),          // vermelho
                    StatusPedido.EmPreparo => new SolidColorBrush(Color.FromRgb(0xF2, 0xA6, 0x00)),          // laranja
                    StatusPedido.ProntoParaEntrega => new SolidColorBrush(Color.FromRgb(0x2A, 0x9D, 0x8F)),  // verde
                    StatusPedido.Entregue => new SolidColorBrush(Color.FromRgb(0x99, 0x99, 0x99)),           // cinza
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Converte o enum StatusPedido em um texto amigável para exibição.
    /// </summary>
    public class StatusParaTextoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusPedido status)
            {
                return status switch
                {
                    StatusPedido.Pendente => "PENDENTE",
                    StatusPedido.EmPreparo => "EM PREPARO",
                    StatusPedido.ProntoParaEntrega => "PRONTO PARA ENTREGA",
                    StatusPedido.Entregue => "ENTREGUE",
                    _ => status.ToString()
                };
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
