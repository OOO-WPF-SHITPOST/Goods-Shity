using Goods.Models;
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
    /// Логика взаимодействия для AddGoodWindow.xaml
    /// </summary>
    public partial class AddGoodWindow : Window
    {
        private string? selectedImageFileName;
        private readonly string resourcesPath;

        public AddGoodWindow()
        {
            InitializeComponent();

            resourcesPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources");

            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            CategoryComboBox.ItemsSource = GoodsContext.Instance.Categories.ToList();
            ManufacturerComboBox.ItemsSource = GoodsContext.Instance.Manufacturers.ToList();
            SupplierComboBox.ItemsSource = GoodsContext.Instance.Suppliers.ToList();
            MessureUnitComboBox.ItemsSource = GoodsContext.Instance.MessureUnits.ToList();

            // Заглушка
            string placeholder = Path.Combine(resourcesPath, "picture.png");
            if (File.Exists(placeholder))
                GoodImage.Source = new BitmapImage(new Uri(placeholder, UriKind.Absolute));
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

                selectedImageFileName = Path.GetFileName(dialog.FileName);

                File.Copy(
                    dialog.FileName,
                    Path.Combine(resourcesPath, selectedImageFileName),
                    true);

                GoodImage.Source = bmp;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (GoodsContext.Instance.Goods
                .Any(g => g.Number == NumberTextBox.Text))
            {
                MessageBox.Show("Товар с таким артикулом уже существует");
                return;
            }

            Good good = new Good
            {
                Number = NumberTextBox.Text,
                Name = NameTextBox.Text,
                CategoryId = (int)CategoryComboBox.SelectedValue,
                ManufacturerId = (int)ManufacturerComboBox.SelectedValue,
                SupplierId = (int)SupplierComboBox.SelectedValue,
                MessureUnitId = (int)MessureUnitComboBox.SelectedValue,
                Price = decimal.Parse(PriceTextBox.Text),
                Discount = int.Parse(DiscountTextBox.Text),
                QuantityInStock = int.Parse(QuantityTextBox.Text),
                Description = DescriptionTextBox.Text,
                Picture = selectedImageFileName
            };

            GoodsContext.Instance.Goods.Add(good);
            GoodsContext.Instance.SaveChanges();

            DialogResult = true;
        }
    }
}
