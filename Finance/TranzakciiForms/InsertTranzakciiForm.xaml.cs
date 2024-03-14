using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Finance.TranzakciiForms
{
    /// <summary>
    /// Логика взаимодействия для InsertTranzakciiForm.xaml
    /// </summary>
    public partial class InsertTranzakciiForm : Window
    {
        public InsertTranzakciiForm()
        {
            InitializeComponent();

            var allTypes = App.Context.C7KategoriPengeluaran.ToList();
            allTypes.Insert(0, new C7KategoriPengeluaran
            {
                Name = "Все категории"
            });
            kategoriiComboBox.SelectedIndex = 0;
            kategoriiComboBox.ItemsSource = allTypes;
        }

        private void savedButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
