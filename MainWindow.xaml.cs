using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {

        private List<Товары> _allProducts = new();
        private string _sortParam = "";
        private string _supplierFilter = "Все поставщики";

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            LoadSuppliers();
        }

        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                _allProducts = context.Товары
                    .Include(p => p.ВкКатегорииNavigation)
                    .Include(p => p.ВкПроизводителиNavigation)
                    .Include(p => p.ВкПоставщикиNavigation)
                    .ToList();
            }
            ApplyFilterAndSort();
        }

        private void LoadSuppliers()
        {
            var suppliers = _allProducts
                .Select(p => p.ВкПоставщикиNavigation.Наименование)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            suppliers.Insert(0, "Все поставщики");
            ComboSuppliers.ItemsSource = suppliers;
            ComboSuppliers.SelectedIndex = 0;
        }

        private void BoxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void ComboSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSuppliers.SelectedItem is string selected)
            {
                _supplierFilter = selected;
                ApplyFilterAndSort();
            }
        }

        private void RadioSort_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio && radio.Tag is string tag)
            {
                _sortParam = tag;
                ApplyFilterAndSort();
            }
        }

        private void ApplyFilterAndSort()
        {
            if (ProductList == null) return;

            IEnumerable<Товары> query = _allProducts;

            string search = BoxFind?.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    (p.Описание?.Contains(search, System.StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Артикул?.Contains(search, System.StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Наименование?.Contains(search, System.StringComparison.OrdinalIgnoreCase) ?? false));
            }

            if (!string.IsNullOrEmpty(_supplierFilter) && _supplierFilter != "Все поставщики")
            {
                query = query.Where(p =>
                    p.ВкПоставщикиNavigation?.Наименование == _supplierFilter);
            }

            if (_sortParam == "Asc")    
            {
                query = query.OrderBy(p => (p.Цена ?? 0) * (100 - (p.Скидка ?? 0)) / 100);
            }
            else if (_sortParam == "Desc")
            {
                query = query.OrderByDescending(p => (p.Цена ?? 0) * (100 - (p.Скидка ?? 0)) / 100);
            }

            ProductList.ItemsSource = query.ToList();
        }

        private void CardProduct_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            WindowEditing windowEditing = new WindowEditing();
            windowEditing.ShowDialog(); 
        }
    }
}