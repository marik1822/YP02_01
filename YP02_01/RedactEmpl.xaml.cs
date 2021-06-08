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
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);

            Employers = new DataTable();
            adapter.Fill(Employers); //загрузка данных
            empl_.ItemsSource = Employers.DefaultView; //привязка к DataGrid
        }

        private void Izm_Click(object sender, RoutedEventArgs e)
        {
            ID_ = Employers.Rows[empl_.SelectedIndex]["ID"].ToString().Trim();
            NavigationService.Navigate(new UpdateEmpl());
        }
    }
}
