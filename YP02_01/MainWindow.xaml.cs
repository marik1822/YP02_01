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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace YP02_01
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable Employee;
        public int Logchick { get; set; }
        public string Role { get; set; }
        public string ID { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Nois.Opacity = 0.0;
            Kaptcha.Text = "";
            KaptchaLog.Opacity = 0.0;
            KaptchaLog.IsEnabled = false;
            Logchick = 0;
            Nois.IsEnabled = false;
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка подключения БД");
            }

        }
        private void LogIN()
        {
            string sql = "";
            Employee = new DataTable();
            string password_ = Password_.Password.ToString().Trim();
            string login_ = Login.Text.Trim();
            SqlConnection connection = null;
            if ((Login.Text == "") && (Password_.Password.ToString() == ""))
            {
                MessageBox.Show("Вы ничего не ввели");
            }
            else
            if (Login.Text == "")
            {
                MessageBox.Show("Вы не ввели логин");
            }
            else
            if (Password_.Password.ToString() == "")
            {
                MessageBox.Show("Вы не ввели пароль");
            }
            else
               if ((Login.Text != "") && (Password_.Password.ToString() != ""))
            {
                sql = " SELECT * FROM Employers where Login='" + login_ + "' and Pass='" + password_ + "'";
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ID = reader[0].ToString();
                    Role = reader[6].ToString();
                    if (Role == "Менеджер А")
                    {
                        //this.Close();
                    }
                    else
                        if (Role == "Менеджер С")
                    {
                        new MenegerCMenu().Show();
                        this.Close();
                    }
                    else
                        if (Role == "Администратор")
                    {
                        new AdminMenu().Show();
                        this.Close();
                    }
                    return;
                }
                reader.Close();
            }
        }
        private void GenerationCapcha()
        {
            Random rnd = new Random();
            string capchastr = "";
            for (int i = 0; i < 5; i++)
            {
                char newchar = (char)(rnd.Next(48, 57) + rnd.Next(8, 25) + rnd.Next(7, 25));
                capchastr += newchar;
            }
            Kaptcha.Text = capchastr;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (KaptchaLog.Text != Kaptcha.Text)
            {
                MessageBox.Show("Неправильно введена капча");
            }else
            if (((Logchick <= 2)&&(KaptchaLog.IsEnabled!=true)) ||((Logchick>2)&&(KaptchaLog.Text== Kaptcha.Text)&& (KaptchaLog.IsEnabled == true))) {
                LogIN();
                Logchick++;
            }
            else
            if((Logchick>2)&&(KaptchaLog.IsEnabled==false))
            {
                Nois.IsEnabled = true;
                Nois.Opacity = 0.75;
                GenerationCapcha();
                KaptchaLog.Opacity = 1;
                KaptchaLog.IsEnabled = true;
                Login.Text = "";
                Password_.Password = "";
            }
            }

        private void Nois_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GenerationCapcha();
        }
    }
}
