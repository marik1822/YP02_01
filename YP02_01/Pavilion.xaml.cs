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
    /// Логика взаимодействия для Pavilion.xaml
    /// </summary>
    public partial class Pavilion : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable Pavilions;
        static DataTable Stages;
        DataTable Areas;

        public static string PavilName { get; set; }
        public static string PavilNum { get; set; }
        public static string PavilStage { get; set; }

        public Pavilion()
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
            string sql = "SELECT * from PavilsTable ";
            SqlConnection connection = null;
            string sql2;
            Stages = new DataTable();
            sql2 = "SELECT DISTINCT Stage from Pavils where Status !='Удален';";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql2, connection);
            connection.Open();

            adapter.Fill(Stages);
            for (int i = 0; i < Stages.Rows.Count; i++)
            {
                Stage.Items.Add(Stages.Rows[i]["Stage"].ToString());
            }
            connection.Close();

            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);


            Pavilions = new DataTable();
            adapter.Fill(Pavilions); //загрузка данных
            pavilDG.ItemsSource = Pavilions.DefaultView; //привязка к DataGrid
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            if (pavilDG.SelectedItem != null)
            {
                string NamePavil = Pavilions.Rows[pavilDG.SelectedIndex]["Name"].ToString().Trim();
                string NumberPavil = Pavilions.Rows[pavilDG.SelectedIndex]["NumberOfPavil"].ToString().Trim();
                string StagePavil = Pavilions.Rows[pavilDG.SelectedIndex]["Stage"].ToString().Trim();
                string sql2 = "UPDATE Pavils SET Status='Удален' WHERE ((Name='" + NamePavil + "') and (NumberOfPavil='" + NumberPavil + "') and (Stage='" + StagePavil + "'))";
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            PavilName = Pavilions.Rows[pavilDG.SelectedIndex]["Name"].ToString().Trim();
            PavilNum = Pavilions.Rows[pavilDG.SelectedIndex]["NumberOfPavil"].ToString().Trim();
            PavilStage = Pavilions.Rows[pavilDG.SelectedIndex]["Stage"].ToString().Trim();
            NavigationService.Navigate(new PavilionUpdate());
        }

        private void Stage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string st = Stage.SelectedItem.ToString();
            string sql3 = "SELECT * FROM PavilsTable WHERE Stage='" + st + "' and Status!='Удален' and StatusPavil!='Удален' ";
            SqlConnection connection = null;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql3, connection);
            SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
            Pavilions = new DataTable();
            adapter.Fill(Pavilions); //загрузка данных
            pavilDG.ItemsSource = Pavilions.DefaultView; //привязка к DataGrid
        }

        private void AreaBtn_Click(object sender, RoutedEventArgs e)
        {
            string sql = "";
            SqlConnection connection = null;
            if ((From.Text == "") && (To.Text == ""))
            {
                MessageBox.Show("Вы ничего не ввели");
            }
            else
            if (From.Text == "")
            {
                MessageBox.Show("Вы не ввели значение ОТ: ");
            }
            else
            if (To.Text == "")
            {
                MessageBox.Show("Вы не ввели значение ДО: ");
            }
            else
            if ((From.Text != "") && (To.Text != ""))
            {
                const string Pattern = @"[0-9]$";
                var from_ = From.Text.Trim().ToLowerInvariant();
                var to_ = To.Text.Trim().ToLowerInvariant();
                if ((Regex.Match(from_, Pattern).Success) && (Regex.Match(to_, Pattern).Success))
                {
                    sql = " SELECT * FROM PavilsTable where Area BETWEEN'" + from_ + "' and '" + to_ + "' ORDER BY Area ";
                    connection = new SqlConnection(connectionString);
                    adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(adapter as SqlDataAdapter);
                    Areas = new DataTable();
                    adapter.Fill(Areas); //загрузка данных
                    pavilDG.ItemsSource = Areas.DefaultView;
                }
                else
                    MessageBox.Show("Неверно введены значения");
            }
        }
    }
}