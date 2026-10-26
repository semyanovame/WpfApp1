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
using System.Xml.Linq;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class WindowEditing : Window
    {
        public WindowEditing(List<Поставщики> поставщики, List<Производители> производители, Товары товар = null)
        {
            InitializeComponent();
            if (товар != null)
            {
                TxtName.Text = товар.Наименование;
                TxtPrice.Text = товар.Цена?.ToString();
                ComboSupplier.SelectedValue = товар.ВкПоставщики;
                ComboManufacturer.SelectedValue = товар.ВкПроизводители;
            }
;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) || ComboSupplier.SelectedValue == null) return;

            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
