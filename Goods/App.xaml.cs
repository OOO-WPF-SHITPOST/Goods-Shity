using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Goods
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string resourcesPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources");

            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);
        }
    }

}
