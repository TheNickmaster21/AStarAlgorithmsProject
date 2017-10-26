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
        Canvas parent;
        Grid map;
        List<ColumnDefinition> columns;
        List<RowDefinition> rows;
        Rectangle[,] tiles;

        public MainWindow()
        {
            InitializeComponent();
            /*textBox.IsReadOnly = true;
            textBox.TextAlignment = TextAlignment.Center;

            main = new MainDriver(); // creates an instance of Main Drive for us to reference whenever the user changes something about the display and get results from.
            textBox.Text = main.TestMap(); // shows results of the algorithim, currenttly uses hardcoded start and end positions.
        */
            
            int size = 10;
            parent = new Canvas();
            InitializeBackground(parent);
            Set_Up_Map(size);
            Create_Buttons();
            
            this.Content = parent;
        }

        void InitializeBackground (Canvas c)
        {
            c.Width = 1;
            c.Height = 1;
            c.HorizontalAlignment = HorizontalAlignment.Center;
            c.VerticalAlignment = VerticalAlignment.Center;

        }

        /// <summary>
        /// Generates the grid (size x size) and fills it with rectangles.
        /// </summary>
        /// <param name="size"></param>
        void Set_Up_Map(int size)
        {
            map = new Grid();
            parent.Children.Add(map);
            map.Background = Brushes.Black;
            //map.HorizontalAlignment = HorizontalAlignment.Center;
            //map.VerticalAlignment = VerticalAlignment.Center;
            map.ShowGridLines = true;
            map.Height = 500;
            map.Width = 500;

            columns = new List<ColumnDefinition>();
            rows = new List<RowDefinition>();
            tiles = new Rectangle[size, size];

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
                    tiles[i, j] = new Rectangle();
                    tiles[i, j].Fill = Brushes.LightGray;
                    tiles[i, j].MouseDown += Mouse_Clicked;
                    map.Children.Add(tiles[i, j]);
                    Grid.SetColumn(tiles[i, j], i);
                    Grid.SetRow(tiles[i, j], j);
                }
            
        }

        void Mouse_Clicked(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = (Rectangle)sender;
            r.Fill = Brushes.DarkOliveGreen;
               
        }

        private void Create_Buttons()
        {
            RadioButton r = new RadioButton();
            parent.Children.Add(r);
            r.HorizontalAlignment = HorizontalAlignment.Right;
            r.VerticalAlignment = VerticalAlignment.Center;
            r.Content = "Start";

        }
    }
}
