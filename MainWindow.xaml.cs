using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<Товары> products = new();    
        private string _sortParam = "";
        private string _supplierFilter = "Все поставщики";
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            LoadSuppliers();

            private void LoadData() 
            {
                using (var context = new AppDbContext())
            
                {
                    var products = context.Товары
                                        .Include(p => p.ВкКатегорииNavigation)
                                        .Include(p => p.ВкПроизводителиNavigation)
                                        .Include(p => p.ВкПоставщикиNavigation)
                                        .ToList();

                    ApplyFilterAndSort();   
                }  
            }
            private void LoadSuppliers()
            {
                var suppliers = products
                .Select(p => p.ВкПоставщикиNavigation.Наименование)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .OrderBy(n => n)
                .ToList();
            }
             private void RadioUpp_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;

            if (radio.Content.ToString() == "по возрастанию")
            {
                SortParam = "по возрастанию";
            }
            else if (radio.Content.ToString() == "по убыванию")
            {
                SortParam = "по убыванию";
            }
            Sort();
        }
        //Фильтрация
        private void ComboSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedItem != null)
            {
                List<string> conv = box.SelectedItem.ToString().Split().ToList();

                FiltParam = box.SelectedItem.ToString();
            }
            Sort();
        }
        public void Sort()
        {
            products = context.Products.Include(q => q.Supplier)
                .Include(q => q.Manufacturer)
                .Include(q => q.Name)
                .Include(q => q.Category)
                .ToList();
            //фильтрация
            try
            {
                products = products.Where(q =>
                (q.Description?.Contains(BoxFind.Text) ?? false)
                || (q.Article?.Contains(BoxFind.Text) ?? false)
                || (q.Name?.Name?.Contains(BoxFind.Text) ?? false)
                ).Where(q => q.Discount >= Convert.ToDouble(FiltParam.Split().ToList()[0]) && q.Discount <= Convert.ToDouble(FiltParam.Split().ToList()[1])).ToList();
            }
            catch
            {
                products = products.Where(q =>
                (q.Description?.Contains(BoxFind.Text) ?? false)
                || (q.Article?.Contains(BoxFind.Text) ?? false)
                || (q.Name?.Name?.Contains(BoxFind.Text) ?? false)
                ).Where(q => FiltParam == "все диапазоны").ToList();
            }
            //сортировка по цене
            if (SortParam == "по возрастанию")
            {
                products = products.OrderBy(q => (q.Price / 100) * (100 - q.Discount)).ToList();
            }
            else if (SortParam == "по убыванию")
            {
                products = products.OrderByDescending(q => (q.Price / 100) * (100 - q.Discount)).ToList();
            }
            //сортировка по количеству на складе 

            DrawProductItem(products);
        }

    }
    }
}