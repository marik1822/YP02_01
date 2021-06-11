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
    /// Логика взаимодействия для PaybackTC.xaml
    /// </summary>
    public partial class PaybackTC : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable TC;
        static DataTable TCitog;
        static DataTable TCPods;
        public static string NameTC_ { get; set; }
        public static string PriceTC { get; set; }
        public static string Result { get; set; }
        public PaybackTC()
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
            string sql = "SELECT Name , Price FROM TC WHERE Name='" + NameTC.SelectedItem.ToString() + "';";
            //  string sql3 = "SELECT * FROM funcofposchet ("+PriceTC+",'"+NameTC_+"');";
            SqlConnection connection = null;
            string Result2 = "";
            SqlConnection connection1 = null;
            SqlConnection connection2 = null;
            /* connection = new SqlConnection(connectionString);
             adapter = new SqlDataAdapter(sql, connection);
             connection.Open();
             SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
             TC = new DataTable();
             adapter.Fill(TC); //загрузка данных */

            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                NameTC_ = reader[0].ToString();
                PriceTC = reader[1].ToString();
                string sql3 = "SELECT dbo.funcofposchet(" + PriceTC + ",'" + NameTC_ + "') as result;";
                //string sql2 = "EXECUTE spisfunc " + Result + ",'" + NameTC_ + "' ";
                connection1 = new SqlConnection(connectionString);
                SqlCommand command1 = new SqlCommand(sql3, connection1);
                connection1.Open();
                reader.Close();
                Result = command1.ExecuteScalar().ToString();
                //string Result2;
                for (int i = 0; i != Result.Length; i++)
                {
                    if (Result[i] == ',')
                    {
                        Result2 = Result2 + ".";
                    }
                    else
                        Result2 = Result2 + Result[i];
                }
                if (Result2 == "")
                {
                    Result2 = "0";
                }
                connection1.Close();
                connection2 = new SqlConnection(connectionString);
                connection2.Open();
                string sql2 = "EXECUTE spisfunc " + Result2 + ",'" + NameTC_ + "' ";
                

                adapter = new SqlDataAdapter(sql2, connection2);

                TCitog = new DataTable();
                adapter.Fill(TCitog); //загрузка данных 
                TC_.ItemsSource = TCitog.DefaultView; //привязка к DataGrid
                connection2.Close(); 
                return;
            }
            reader.Close();
            connection.Close();


        }
    }
}
