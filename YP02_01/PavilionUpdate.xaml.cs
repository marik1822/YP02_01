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
    /// Логика взаимодействия для PavilionUpdate.xaml
    /// </summary>
    public partial class PavilionUpdate : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable Pavilions;
        DataTable Pavils;

        public PavilionUpdate()
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

        static DataTable ExecuteSql(string sql)
        {
            //string sql;
            Pavilions = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    Pavilions.Load(reader);
                }
            }
            return Pavilions;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = null;
            string sql2;
            string sql;
            Pavils = new DataTable();
            sql2 = "SELECT DISTINCT Status from Pavils where Status!='Удален';";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql2, connection);
            connection.Open();

            adapter.Fill(Pavils);
            for (int i = 0; i < Pavils.Rows.Count; i++)
            {
                Status.Items.Add(Pavils.Rows[i]["Status"].ToString());
            }
            connection.Close();

            Pavilions = ExecuteSql("SELECT * FROM Pavils WHERE Name='" + Pavilion.PavilName + "'");
            Stage_.Text = Pavilions.Rows[0]["Stage"].ToString();
            NumberOfPavil_.Text = Pavilions.Rows[0]["NumberOfPavil"].ToString();
            Area_.Text = Pavilions.Rows[0]["Area"].ToString();
            Koef_.Text = Pavilions.Rows[0]["Koef"].ToString();
            Price_.Text = Pavilions.Rows[0]["Price"].ToString();
            /*switch (Pavilions.Rows[0]["Status"].ToString())
            {
                case "План":
                    Status.SelectedIndex = 0;
                    break;
                case "Реализация":
                    Status.SelectedIndex = 1;
                    break;
                case "Строительсто":
                    Status.SelectedIndex = 2;
                    break;
            }*/

        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
