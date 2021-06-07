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
    /// Логика взаимодействия для AddTC.xaml
    /// </summary>
    public partial class AddTC : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable TCtable;
        static DataTable STC;
        static DataTable TC_;
        static DataTable TC1;
        static bool clic_;
        public AddTC()
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
            clic_ = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            clic_ = false; ;
            string sql;
            SqlConnection connection = null;
            STC = new DataTable();
            sql = "SELECT DISTINCT Status from TC ;";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            connection.Open();

            adapter.Fill(STC);
            for (int i = 0; i < STC.Rows.Count; i++)
            {
                Status.Items.Add(STC.Rows[i]["Status"].ToString());
            }
            connection.Close();
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            string sql3;
            TC1 = new DataTable();
            string st = "";
            switch (Status.SelectedIndex)
            {
                case 0:
                    st = "План";
                    break;
                case 1:
                    st = "Реализация";
                    break;
                case 2:
                    st = "Строительсто";
                    break;
            }
            SqlConnection connection = null;
            string Koef1 = "";
            string Koef2 = Koef_.Text.ToString();
            for (int i = 0; i != Koef2.Length; i++)
            {
                if (Koef2[i] == ',')
                {
                    Koef1 = Koef1 + ".";
                }
                else
                    Koef1 = Koef1 + Koef2[i];
            }
            sql3 = "UPDATE TC SET Status='" + st + "',CountPavil=" + NumOfPav_.Text + ",City='" + City_.Text + "',Price=" + PriceBuild_.Text + " ,Koef=" + Koef1 + " ,Stages=" + Floor_.Text + " WHERE Name='" + TC.TCName + "';";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql3, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Торговый центр успешно редактирован");
                TC.TCName = Name_.Text;
            }
            else
                MessageBox.Show("Ошибка редактирования");
        }
    }
}
