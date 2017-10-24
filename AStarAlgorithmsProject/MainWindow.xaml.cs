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
            /*InitializeComponent();
            textBox.IsReadOnly = true;
            textBox.TextAlignment = TextAlignment.Center;

            main = new MainDriver(); // creates an instance of Main Drive for us to reference whenever the user changes something about the display and get results from.
            textBox.Text = main.TestMap(); // shows results of the algorithim, currenttly uses hardcoded start and end positions.
        */
            Grid map = new Grid();
            //map.Name = "map";
            map.Background = Brushes.Black;
            map.HorizontalAlignment = HorizontalAlignment.Center;
            map.VerticalAlignment = VerticalAlignment.Center;
            map.ShowGridLines = true;
            map.Height = 100;
            map.Width = 100;

            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            map.ColumnDefinitions.Add(colDef1);
            map.ColumnDefinitions.Add(colDef2);
            map.ColumnDefinitions.Add(colDef3);

            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            map.RowDefinitions.Add(rowDef1);
            map.RowDefinitions.Add(rowDef2);
            map.RowDefinitions.Add(rowDef3);
            map.RowDefinitions.Add(rowDef4);

            Rectangle r = new Rectangle();
            r.Fill = Brushes.DeepSkyBlue;
            map.Children.Add(r);

            Grid.SetColumn(r,0);
            Grid.SetRow(r, 0);
            //map.Background = ;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
