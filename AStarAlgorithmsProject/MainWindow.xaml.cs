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
        //MainDriver main;

        public MainWindow()
        {
            /*InitializeComponent();
            textBox.IsReadOnly = true;
            textBox.TextAlignment = TextAlignment.Center;

            main = new MainDriver(); // creates an instance of Main Drive for us to reference whenever the user changes something about the display and get results from.
            textBox.Text = main.TestMap(); // shows results of the algorithim, currenttly uses hardcoded start and end positions.
        */
            int size = 10;

            Grid map = new Grid();
            map.Background = Brushes.Black;
            map.HorizontalAlignment = HorizontalAlignment.Center;
            map.VerticalAlignment = VerticalAlignment.Center;
            map.ShowGridLines = true;
            map.Height = 500;
            map.Width = 500;

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            List<RowDefinition> rows = new List<RowDefinition>();
            Rectangle[,] tiles = new Rectangle[size,size];

            for (int i = 0; i < size; i++)
            {
                columns.Add(new ColumnDefinition());
                map.ColumnDefinitions.Add(columns[i]);

                rows.Add(new RowDefinition());
                map.RowDefinitions.Add(rows[i]);
            }

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                { 
                    tiles[i,j] = new Rectangle();
                    tiles[i,j].Fill = Brushes.LightGray;
                    tiles[i, j].MouseDown += Mouse_Clicked;
                    map.Children.Add(tiles[i,j]);
                    Grid.SetColumn(tiles[i, j], i);
                    Grid.SetRow(tiles[i, j],j);
                }

            this.Content = map;
        }

        void Mouse_Clicked(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
