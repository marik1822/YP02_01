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
    /// Логика взаимодействия для AddEmpl.xaml
    /// </summary>
    public partial class AddEmpl : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable EmplTable;
        static DataTable PhotoTable;
        static DataTable RoleTable;
        static DataTable GenderTable;
        static bool clic_;
        public AddEmpl()
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
            string sql0;
            string id = null;
            string sql3;
            EmplTable = new DataTable();
            string gd = "";
            switch (Gender.SelectedIndex)
            {
                case 0:
                    gd = "М";
                    break;
                case 1:
                    gd = "Ж";
                    break;
            }
            string rl = "";
            switch (Role.SelectedIndex)
            {
                case 0:
                    rl = "Администратор";
                    break;
                case 1:
                    rl = "Менеджер А";
                    break;
                case 2:
                    rl = "Менеджер С";
                    break;
            }

            SqlConnection connection0 = null;
            sql0 = "SELECT top(1) ID from Employers Order by ID desc;";
            SqlCommand command0 = new SqlCommand(sql0, connection0);
            SqlDataReader reader = command0.ExecuteReader();
            while (reader.Read())
            {
                id = reader[0].ToString();
                int id1 = int.Parse(id) + 1;
                id = id1.ToString();
            }
            reader.Close();

            SqlConnection connection = null; 
            sql3 = "INSERT INTO Employers ID = '"+ id +"'Lname = '" + Lname_.Text + "', Fname= '" + Fname_.Text + "', Sname= '" + Sname_.Text + "', Login= '" + Login_.Text + "', Pass= '" + Pass_.Text + "', Role= '" + rl + "', Phone= '" + Phone_.Text + "', Gender= '" + gd + "' WHERE ID ='" + RedactEmpl.ID_ + "';";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql3, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Торговый центр успешно сохранён");
                clic_ = true;

            }
            else
                MessageBox.Show("Ошибка сохранения");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            clic_ = false; ;
            SqlConnection connection = null;
            string sql;
            RoleTable = new DataTable();
            sql = "SELECT DISTINCT Role from Employers where Role!='Удален';";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            connection.Open();
            adapter.Fill(RoleTable);
            for (int i = 0; i < RoleTable.Rows.Count; i++)
            {
                Role.Items.Add(RoleTable.Rows[i]["Role"].ToString());
            }
            connection.Close();

            SqlConnection connection2 = null;
            string sql2;
            GenderTable = new DataTable();
            sql2 = "SELECT DISTINCT Gender from Employers ;";
            connection2 = new SqlConnection(connectionString);
            SqlDataAdapter adapter2 = new SqlDataAdapter(sql2, connection2);
            connection2.Open();
            adapter2.Fill(GenderTable);
            for (int i = 0; i < GenderTable.Rows.Count; i++)
            {
                Gender.Items.Add(GenderTable.Rows[i]["Gender"].ToString());
            }
            connection2.Close();

        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (clic_)
            {
                string sql3;
                PhotoTable = new DataTable();
                SqlConnection connection = null;
                connection = new SqlConnection(connectionString);
                connection.Open();
                sql3 = "UPDATE Employers SET Photo =(SELECT * FROM OPENROWSET(BULK N'" + PathP.Text + "', SINGLE_BLOB) AS image) WHERE Name = '" + RedactEmpl.ID_ + "';";
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