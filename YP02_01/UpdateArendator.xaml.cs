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
    /// Логика взаимодействия для UpdateArendator.xaml
    /// </summary>
    public partial class UpdateArendator : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable ArendatTable;
        public UpdateArendator()
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
            ArendatTable = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    ArendatTable.Load(reader);
                }
            }
            return ArendatTable;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ArendatTable = ExecuteSql("SELECT * FROM Arendatory WHERE ID='" + Arendators.ID_Arendator + "'");
            Name_.Text = ArendatTable.Rows[0]["Name"].ToString();
            Phone_.Text = ArendatTable.Rows[0]["Phone"].ToString();
            Adress_.Text = ArendatTable.Rows[0]["Address"].ToString();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string sql3;
        
            ArendatTable = new DataTable();
            SqlConnection connection = null;
            sql3 = "UPDATE Arendatory SET Name= '" + Name_.Text + "', Phone= '" + Phone_.Text + "', Address= '" + Adress_.Text + "' WHERE ID ='" + Arendators.ID_Arendator + "';";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql3, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Пользователь успешно редактирован");
            }
            else
                MessageBox.Show("Ошибка редактирования");
        }
    }
}
