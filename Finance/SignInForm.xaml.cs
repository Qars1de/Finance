using Finance.BalanceForms;
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

namespace Finance
{
    /// <summary>
    /// Логика взаимодействия для SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {
        public SignInForm()
        {
            InitializeComponent();
        }

        private void signButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text == "" || passBox.Text == "")
            {
                MessageBox.Show("Заполните поля");
            }
            else
            {

                DB db = new DB();

                DataTable table = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();


                SqlCommand command2 = new SqlCommand("select UserID from [7User] where Login = '" + loginBox.Text + "'", db.getConnection());

                db.openConnection();
                SqlDataReader reader = command2.ExecuteReader();

                while (reader.Read())
                {
                    int UserLogin = reader.GetInt32(0);
                    App.Current.Resources["login"] = UserLogin;
                }
                db.closeConnection();

                SqlCommand command = new SqlCommand("SELECT * FROM [7User] where Login = @login and Password = @pass", db.getConnection());
                command.Parameters.Add("@login", SqlDbType.VarChar).Value = loginBox.Text;
                command.Parameters.Add("@pass", SqlDbType.VarChar).Value = passBox.Text;


                db.openConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    BalanceForm form = new BalanceForm();
                    form.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
                db.closeConnection();
            }
        }

        private void reglabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SignUpForm form = new SignUpForm();
            form.Show();
            this.Close();
        }
    }
}
