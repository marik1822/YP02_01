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
using System.Text.RegularExpressions;


namespace YP02_01
{
    /// <summary>
    /// Логика взаимодействия для SelectPavil.xaml
    /// </summary>
    public partial class SelectPavil : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable Pavilions;
        static DataTable Stages;
        DataTable Areas;
        public SelectPavil()
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
            string sql = "SELECT * from PavilsTable WHERE Name='"+TC.TCName+ "' AND StatusPavil='Свободен'";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);


            Pavilions = new DataTable();
            adapter.Fill(Pavilions); //загрузка данных
            pavilDG.ItemsSource = Pavilions.DefaultView; //привязка к DataGrid
        }

        private void SelectArend_Click(object sender, RoutedEventArgs e)
        {
            if ((Name_.Text.Trim() == "") && (Phone_.Text.Trim() == "") && (Adres_.Text.Trim() == ""))
            {
                MessageBox.Show("Вы не ввели информацию о арендаторе");
            } else
            if (Name_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели название организации");
            } else
            if (Phone_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели телефон");
            } else
            if (Adres_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели адрес");
            } else
            if ((Name_.Text.Trim() != "") && (Phone_.Text.Trim() != "") && (Adres_.Text.Trim() != "")) {
                Pavilion.PavilNum = Pavilions.Rows[pavilDG.SelectedIndex]["NumberOfPavil"].ToString().Trim(); ;
                Pavilion.PavilStage = Pavilions.Rows[pavilDG.SelectedIndex]["Stage"].ToString().Trim(); ;
                Pavilion.PavilName = TC.TCName;
                string sql2 = "";
                SqlConnection connection = null;
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql2, connection);
                connection.Open();
                int num = command.ExecuteNonQuery();
                if (num != 0)
                {
                    MessageBox.Show("Павильон успешно арендован");
                    // TC.TCName = Name_.Text;
                }
                else
                    MessageBox.Show("Ошибка арендации");
            }
        }

        private void SelectBron_Click(object sender, RoutedEventArgs e)
        {
            if ((Name_.Text.Trim() == "") && (Phone_.Text.Trim() == "") && (Adres_.Text.Trim() == ""))
            {
                MessageBox.Show("Вы не ввели информацию о арендаторе");
            }
            else
           if (Name_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели название организации");
            }
            else
           if (Phone_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели телефон");
            }
            else
           if (Adres_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели адрес");
            }
            else
            if ((Name_.Text.Trim() != "") && (Phone_.Text.Trim() != "") && (Adres_.Text.Trim() != ""))
                {
                Pavilion.PavilNum = Pavilions.Rows[pavilDG.SelectedIndex]["NumberOfPavil"].ToString().Trim(); ;
                Pavilion.PavilStage = Pavilions.Rows[pavilDG.SelectedIndex]["Stage"].ToString().Trim(); ;
                Pavilion.PavilName = TC.TCName;
                string sql3 = "";
                SqlConnection connection = null;
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql3, connection);
                connection.Open();
                int num = command.ExecuteNonQuery();
                if (num != 0)
                {
                    MessageBox.Show("Павильон успешно забронирован");
                    //TC.TCName = Name_.Text;
                }
                else
                    MessageBox.Show("Ошибка бронирования");
                }
        }
    }
}
