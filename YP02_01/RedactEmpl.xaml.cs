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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace YP02_01
{
    /// <summary>
    /// Логика взаимодействия для RedactEmpl.xaml
    /// </summary>
    public partial class RedactEmpl : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable Employers;

        public static string ID_ { get; set; }

        public RedactEmpl()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //подключено БД
            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка подключения БД");
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * from Employers;";
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);

            Employers = new DataTable();
            adapter.Fill(Employers); //загрузка данных
            empl_.ItemsSource = Employers.DefaultView; //привязка к DataGrid
        }

        private void Izm_Click(object sender, RoutedEventArgs e)
        {
            ID_ = Employers.Rows[empl_.SelectedIndex]["ID"].ToString().Trim();
            NavigationService.Navigate(new UpdateEmpl());
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmpl());
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            if (empl_.SelectedItem != null)
            {
                string idempl = Employers.Rows[empl_.SelectedIndex]["ID"].ToString().Trim();
                string sql2 = "UPDATE Employers SET Role='Удален' WHERE ID='" + idempl + "'";
                SqlConnection connection = null;

                connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql2, connection);
                connection.Open();
                int num = command.ExecuteNonQuery();
                if (num != 0)
                {
                    MessageBox.Show("Строка успешно удалена");
                }
                else MessageBox.Show("Ошибка удаления");
            }
            else MessageBox.Show("Вы не выбрали строку для удаления");
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string sql = "";
            SqlConnection connection = null;
            if (Search.Text == "")
            {
                MessageBox.Show("Вы не ввели значение: ");
            }
            else
            if (Search.Text != "")
            {
                const string Pattern = @"^[А-Яа-яЁё\s]+$";
                var search_ = Search.Text.Trim().ToLowerInvariant();
                if (Regex.Match(search_, Pattern).Success)
                {
                    sql = " SELECT * FROM Employers where Lname= '" + search_ + "' ";
                    connection = new SqlConnection(connectionString);
                    adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
                    Employers = new DataTable();
                    adapter.Fill(Employers); //загрузка данных
                    empl_.ItemsSource = Employers.DefaultView;
                }
                else
                    MessageBox.Show("Неверно введены значения");
            }
        }
    }
}
