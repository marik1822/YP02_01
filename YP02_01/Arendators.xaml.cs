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
    /// Логика взаимодействия для Arendators.xaml
    /// </summary>
    public partial class Arendators : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable Arendatory;

        public static string ID_Arendator { get; set; }
        public Arendators()
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
            string sql = "SELECT * from Arendatory;";
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);

            Arendatory = new DataTable();
            adapter.Fill(Arendatory); //загрузка данных
            arendat_.ItemsSource = Arendatory.DefaultView; //привязка к DataGrid
        }

        private void Izm_Click(object sender, RoutedEventArgs e)
        {
            ID_Arendator = Arendatory.Rows[arendat_.SelectedIndex]["ID"].ToString().Trim();
            NavigationService.Navigate(new UpdateArendator());
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddArendator());
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
                const string Pattern = @"^[A-Za-z\s]+$";
                var search_ = Search.Text.Trim().ToLowerInvariant();
                if (Regex.Match(search_, Pattern).Success)
                {
                    sql = " SELECT * FROM Arendatory where Name= '" + search_ + "' ";
                    connection = new SqlConnection(connectionString);
                    adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
                    Arendatory = new DataTable();
                    adapter.Fill(Arendatory); //загрузка данных
                    arendat_.ItemsSource = Arendatory.DefaultView;
                }
                else
                    MessageBox.Show("Неверно введены значения");
            }
        }

        private void SpisArend_Click(object sender, RoutedEventArgs e)
        {
            ID_Arendator = Arendatory.Rows[arendat_.SelectedIndex]["ID"].ToString().Trim();
            NavigationService.Navigate(new ListArend());
            
        }
    }
}