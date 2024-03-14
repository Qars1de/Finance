using Finance.TranzakciiForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Finance.BalanceForms
{
    /// <summary>
    /// Логика взаимодействия для BalanceForm.xaml
    /// </summary>
    public partial class BalanceForm : Window
    {
        public BalanceForm()
        {
            InitializeComponent();
            Update();
        }

        private void Update()
        {
            DB db = new DB();



            SqlCommand command = new SqlCommand("select Balance from[7Balance] where UserID = '" + App.Current.Resources["login"] + "'", db.getConnection());
            SqlCommand command2 = new SqlCommand("select LastName, FirstName, Patronymic from [7User] where UserID = '" + App.Current.Resources["login"] + "'", db.getConnection());

            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            SqlDataReader reader2 = command2.ExecuteReader();

            while (reader.Read())
            {
                int balance = reader.GetInt32(0);
                summaLabel.Content = Convert.ToString(balance);
            }

            while (reader2.Read())
            {
                string LastName = reader2.GetString(0);
                string FirstName = reader2.GetString(1);
                string Patronymic = reader2.GetString(2);
                fioLabel.Content = LastName + " " + FirstName + " " + Patronymic;
            }
            db.closeConnection();
        }
        private void tranzakciiButton_Click(object sender, RoutedEventArgs e)
        {
            TranzakciiForm form = new TranzakciiForm();

            if (form.ShowDialog() == true)
            {
                Update();
            }
        }
    }
}
