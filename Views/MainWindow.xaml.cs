using System.Windows;

namespace PizzariaApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AbrirNovoPedido_Click(object sender, RoutedEventArgs e)
        {
            var tela = new NovoPedidoWindow();
            tela.Show();
        }

        private void AbrirPainelCozinha_Click(object sender, RoutedEventArgs e)
        {
            var tela = new PainelCozinhaWindow();
            tela.Show();
        }
    }
}
