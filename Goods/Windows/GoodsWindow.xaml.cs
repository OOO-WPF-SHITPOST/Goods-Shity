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
        public string Number { get; set; }
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

            controlPanelGrid.Visibility = Visibility.Hidden;
            controlPanelScrollViewer.Visibility = Visibility.Hidden;

            if (GoodsContext.CurrentUser != null)
            {
                userNameTextBlock.Text = $"{GoodsContext.CurrentUser.LastName} {GoodsContext.CurrentUser.FirstName[0]}.{GoodsContext.CurrentUser.MiddleName?[0]}.";

                switch (GoodsContext.CurrentUser.Role.Name)
                {
                    case "Администратор":
                        controlPanelGrid.Visibility = Visibility.Visible;
                        controlPanelScrollViewer.Visibility = Visibility.Visible;
                        break;
                    case "Менеджер":
                        controlPanelScrollViewer.Visibility = Visibility.Visible;
                        controlPanelGrid.Visibility = Visibility.Visible;
                        deleteButton.Visibility = Visibility.Hidden;
                        addButton.Visibility = Visibility.Hidden;
                        editButton.Visibility = Visibility.Hidden;
                        break;
                }
            }

            nameComboBoxItem.IsSelected = true;
            defaultSortTypeItem.IsSelected = true;
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
            var searchText = searchTextBox.Text;
            var sortField = columnForSortComboBox.Text;
            var sortType = sortTypeComboBox.Text;
            List<Good> goods = null;

            if (searchText != "")
            {
                goods = GoodsContext.Instance.Goods
               .Include(g => g.Category)
               .Include(g => g.Manufacturer)
               .Include(g => g.Supplier)
               .Include(g => g.MessureUnit)
               .Where(g => g.Number.Contains(searchText)
                    || g.Name.Contains(searchText)
                    || g.Price.ToString().Contains(searchText)
                    || g.Supplier.Name.Contains(searchText)
                    || g.Manufacturer.Name.Contains(searchText)
                    || g.Category.Name.Contains(searchText)
                    || g.Discount.ToString().Contains(searchText)
                    || g.QuantityInStock.ToString().Contains(searchText)
                    || g.Description.Contains(searchText))
               .ToList();
               
            } else
            {
                goods = GoodsContext.Instance.Goods
               .Include(g => g.Category)
               .Include(g => g.Manufacturer)
               .Include(g => g.Supplier)
               .Include(g => g.MessureUnit)
               .ToList();
            }

            if (goods != null && goods.Count > 1)
            {
                switch (sortField)
                {
                    case "название":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.Name).ToList() : goods.OrderBy(g => g.Name).ToList();
                        break;
                    case "цена":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.Price).ToList() : goods.OrderBy(g => g.Price).ToList();
                        break;
                    case "поставщик":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.Supplier.Name).ToList() : goods.OrderBy(g => g.Supplier.Name).ToList();
                        break;
                    case "производитель":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.Manufacturer.Name).ToList() : goods.OrderBy(g => g.Manufacturer.Name).ToList();
                        break;
                    case "категория":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.Category.Name).ToList() : goods.OrderBy(g => g.Category.Name).ToList();
                        break;
                    case "скидка":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.Discount).ToList() : goods.OrderBy(g => g.Discount).ToList();
                        break;
                    case "количество":
                        goods = (sortType == "возрастанию") ? goods.OrderByDescending(g => g.QuantityInStock).ToList() : goods.OrderBy(g => g.QuantityInStock).ToList();
                        break;
                }
            }

            if (goods != null)
            {
                GoodsListView.ItemsSource = goods
                .Select(g => new GoodViewModel
                {
                    Number = g.Number,
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
                .ToList(); ;
            }
        }

        private string GetImagePath(string? picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                return "/Resources/picture.png";

            return $"/Resources/{picture}";
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadGoods();
        }

        private void columnForSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGoods();
        }

        private void sortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGoods();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedGood = GoodsListView.SelectedItem as GoodViewModel;
            
            if (selectedGood != null)
            {
                var good = GoodsContext.Instance.Goods.Where(g => g.Number == selectedGood.Number).FirstOrDefault();

                if (GoodsContext.Instance.GoodsOrders.Where(go => go.GoodNumber == good.Number).ToList().Count > 0)
                {
                    MessageBox.Show("Товар содержится в заказе!");
                    return;
                }

                GoodsContext.Instance.Goods.Remove(good);
                GoodsContext.Instance.SaveChanges();
                LoadGoods();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new AddGoodWindow().ShowDialog();
            LoadGoods();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var selectedGood = GoodsListView.SelectedItem as GoodViewModel;

            if (selectedGood != null)
            {
                new EditGoodWindow(selectedGood.Number).ShowDialog();
                LoadGoods();
            }
        }
    }
}
