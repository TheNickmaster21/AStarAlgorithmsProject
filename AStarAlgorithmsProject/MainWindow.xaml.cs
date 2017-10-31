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
        int size;                       // length and width of the map
        Grid map;                       // Grid object to hold tiles
        List<ColumnDefinition> columns;
        List<RowDefinition> rows;

        Rectangle[,] tiles;                     //Collection of Rects that function as the map tiles
        Brush mapBrush = Brushes.DarkOliveGreen;//The brush to color rectangles with, defaulted to start.

        //brushes that hold colors (read only)
        Brush defaultColor = Brushes.LightGray; //Default color of tiles
        Brush startColor = Brushes.DarkOliveGreen;
        Brush goalColor = Brushes.Maroon;
        Brush wallColor = Brushes.Black;
        Brush pathColor = Brushes.SteelBlue;

        private bool startExists = false;
        private bool goalExists = false;

        AStarSolver solver;
        List<Point> path;
        Point start;
        Point goal;

        public MainWindow()
        {
            InitializeComponent();

            size = 10;
            Set_Up_Map(size);
            Create_Buttons();

            this.Title = "Project";
            this.Content = map;
        }

        /// <summary>
        /// Generates the grid (size x size) and fills it with default rectangles.
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
                    tiles[i, j].Fill = defaultColor;
                    tiles[i, j].MouseDown += Mouse_Clicked;
                    map.Children.Add(tiles[i, j]);
                    Grid.SetColumn(tiles[i, j], i);
                    Grid.SetRow(tiles[i, j], j);
                }
        }

        /// <summary>
        /// Triggers when one of the rectangles is clicked.
        /// Changes the color of the tile to mapBrush or default.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Mouse_Clicked(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = (Rectangle)sender;
            if (r == null)
                return;

            //Prevents duplication of start tile
            if (mapBrush.Equals(startColor))
            {
                if (startExists == true)
                {
                    foreach (Rectangle t in tiles)
                    {
                        if (t.Fill.Equals(startColor))
                        {
                            t.Fill = defaultColor;
                        }
                    }
                }
                startExists = true;
            }

            //Prevents duplication of goal tile
            if (mapBrush.Equals(goalColor))
            {
                if (goalExists == true)
                {
                    foreach (Rectangle t in tiles)
                    {
                        if (t.Fill.Equals(goalColor))
                        {
                            t.Fill = defaultColor;
                        }
                    }
                }
                goalExists = true;
            }

            //Allows deletion of tiles (only works for black tiles, don't know why)
            if (mapBrush.Equals(r.Fill))
                r.Fill = defaultColor;
            else
                r.Fill = mapBrush;
        }

        /// <summary>
        /// Changes the color of the brush based on the radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Radio_Changed(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb == null)
                return;

            if (rb.Content.Equals("Start"))
            {
                mapBrush = startColor;
            }
            else if (rb.Content.Equals("Goal"))
            {
                mapBrush = goalColor;
            }
            else if (rb.Content.Equals("Wall"))
            {
                mapBrush = wallColor;
            }
        }


        /// <summary>
        /// Creates the Radio and Submit buttons then attaches them to the grid.
        /// </summary>
        private void Create_Buttons()
        {
            RadioButton r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 0);
            map.Children.Add(r);
            r.Content = "Start";
            r.Checked += Radio_Changed;
            r.IsChecked = true;

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

            Button b = new Button();
            Grid.SetColumn(b, size + 1);
            Grid.SetRow(b, 3);
            b.Content = "Submit";
            map.Children.Add(b);
            b.Click += Submit_Clicked;

            b = new Button();
            Grid.SetColumn(b, size + 1);
            Grid.SetRow(b, 4);
            b.Content = "Reset";
            map.Children.Add(b);
            b.Click += Reset_Map;

        }
        
        /// <summary>
        /// Creates a new AStarSolver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Submit_Clicked(object sender, EventArgs e)
        {
            solver = new AStarSolver(size);
            Read_Map();
            path = solver.GetPath(start, goal);
            Print_Path();
        }

        /// <summary>
        /// Sets walls to impassable, and gets the start and goal points
        /// </summary>
        private void Read_Map()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (tiles[i, j].Fill.Equals(wallColor))
                    {
                        solver.SetPassable(new Point(i, j), false);
                    }

                    if (tiles[i, j].Fill.Equals(startColor))
                    {
                        start = new Point(i, j);
                    }

                    if (tiles[i, j].Fill.Equals(goalColor))
                    {
                        goal = new Point(i, j);
                    }
                }
        }

        /// <summary>
        /// Changes the color of the path tiles.
        /// </summary>
        private void Print_Path()
        {
            foreach(Point p in path)
            {
                tiles[p.X, p.Y].Fill = pathColor;
            }
        }


        /// <summary>
        /// Returns the map to the default color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Map(object sender, EventArgs e)
        {
            foreach (Rectangle t in tiles)
            {
                t.Fill = defaultColor;
            }
        }
    }
}
