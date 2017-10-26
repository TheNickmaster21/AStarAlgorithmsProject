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
        int size;
        Grid map;
        List<ColumnDefinition> columns;
        List<RowDefinition> rows;

        Rectangle[,] tiles;
        Brush mapBrush = Brushes.DarkOliveGreen;

        private bool startExists = false;
        private bool goalExists = false;

        public MainWindow()
        {
            InitializeComponent();
            /*textBox.IsReadOnly = true;
            textBox.TextAlignment = TextAlignment.Center;

            main = new MainDriver(); // creates an instance of Main Drive for us to reference whenever the user changes something about the display and get results from.
            textBox.Text = main.TestMap(); // shows results of the algorithim, currenttly uses hardcoded start and end positions.
        */
            
            size = 10;
            Set_Up_Map(size);
            Create_Buttons();

            this.Title = "Project";
            this.Content = map;
        }

        /// <summary>
        /// Generates the grid (size x size) and fills it with rectangles.
        /// </summary>
        /// <param name="size"></param>
        void Set_Up_Map(int size)
        {
            map = new Grid();
            map.Background = Brushes.WhiteSmoke;
            map.HorizontalAlignment = HorizontalAlignment.Center;
            map.VerticalAlignment = VerticalAlignment.Center;
            map.ShowGridLines = true;
            map.Height = 500;
            map.Width = 500;

            columns = new List<ColumnDefinition>();
            rows = new List<RowDefinition>();
            tiles = new Rectangle[size, size];

            for (int i = 0; i < size + 1; i++)
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
            if (rows == null)
                return;

            Rectangle r = (Rectangle)sender;
            if (mapBrush.Equals(Brushes.DarkOliveGreen))
            {
                if (startExists == true)
                {
                    foreach (Rectangle t in tiles)
                    {
                        if (t.Fill.Equals(Brushes.DarkOliveGreen))
                        {
                            t.Fill = Brushes.LightGray;
                        }
                    }
                }
                startExists = true;
            }

            if (mapBrush.Equals(Brushes.Maroon))
            {
                if (goalExists == true)
                {
                    foreach (Rectangle t in tiles)
                    {
                        if (t.Fill.Equals(Brushes.Maroon))
                        {
                            t.Fill = Brushes.LightGray;
                        }
                    }
                }
                goalExists = true;
            }

            r.Fill = mapBrush;
               
        }

        void Radio_Changed(object sender,EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb == null)
                return;

            if(rb.Content.Equals("Start"))
            {
                mapBrush = Brushes.DarkOliveGreen;
            }
            else if(rb.Content.Equals("Goal"))
            {
                mapBrush = Brushes.Maroon;
            }
            else if (rb.Content.Equals("Wall"))
            {
                mapBrush = Brushes.Black;
            }
        }

        private void Create_Buttons()
        {
            RadioButton r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 0);
            map.Children.Add(r);
            r.Content = "Start";
            r.Checked += Radio_Changed;

            r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 1);
            map.Children.Add(r);
            r.Content = "Goal";
            r.Checked += Radio_Changed;

            r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 2);
            map.Children.Add(r);
            r.Content = "Wall";
            r.Checked += Radio_Changed;

        }
    }
}
