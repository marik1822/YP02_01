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
    /// Логика взаимодействия для Pavilion.xaml
    /// </summary>
    public partial class Pavilion : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable Pavilions;

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
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(sql, connection);
            connection.Open();
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
                string sql2 = "DELETE FROM Pavils where ((Name='" + NamePavil + "') and (NumberOfPavil='" + NumberPavil + "') and (Stage='" + StagePavil + "'))";
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
            NavigationService.Navigate(new PavilionUpdate());
        }

        
    }
}