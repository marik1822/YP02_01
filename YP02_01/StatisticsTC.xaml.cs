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
    /// Логика взаимодействия для StatisticsTC.xaml
    /// </summary>
    public partial class StatisticsTC : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable TC;
        public StatisticsTC()
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
            string sql = "SELECT Name from TC;";
            SqlConnection connection = null;
            TC = new DataTable();
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            connection.Open();

            adapter.Fill(TC);
            for (int i = 0; i < TC.Rows.Count; i++)
            {
                NameTC.Items.Add(TC.Rows[i]["Name"].ToString());
            }
            connection.Close();
        }

        private void NameTC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sql = "EXECUTE ViewTC1 @nameTC ='" + NameTC.SelectedItem.ToString()+ "';";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            connection.Open();
            TC = new DataTable();
            adapter.Fill(TC); //загрузка данных 
            TC_.ItemsSource = TC.DefaultView; //привязка к DataGrid
            connection.Close();
        }
    }
}
