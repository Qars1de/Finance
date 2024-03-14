using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для TranzakciiForm.xaml
    /// </summary>
    public partial class TranzakciiForm : Window
    {
        public TranzakciiForm()
        {
            InitializeComponent();
            Update();
        }

        private void Update()
        {
            var tranzakcii = App.Context.C7Tranzakcii.ToList();
            tranzakcii = tranzakcii.Where(p => p.UserID.ToString().ToLower().Contains(App.Current.Resources["login"].ToString().ToLower())).ToList();
            TranzakciiGrid.ItemsSource = tranzakcii;
        }

        private void addedButton_Click(object sender, RoutedEventArgs e)
        {
            InsertTranzakciiForm form = new InsertTranzakciiForm();
            StringBuilder errors = new StringBuilder();

            if (form.ShowDialog() == true)
            {
                if (string.IsNullOrWhiteSpace(form.summaBox.Text))
                    errors.AppendLine("Введите сумму");
                if (form.kategoriiComboBox.SelectedIndex == 0)
                    errors.AppendLine("Укажите категорию");
                if (string.IsNullOrWhiteSpace(form.datePicker.Text))
                    errors.AppendLine("Выберите дату произведения операции");
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }
                else
                {
                    DB db = new DB();
                    if (form.kategoriiComboBox.Text == "Пополнение")
                    {
                        SqlCommand command2 = new SqlCommand("update [7Balance] set Balance =Balance + '" + form.summaBox.Text + "' where UserID = '" + App.Current.Resources["login"] + "'", db.getConnection());

                        SqlCommand command = new SqlCommand("insert into [7Tranzakcii] (UserID, KategoriID, Data, Summa) select @user, CategoriID, @data, @summa from [7KategoriPengeluaran] where [7KategoriPengeluaran].Name = @kat", db.getConnection());
                        command.Parameters.Add("@user", SqlDbType.Int).Value = App.Current.Resources["login"];
                        command.Parameters.Add("@kat", SqlDbType.VarChar).Value = form.kategoriiComboBox.Text;
                        command.Parameters.Add("@summa", SqlDbType.Int).Value = form.summaBox.Text;
                        command.Parameters.Add("@data", SqlDbType.Date).Value = form.datePicker.Text;
                        db.openConnection();
                        command.ExecuteNonQuery();
                        command2.ExecuteNonQuery();
                        Update();
                        db.closeConnection();
                    }
                    else
                    {
                        int balance;
                        SqlCommand command2 = new SqlCommand("select Balance from [7Balance] where UserID = '" + App.Current.Resources["login"] + "'", db.getConnection());
                        db.openConnection();
                        SqlDataReader reader = command2.ExecuteReader();
                        while (reader.Read())
                        {
                            App.Current.Resources["balance"] = reader.GetInt32(0);
                        }

                        balance = Convert.ToInt32(App.Current.Resources["balance"]);

                        db.closeConnection();

                        if (balance < Convert.ToInt32(form.summaBox.Text))
                        {
                            MessageBox.Show("На счету меньше средств чем указано в создании операции");
                        }
                        else
                        {
                            SqlCommand command = new SqlCommand("insert into [7Tranzakcii] (UserID, KategoriID, Data, Summa) select @user, CategoriID, @data, @summa from [7KategoriPengeluaran] where [7KategoriPengeluaran].Name = @kat", db.getConnection());
                            SqlCommand command3 = new SqlCommand("update [7Balance] set Balance =Balance - '" + form.summaBox.Text + "' where UserID = '" + App.Current.Resources["login"] + "'", db.getConnection());
                            command.Parameters.Add("@user", SqlDbType.Int).Value = App.Current.Resources["login"];
                            command.Parameters.Add("@kat", SqlDbType.VarChar).Value = form.kategoriiComboBox.Text;
                            command.Parameters.Add("@summa", SqlDbType.Int).Value = form.summaBox.Text;
                            command.Parameters.Add("@data", SqlDbType.Date).Value = form.datePicker.Text;

                            db.openConnection();
                            command.ExecuteNonQuery();
                            command3.ExecuteNonQuery();
                            Update();
                            db.closeConnection();
                        }


                    }
                    DialogResult = true;
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var TranRemove = TranzakciiGrid.SelectedItems.Cast<C7Tranzakcii>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {TranRemove.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.Context.C7Tranzakcii.RemoveRange(TranRemove);
                    App.Context.SaveChanges();
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}