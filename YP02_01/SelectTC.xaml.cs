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
    /// Логика взаимодействия для SelectTC.xaml
    /// </summary>
    public partial class SelectTC : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable TC1;
        static DataTable STC;
        static DataTable City_;
        
        public SelectTC()
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

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            TC.TCName = TC1.Rows[tc_.SelectedIndex]["Name"].ToString().Trim();
            NavigationService.Navigate(new SelectPavil());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * from TC;";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
            adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();
            TC1 = new DataTable();
            adapter.Fill(TC1); //загрузка данных
            tc_.ItemsSource = TC1.DefaultView; //привязка к DataGrid
        }
    }
}
