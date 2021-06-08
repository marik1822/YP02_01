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
            sql3 = "INSERT INTO TC(Name,Status,CountPavil,City,Price,Koef,Stages) VALUES('" + Name_.Text + "','" + Status.SelectedItem.ToString() + "'," + NumOfPav_.Text+ " ,'" + City_.Text + "' ," + PriceBuild_.Text + "," + Koef_.Text + " ," + Floor_.Text + " );";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql3, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Торговый центр успешно сохранён");
                TC.TCName = Name_.Text;
                clic_ = true;

            }
            else
                MessageBox.Show("Ошибка сохранения");
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
            if (clic_)
            {
                string sql3;
                TC_ = new DataTable();
                SqlConnection connection = null;
                //bool id = true;
                connection = new SqlConnection(connectionString);
                connection.Open();
                sql3 = "UPDATE TC SET Image =(SELECT * FROM OPENROWSET(BULK N'" + PathP.Text+ "', SINGLE_BLOB) AS image) WHERE Name = '" + TC.TCName + "';";
                SqlCommand command3 = new SqlCommand(sql3, connection);
                int num2 = command3.ExecuteNonQuery();
                if (num2 != 0)
                {
                    MessageBox.Show("Фотография успешно загружена");
                }
                else MessageBox.Show("Ошибка: Неправильно указан путь");

            }
        }
        }
    
}
