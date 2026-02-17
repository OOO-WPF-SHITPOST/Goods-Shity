using Goods.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Goods.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditGoodWindow.xaml
    /// </summary>
    public partial class EditGoodWindow : Window
    {
        private readonly Good currentGood;
        private string? newImageFileName;
        private readonly string resourcesPath;

        public EditGoodWindow(string goodNumber)
        {
            InitializeComponent();

            resourcesPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources");

            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            currentGood = GoodsContext.Instance.Goods
                .First(g => g.Number == goodNumber);

            CategoryComboBox.ItemsSource = GoodsContext.Instance.Categories.ToList();
            ManufacturerComboBox.ItemsSource = GoodsContext.Instance.Manufacturers.ToList();
            SupplierComboBox.ItemsSource = GoodsContext.Instance.Suppliers.ToList();
            MessureUnitComboBox.ItemsSource = GoodsContext.Instance.MessureUnits.ToList();

            NumberTextBox.Text = currentGood.Number;
            NameTextBox.Text = currentGood.Name;
            PriceTextBox.Text = currentGood.Price.ToString();
            DiscountTextBox.Text = currentGood.Discount.ToString();
            QuantityTextBox.Text = currentGood.QuantityInStock.ToString();
            DescriptionTextBox.Text = currentGood.Description;

            CategoryComboBox.SelectedValue = currentGood.CategoryId;
            ManufacturerComboBox.SelectedValue = currentGood.ManufacturerId;
            SupplierComboBox.SelectedValue = currentGood.SupplierId;
            MessureUnitComboBox.SelectedValue = currentGood.MessureUnitId;

            string imgPath = string.IsNullOrWhiteSpace(currentGood.Picture)
                ? Path.Combine(resourcesPath, "picture.png")
                : Path.Combine(resourcesPath, currentGood.Picture);

            if (File.Exists(imgPath))
                GoodImage.Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute));
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                BitmapImage bmp = new BitmapImage(new Uri(dialog.FileName));

                if (bmp.PixelWidth > 2000 || bmp.PixelHeight > 2000)
                {
                    MessageBox.Show("Максимальный размер изображения 300x200 px");
                    return;
                }

                newImageFileName = Path.GetFileName(dialog.FileName);

                File.Copy(
                    dialog.FileName,
                    Path.Combine(resourcesPath, newImageFileName),
                    true);

                GoodImage.Source = bmp;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            currentGood.Name = NameTextBox.Text;
            currentGood.Price = decimal.Parse(PriceTextBox.Text);
            currentGood.Discount = int.Parse(DiscountTextBox.Text);
            currentGood.QuantityInStock = int.Parse(QuantityTextBox.Text);
            currentGood.Description = DescriptionTextBox.Text;

            currentGood.CategoryId = (int)CategoryComboBox.SelectedValue;
            currentGood.ManufacturerId = (int)ManufacturerComboBox.SelectedValue;
            currentGood.SupplierId = (int)SupplierComboBox.SelectedValue;
            currentGood.MessureUnitId = (int)MessureUnitComboBox.SelectedValue;

            if (newImageFileName != null)
                currentGood.Picture = newImageFileName;

            GoodsContext.Instance.SaveChanges();
            DialogResult = true;
        }
    }
}
