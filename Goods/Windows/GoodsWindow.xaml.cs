using Goods.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Goods.Windows
{
    /// <summary>
    /// Логика взаимодействия для GoodsWindow.xaml
    /// </summary>
    /// 
    public class GoodViewModel
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
        public string MessureUnit { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string ImagePath { get; set; }
    }


    public partial class GoodsWindow : Window
    {
        public GoodsWindow()
        {
            InitializeComponent();

            if (GoodsContext.CurrentUser != null)
            {
                userNameTextBlock.Text = $"{GoodsContext.CurrentUser.LastName} {GoodsContext.CurrentUser.FirstName[0]}.{GoodsContext.CurrentUser.MiddleName?[0]}.";

                switch (GoodsContext.CurrentUser.Role.Name)
                {
                    case "Администратор":
                        controlPanelGrid.Visibility = Visibility.Visible;
                        break;
                    case "Менеджер":
                        controlPanelGrid.Visibility = Visibility.Visible;
                        break;
                }
            }

            LoadGoods();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoodsContext.CurrentUser = null;
            new MainWindow().Show();
            this.Close();
        }

        private void LoadGoods()
        {
            var goods = GoodsContext.Instance.Goods
                .Include(g => g.Category)
                .Include(g => g.Manufacturer)
                .Include(g => g.Supplier)
                .Include(g => g.MessureUnit)
                .ToList()
                .Select(g => new GoodViewModel
                {
                    Name = g.Name,
                    CategoryName = g.Category.Name,
                    Description = g.Description,
                    Manufacturer = g.Manufacturer.Name.Trim(),
                    Supplier = g.Supplier.Name.Trim(),
                    Price = g.Price,
                    MessureUnit = g.MessureUnit.Name.Trim(),
                    Quantity = g.QuantityInStock,
                    Discount = g.Discount,
                    ImagePath = GetImagePath(g.Picture)
                })
                .ToList();

            GoodsItemsControl.ItemsSource = goods;
        }

        private string GetImagePath(string? picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                return "/Resources/picture.png";

            return $"/Resources/{picture}";
        }


    }
}
