using System.Windows;
using PizzariaApp.Data;

namespace PizzariaApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Garante que o banco de dados (SQLite) exista e já tenha os produtos iniciais.
            using (var context = new PizzariaContext())
            {
                context.Database.EnsureCreated();
                DbInitializer.Seed(context);
            }
        }
    }
}
