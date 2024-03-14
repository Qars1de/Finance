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
    /// Логика взаимодействия для SignUpForm.xaml
    /// </summary>
    public partial class SignUpForm : Window
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(LastNameBox.Text))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(PatronymicBox.Text))
                errors.AppendLine("Укажите отчество");
            if (string.IsNullOrWhiteSpace(loginBox.Text))
                errors.AppendLine("Придуумайте логин");
            if (string.IsNullOrWhiteSpace(passBox.Text))
                errors.AppendLine("Введите пароль");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                var check = (from b in App.Context.C7User where b.Login == loginBox.Text select b).SingleOrDefault();
                if (check != null)
                {
                    MessageBox.Show("Данный логин занят");
                }
                else
                {
                    DB db = new DB();

                    SqlCommand command = new SqlCommand("insert into [7User] (LastName, FirstName, Patronymic, Login, Password) values (@surname, @name, @patr, @log, @pass)", db.getConnection());
                    command.Parameters.Add("@surname", SqlDbType.VarChar).Value = LastNameBox.Text;
                    command.Parameters.Add("@name", SqlDbType.VarChar).Value = FirstNameBox.Text;
                    command.Parameters.Add("@patr", SqlDbType.VarChar).Value = PatronymicBox.Text;
                    command.Parameters.Add("@log", SqlDbType.VarChar).Value = loginBox.Text;
                    command.Parameters.Add("@pass", SqlDbType.VarChar).Value = passBox.Text;

                    db.openConnection();
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Аккаунт успешно создан");
                        SignInForm form = new SignInForm();
                        form.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    db.closeConnection();

                    //Создание счёта баланса
                    SqlCommand command2 = new SqlCommand("select UserID from [7User] where Login = '" + loginBox.Text + "'", db.getConnection());

                    db.openConnection();
                    SqlDataReader reader = command2.ExecuteReader();

                    while (reader.Read())
                    {
                        int UserLogin = reader.GetInt32(0);
                        App.Current.Resources["login"] = UserLogin;
                    }
                    SqlCommand command3 = new SqlCommand("insert into [7Balance] (UserID, Balance) values (@user, @bal)", db.getConnection());
                    command3.Parameters.Add("@user", SqlDbType.Int).Value = App.Current.Resources["login"];
                    command3.Parameters.Add("@bal", SqlDbType.Int).Value = 0;

                    command3.ExecuteNonQuery();
                    db.closeConnection();
                }
            }
        }

        private void authlabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SignInForm form = new SignInForm();
            form.Show();
            this.Close();
        }
    }
}
