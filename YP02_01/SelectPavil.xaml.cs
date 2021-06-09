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
        static DataTable Arend;
        DataTable Arends;
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
            if (Name_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели информацию о арендаторе");
            } else

            if (DateEnd.SelectedDate.Value.ToString() == "")
            {
                MessageBox.Show("Вы не выбрали дату");
            }
            if ((Name_.Text.Trim() != "")&&(DateEnd.SelectedDate.Value.ToString() != ""))  {
                string sql;
                string sql0;
                string id="";
                Arends = new DataTable();
                //connection = new SqlConnection(connectionString);
                SqlConnection connection = null;
                connection = new SqlConnection(connectionString);
                //connection.Open();
                sql0 = "SELECT top(1) id from Arenda Order by id desc;";
                SqlCommand command0 = new SqlCommand(sql0, connection);
                connection.Open();
                SqlDataReader reader0 = command0.ExecuteReader();
                while (reader0.Read())
                {
                    id = reader0[0].ToString();
                    int id1 = int.Parse(id) + 1;
                    id = id1.ToString();
                }
                reader0.Close();
              //  connection.Close();
                connection = null;
                sql = "SELECT * FROM Arendatory WHERE Name='"+Name_.Text+"'";
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string id_arendatory = reader[0].ToString();
                    Pavilion.PavilNum = Pavilions.Rows[pavilDG.SelectedIndex]["NumberOfPavil"].ToString().Trim(); ;
                    Pavilion.PavilStage = Pavilions.Rows[pavilDG.SelectedIndex]["Stage"].ToString().Trim(); ;
                    Pavilion.PavilName = TC.TCName;
                    //set dateformat ymd go
                    string sql2 = " EXECUTE BronPavil @IdArenda = "+id+", @IdArendatory = "+id_arendatory+", @Name = '"+TC.TCName+"', @ID_EMP = "+MainWindow.ID+", @NumberOfPavil = '"+ Pavilion.PavilNum + "', @Status = 'Открыт', @Start_rent = '" + DateTime.Now.ToString()+"', @Finisth_rent = '"+DateEnd.Text+ " 00:00:00" + "',@StatusPavil='Арендован';";
                    //SqlConnection connection = null;
                    connection = new SqlConnection(connectionString);
                    SqlCommand command1 = new SqlCommand(sql2, connection);
                    connection.Open();
                    int num = command1.ExecuteNonQuery();
                    if (num != 0)
                    {
                        MessageBox.Show("Павильон успешно арендован");
                        // TC.TCName = Name_.Text;
                    }
                    else
                        MessageBox.Show("Ошибка арендации");
                    return;
                }
                reader.Close();
                MessageBox.Show("Арендатор не найден");
                
            }
        }

        private void SelectBron_Click(object sender, RoutedEventArgs e)
        {
            if (Name_.Text.Trim() == "")
            {
                MessageBox.Show("Вы не ввели информацию о арендаторе");
            }
            else

            if (DateEnd.SelectedDate.Value.ToString() == "")
            {
                MessageBox.Show("Вы не выбрали дату");
            }
            if ((Name_.Text.Trim() != "") && (DateEnd.SelectedDate.Value.ToString() != ""))
            {
                string sql;
                string sql0;
                string id = "";
                Arends = new DataTable();
                //connection = new SqlConnection(connectionString);
                SqlConnection connection = null;
                connection = new SqlConnection(connectionString);
                //connection.Open();
                sql0 = "SELECT top(1) id from Arenda Order by id desc;";
                SqlCommand command0 = new SqlCommand(sql0, connection);
                connection.Open();
                SqlDataReader reader0 = command0.ExecuteReader();
                while (reader0.Read())
                {
                    id = reader0[0].ToString();
                    int id1 = int.Parse(id) + 1;
                    id = id1.ToString();
                }
                reader0.Close();
                //  connection.Close();
                connection = null;
                sql = "SELECT * FROM Arendatory WHERE Name='" + Name_.Text + "'";
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string id_arendatory = reader[0].ToString();
                    Pavilion.PavilNum = Pavilions.Rows[pavilDG.SelectedIndex]["NumberOfPavil"].ToString().Trim(); ;
                    Pavilion.PavilStage = Pavilions.Rows[pavilDG.SelectedIndex]["Stage"].ToString().Trim(); ;
                    Pavilion.PavilName = TC.TCName;
                    //set dateformat ymd go
                    string sql2 = " EXECUTE BronPavil @IdArenda = " + id + ", @IdArendatory = " + id_arendatory + ", @Name = '" + TC.TCName + "', @ID_EMP = " + MainWindow.ID + ", @NumberOfPavil = '" + Pavilion.PavilNum + "', @Status = 'Ожидание', @Start_rent = '" + DateTime.Now.ToString() + "', @Finisth_rent = '" + DateEnd.Text + " 00:00:00" + "',@StatusPavil='Забронировано';";
                    //SqlConnection connection = null;
                    connection = new SqlConnection(connectionString);
                    SqlCommand command1 = new SqlCommand(sql2, connection);
                    connection.Open();
                    int num = command1.ExecuteNonQuery();
                    if (num != 0)
                    {
                        MessageBox.Show("Павильон успешно забронирован");
                        // TC.TCName = Name_.Text;
                    }
                    else
                        MessageBox.Show("Ошибка бронирования");
                    return;
                }
                reader.Close();
                MessageBox.Show("Арендатор не найден");

            }
        }
    }
}
