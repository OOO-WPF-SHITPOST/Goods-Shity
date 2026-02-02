using Goods.Models;
using Goods.Windows;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Goods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var login = loginTextBox.Text;
            var password = passwordBox.Password;

            var user = GoodsContext.Instance.Users
                .Include(u => u.Role)
                .Where(u => u.Login == login && u.Password == password)
                .FirstOrDefault();
            if (user != null)
            {
                GoodsContext.CurrentUser = user;

                new GoodsWindow().Show();
                this.Close();
            } else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new GoodsWindow().Show();
            this.Close();
        }
    }
}