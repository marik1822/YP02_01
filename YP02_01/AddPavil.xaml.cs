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
    /// Логика взаимодействия для AddPavil.xaml
    /// </summary>
    public partial class AddPavil : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        
        static DataTable Pavil;
        static DataTable Pavilions;

        public AddPavil()
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
            string sql;
            SqlConnection connection = null;
            Pavil = new DataTable();
            sql = "SELECT DISTINCT Status from Pavils where Status!='Удален'; ;";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            connection.Open();

            adapter.Fill(Pavil);
            for (int i = 0; i < Pavil.Rows.Count; i++)
            {
                Status.Items.Add(Pavil.Rows[i]["Status"].ToString());
            }
            connection.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string sql3;
            Pavilions = new DataTable();
            SqlConnection connection = null;
            sql3 = "INSERT INTO Pavils SET Name='" + Name_.Text + "', NumberOfPavil='" + NumberOfPavil_.Text + "', Stage=" + Stage_.Text + ", Status='" + Status.SelectedItem.ToString() + "', Area=" + Area_.Text + ", PriceForMetr=" + Price_.Text + " ,Koef=" + Koef_.Text + ";";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql3, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Павильон успешно создан");
            }
            else
                MessageBox.Show("Ошибка создания");
        }

       
    }
}
