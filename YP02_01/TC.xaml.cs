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
    /// Логика взаимодействия для TC.xaml
    /// </summary>
    public partial class TC : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable TC1;
        static DataTable STC;
        static DataTable City_;
        public static string TCName { get; set; }
        public TC()
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
            string sql = "SELECT * from TC;";
            string sql3 = "SELECT DISTINCT City from TC where Status!='Удален'";
            SqlConnection connection = null;
            string sql2;
            STC = new DataTable();
            sql2 = "SELECT DISTINCT Status from TC where Status!='Удален';";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql2, connection);
            connection.Open();

            adapter.Fill(STC);
            for (int i = 0; i < STC.Rows.Count; i++)
            {
                Status.Items.Add(STC.Rows[i]["Status"].ToString());
            }
            connection.Close();
            connection = new SqlConnection(connectionString);
            City_ = new DataTable();
            adapter = new SqlDataAdapter(sql3, connection);
            connection.Open();
            adapter.Fill(City_);
            for (int i = 0; i < City_.Rows.Count; i++)
            {
                City.Items.Add(City_.Rows[i]["City"].ToString());
            }
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
            adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();

            TC1 = new DataTable();
            adapter.Fill(TC1); //загрузка данных
            tc_.ItemsSource = TC1.DefaultView; //привязка к DataGrid
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (tc_.SelectedItem != null)
            {
                string NameTC = TC1.Rows[tc_.SelectedIndex]["Name"].ToString().Trim();
                string sql2 = "UPDATE TC SET Status='Удален' WHERE Name='" + NameTC + "';";
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

        private void Izm_Click(object sender, RoutedEventArgs e)
        {
            TCName = TC1.Rows[tc_.SelectedIndex]["Name"].ToString().Trim();
            NavigationService.Navigate(new TCUpdate());
        }

        

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string st = Status.SelectedItem.ToString();
            string sql3 = "SELECT * FROM TC WHERE Status='" + st + "'";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql3, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
            adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();
            TC1 = new DataTable();
            adapter.Fill(TC1); //загрузка данных
            tc_.ItemsSource = TC1.DefaultView; //привязка к DataGrid
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string st = City.SelectedItem.ToString();
            string sql3 = "SELECT * FROM TC WHERE City='" + st + "' and Status!='Удален' ";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql3, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
            adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();
            TC1 = new DataTable();
            adapter.Fill(TC1); //загрузка данных
            tc_.ItemsSource = TC1.DefaultView; //привязка к DataGrid
        }

        private void AddTC_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTC());
        }
    }
}
