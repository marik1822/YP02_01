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
    /// Логика взаимодействия для AddArendator.xaml
    /// </summary>
    public partial class AddArendator : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable ArendatTable;
        public AddArendator()
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
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string sql;
            string sql0;
            string id = "";
            ArendatTable = new DataTable();
            //connection = new SqlConnection(connectionString);
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            //connection.Open();
            sql0 = "SELECT top(1) ID from Arendatory Order by ID desc;";
            SqlCommand command0 = new SqlCommand(sql0, connection);
            connection.Open();
            SqlDataReader reader0 = command0.ExecuteReader();
            while (reader0.Read())
            {
                id = reader0[0].ToString();
                int id1 = int.Parse(id) + 1;
                id = id1.ToString();
            }
            reader0.Close();

            connection = null;
            sql = "INSERT INTO Arendatory ID = '" + id + "', Name = '" + Name_.Text + "', Phone= '" + Phone_.Text + "', Address= '" + Adress_.Text + "' WHERE ID ='" + Arendators.ID_Arendator + "';";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Торговый центр успешно сохранён");

            }
            else
                MessageBox.Show("Ошибка сохранения");
        }
    }
}
