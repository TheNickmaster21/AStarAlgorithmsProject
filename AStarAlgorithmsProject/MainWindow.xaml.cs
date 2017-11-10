using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AStarAlgorithmsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        AStarSolver Asolver;
        DijkstraSolver Dsolver;
        // Greedy solver    Fill in lines ~ 305 & 326
        List<Point> path;
        Point start;
        Point goal;

        int selectAlgo = 0;     // Holds which algorithm to run. (0,A*) (1,Djikstra) (2,Greedy)

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
            map.Height = 650;
            map.Width = 650;
            

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
            if (rb.GroupName.Equals("Tile"))
            {
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
            else if(rb.GroupName.Equals("Algo"))
            {
                if (rb.Content.Equals("A*"))
                {
                    selectAlgo = 0;
                }
                else if (rb.Content.Equals("Djikstra"))
                {
                    selectAlgo = 1;
                }
                else if (rb.Content.Equals("Greedy"))
                {
                    selectAlgo = 2;
                }
            }
        }


        /// <summary>
        /// Creates the Radio and Submit buttons then attaches them to the grid.
        /// </summary>
        private void Create_Buttons()
        {
            //Right Hand buttons
            //Tile controls
            RadioButton r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 0);
            map.Children.Add(r);
            r.Content = "Start";
            r.GroupName = "Tile";
            r.Checked += Radio_Changed;
            r.IsChecked = true;
            
            r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 1);
            map.Children.Add(r);
            r.Content = "Goal";
            r.GroupName = "Tile";
            r.Checked += Radio_Changed;

            r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 2);
            map.Children.Add(r);
            r.Content = "Wall";
            r.GroupName = "Tile";
            r.Checked += Radio_Changed;

            r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 3);
            map.Children.Add(r);
            r.Content = "Cost";
            r.GroupName = "Tile";
            r.Checked += Radio_Changed;

            //Allows users to select cost of tile
            ComboBox cb = new ComboBox();
            Grid.SetColumn(cb, size + 1);
            Grid.SetRow(cb, 4);
            map.Children.Add(cb);
            TextBox tb;
            for(int i = 0; i < 4; i++)
            {
                tb = new TextBox();
                tb.Text = i.ToString();
                cb.Items.Add(tb);
            }
            cb.SelectedIndex = 1; 
            
            //Map controls
            Button b = new Button();
            Grid.SetColumn(b, size + 1);
            Grid.SetRow(b, 5);
            b.Content = "Submit";
            map.Children.Add(b);
            b.Click += Submit_Clicked;

            b = new Button();
            Grid.SetColumn(b, size + 1);
            Grid.SetRow(b, 6);
            b.Content = "Reset";
            map.Children.Add(b);
            b.Click += Reset_Map;

            //Algorithm controls
            r = new RadioButton();
            Grid.SetColumn(r, 0);
            Grid.SetRow(r, size + 1);
            map.Children.Add(r);
            r.Content = "A*";
            r.GroupName = "Algo";
            r.IsChecked = true;
            r.Checked += Radio_Changed;

            r = new RadioButton();
            Grid.SetColumn(r, 1);
            Grid.SetRow(r, size + 1);
            map.Children.Add(r);
            r.Content = "Djikstra";
            r.GroupName = "Algo";
            r.Checked += Radio_Changed;

            r = new RadioButton();
            Grid.SetColumn(r, 2);
            Grid.SetRow(r, size + 1);
            map.Children.Add(r);
            r.Content = "Greedy";
            r.GroupName = "Algo";
            r.Checked += Radio_Changed;

        }
        
        /// <summary>
        /// Creates a new AStarSolver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Submit_Clicked(object sender, EventArgs e)
        {
            if (selectAlgo == 0)
            {
                Asolver = new AStarSolver(size);
                path = Asolver.GetPath(start, goal);
            }
            else if (selectAlgo == 1)
            {
                Dsolver = new DijkstraSolver(size);
                path = Dsolver.getDijkstraPath(start, goal);
            }
            else if (selectAlgo == 2)
            {
                //Gsolver = new solver... pather = gsolver.getpath
            }
            Read_Map();
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
                        if(selectAlgo == 0)
                            Asolver.SetPassable(new Point(i, j), false);
                        else if(selectAlgo == 1)
                            Dsolver.SetPassable(new Point(i, j), false);
                        else { }
                            //Greedy.SetPassable...
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
