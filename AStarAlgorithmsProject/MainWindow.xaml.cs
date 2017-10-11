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

namespace AStarAlgorithmsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainDriver main;

        public MainWindow()
        {
            InitializeComponent();
            textBox.IsReadOnly = true;
            textBox.TextAlignment = TextAlignment.Center;

            main = new MainDriver(); // creates an instance of Main Drive for us to reference whenever the user changes something about the display and get results from.
            textBox.Text = main.TestMap(); // shows results of the algorithim, currenttly uses hardcoded start and end positions.
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
