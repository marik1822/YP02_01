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
    /// Логика взаимодействия для UpdateEmpl.xaml
    /// </summary>
    public partial class UpdateEmpl : Page
    {
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable RoleTable;
        static DataTable GenderTable;
        static DataTable EmplTable;

        public UpdateEmpl()
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
            EmplTable = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    EmplTable.Load(reader);
                }
            }
            return EmplTable;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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

            EmplTable = ExecuteSql("SELECT Photo FROM Employers WHERE ID='" + RedactEmpl.ID_ + "' ");
            LViewImage.ItemsSource = EmplTable.DefaultView;
            EmplTable = ExecuteSql("SELECT * FROM Employers WHERE ID='" + RedactEmpl.ID_ + "'");
            Lname_.Text = EmplTable.Rows[0]["Lname"].ToString();
            Fname_.Text = EmplTable.Rows[0]["Fname"].ToString();
            Sname_.Text = EmplTable.Rows[0]["Sname"].ToString();
            Login_.Text = EmplTable.Rows[0]["Login"].ToString();
            Pass_.Text = EmplTable.Rows[0]["Pass"].ToString();
            Phone_.Text = EmplTable.Rows[0]["Phone"].ToString();
            switch (EmplTable.Rows[0]["Gender"].ToString())
            {
                case "Ж":
                    Gender.SelectedIndex = 0;
                    break;
                case "М":
                    Gender.SelectedIndex = 1;
                    break;
            }
            switch (EmplTable.Rows[0]["Role"].ToString())
            {
                case "Администратор":
                    Role.SelectedIndex = 0;
                    break;
                case "Менеджер А":
                    Role.SelectedIndex = 1;
                    break;
                case "Менеджер С":
                    Role.SelectedIndex = 2;
                    break;
            }

        }

       
        private void Add_Click(object sender, RoutedEventArgs e)
        {
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

            SqlConnection connection = null;
            sql3 = "UPDATE Employers SET Lname = '" + Lname_.Text + "', Fname= '" + Fname_.Text + "', Sname= '" + Sname_.Text + "', Login= '" + Login_.Text + "', Pass= '" + Pass_.Text + "', Role= '" + rl + "', Phone= '" + Phone_.Text + "', Gender= '" + gd + "' WHERE ID ='" + RedactEmpl.ID_ + "';";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql3, connection);
            connection.Open();
            int num = command.ExecuteNonQuery();
            if (num != 0)
            {
                MessageBox.Show("Пользователь успешно редактирован");
            }
            else
                MessageBox.Show("Ошибка редактирования");
        }

        private void Gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

