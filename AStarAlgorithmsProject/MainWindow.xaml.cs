﻿using System;
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

        Dictionary<Rectangle, Tile> recTiles; // Used to have O(1) acess to a tile from a given rectangle
        Rectangle[,] tiles;                     //Collection of Rects that function as the map tiles
        Brush mapBrush = Brushes.DarkOliveGreen;//The brush to color rectangles with, defaulted to start.

        //brushes that hold colors (read only)
        Brush defaultColor = Brushes.LightGray; //Default color of tiles
        Brush startColor = Brushes.DarkOliveGreen;
        Brush goalColor = Brushes.Maroon;
        Brush wallColor = Brushes.Black;
        Brush pathColor = Brushes.SteelBlue;
        Brush difficultSand = Brushes.SandyBrown;
        Brush difficultMud = Brushes.SaddleBrown;
        Brush difficultBrush = Brushes.SandyBrown; //Holds current difficulty

        private bool startExists = false;
        private bool goalExists = false;

        Solver solver;
        TileMap tileMap;

        List<Point> path;

        int selectAlgo = 0;     // Holds which algorithm to run. (0,A*) (1,Djikstra) (2,Greedy)
        int sandCost = 2;
        int mudCost = 3;

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

            path = new List<Point>();

            tileMap = new TileMap(size);
            //tileMap.InitMap();

            recTiles = new Dictionary<Rectangle, Tile>();

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
                    tileMap[i, j] = new Tile(new Point(i, j));
                    recTiles.Add(tiles[i, j], tileMap.Get(i, j));
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
            Tile tile;
            recTiles.TryGetValue(r, out tile);

            if (tile == null)
            {
                return;
            }

            ResetPathOnClick();

            //Prevents duplication of start tile
            if (mapBrush.Equals(startColor))
            {
                if (tile == tileMap.Goal)
                {
                    goalExists = false;
                    tileMap.Goal = null;
                }
                if (!tile.Passable)
                {
                    tile.Passable = true;
                }
                if (startExists == true)
                {
                    tiles[tileMap.Start.Location.X, tileMap.Start.Location.Y].Fill = defaultColor;
                }
                tileMap.SetStart(tile.Location);
                tileMap.SetMovementCost(tile.Location, 1);
                r.Fill = startColor;
                startExists = true;
            }

            //Prevents duplication of goal tile
            else if (mapBrush.Equals(goalColor))
            {
                if (tile == tileMap.Start)
                {
                    startExists = false;
                    tileMap.Start = null;
                }
                if (!tile.Passable)
                {
                    tile.Passable = true;
                }
                if (goalExists == true)
                {
                    tiles[tileMap.Goal.Location.X, tileMap.Goal.Location.Y].Fill = defaultColor;
                }
                tileMap.SetGoal(tile.Location);
                tileMap.SetMovementCost(tile.Location, 1);
                r.Fill = goalColor;
                goalExists = true;
            }

            else if (mapBrush.Equals(wallColor)) // make an impassible tile (cant be 
            {
                if (tile != tileMap.Goal && tile != tileMap.Start)
                {
                    tile.Passable = false;
                    r.Fill = wallColor;
                }
            }

            else if (mapBrush.Equals(difficultBrush)) // set tile cost and color pased on the terrain type
            {

                if (tile == tileMap.Goal)
                {
                    goalExists = false;
                    tileMap.Goal = null;
                }
                else if (tile == tileMap.Start)
                {
                    startExists = false;
                    tileMap.Start = null;
                }

                if (!tile.Passable)
                {
                    tile.Passable = true;
                }
                tile.CostScalar = difficultBrush == difficultSand ? sandCost : mudCost;
                r.Fill = SetRectangleFillByCost(tile);
            }

            else if (mapBrush.Equals(defaultColor)) // "delete" a tile (reset it back to a normal tile)
            {
                if (tile == tileMap.Goal)
                {
                    goalExists = false;
                    tileMap.Goal = null;
                }
                else if (tile == tileMap.Start)
                {
                    startExists = false;
                    tileMap.Start = null;
                }

                tile.CostScalar = 1;

                if (!tile.Passable)
                {
                    tile.Passable = true;
                }

                r.Fill = defaultColor;
            }
        }

        public void ResetPathOnClick()
        {
            if (path.Count != 0)
            {
                foreach (Point p in path)
                {
                    tiles[p.X, p.Y].Fill = SetRectangleFillByCost(tileMap.Get(p));
                }
            }

            path.Clear();
        }

        public Brush SetRectangleFillByCost(Tile t)
        {
            Brush result;

            switch (t.CostScalar)
            {
                case 2:
                    result = difficultSand;
                    break;
                case 3:
                    result = difficultMud;
                    break;
                default:
                    result = defaultColor;
                    break;
            }

            return result;
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
                else if (rb.Content.Equals("Cost"))
                {
                    mapBrush = difficultBrush;
                }
                else if (rb.Content.Equals("Default"))
                {
                    mapBrush = defaultColor;
                }
            }
            else if (rb.GroupName.Equals("Algo"))
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
        /// Changes the difficulty brush to be the correct color, to be assigned to the mapBrush.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_Changed(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            if (combo == null)
                return;
            Brush temp = difficultBrush;

            if (combo.SelectedIndex == 0)
            {
                difficultBrush = difficultSand;
            }
            else
            {
                difficultBrush = difficultMud;
            }

            if(mapBrush.Equals(temp))
            {
                mapBrush = difficultBrush;
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

            r = new RadioButton();
            Grid.SetColumn(r, size + 1);
            Grid.SetRow(r, 5);
            map.Children.Add(r);
            r.Content = "Default";
            r.GroupName = "Tile";
            r.Checked += Radio_Changed;

            //Allows users to select cost of tile
            ComboBox cb = new ComboBox();
            Grid.SetColumn(cb, size + 1);
            Grid.SetRow(cb, 4);
            map.Children.Add(cb);
            TextBox tb;
            for(int i = 2; i < 4; i++)
            {
                tb = new TextBox();
                tb.Text = i.ToString();
                cb.Items.Add(tb);
            }
            cb.SelectionChanged += Box_Changed;
            cb.SelectedIndex = 0; 
            
            //Map controls
            Button b = new Button();
            Grid.SetColumn(b, size + 1);
            Grid.SetRow(b, 6);
            b.Content = "Submit";
            map.Children.Add(b);
            b.Click += Submit_Clicked;

            b = new Button();
            Grid.SetColumn(b, size + 1);
            Grid.SetRow(b, 7);
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
            ResetPathOnClick();

            if (goalExists && startExists)
            {
                switch (selectAlgo)
                {
                    case 0:
                        solver = new AStarSolver();
                        break;
                    case 1:
                        solver = new DijkstraSolver();
                        break;
                    case 2:
                        solver = new GreedySolver();
                        break;
                }

                //  Read_Map();
                path = solver.getPath(tileMap);
                Print_Path();
            }
        }

        /// <summary>
        /// Changes the color of the path tiles.
        /// </summary>
        private void Print_Path()
        {
            if (path.Count != 0)
            {
                path.RemoveAt(0);
                path.RemoveAt(path.Count - 1);
                foreach (Point p in path)
                {
                    tiles[p.X, p.Y].Fill = pathColor;
                }
            }
        }


        /// <summary>
        /// Returns the map to the default color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Map(object sender, EventArgs e)
        {
            if (path.Count != 0)
            {
                path.Clear();
            }

            foreach (Rectangle t in tiles)
            {
                t.Fill = defaultColor;
                recTiles[t].Passable = true;
                recTiles[t].CostScalar = 1;
            }

            startExists = false;
            goalExists = false;
        }
    }
}
