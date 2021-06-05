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
    /// Логика взаимодействия для TCUpdate.xaml
    /// </summary>
    public partial class TCUpdate : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable TCtable;
        static DataTable STC;
        static DataTable TC_;
        public TCUpdate()
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
            TCtable = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    TCtable.Load(reader);
                }
            }
            return TCtable;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = null;
            string sql2;
            string sql;
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
            TCtable = ExecuteSql("SELECT Image FROM TC WHERE Name='" + TC.TCName + "' ");
            LViewImage.ItemsSource = TCtable.DefaultView;
            TCtable = ExecuteSql("SELECT * FROM TC WHERE Name='" + TC.TCName + "'");
            Name_.Text = TCtable.Rows[0]["Name"].ToString();
            Koef_.Text = TCtable.Rows[0]["Koef"].ToString();
            PriceBuild_.Text = TCtable.Rows[0]["Price"].ToString();
            City_.Text = TCtable.Rows[0]["City"].ToString();
            Floor_.Text = TCtable.Rows[0]["Stages"].ToString();
            NumOfPav_.Text = TCtable.Rows[0]["CountPavil"].ToString();
            switch (TCtable.Rows[0]["Status"].ToString())
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
            }

        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
