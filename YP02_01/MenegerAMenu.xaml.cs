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
using System.Windows.Shapes;

namespace YP02_01
{
    /// <summary>
    /// Логика взаимодействия для MenegerAMenu.xaml
    /// </summary>
    public partial class MenegerAMenu : Window
    {
        public MenegerAMenu()
        {
            InitializeComponent();
            frameM.Content = new Arendators();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Arenda_Click(object sender, RoutedEventArgs e)
        {

            frameM.Content = new Arendators();
        }
    }
}
