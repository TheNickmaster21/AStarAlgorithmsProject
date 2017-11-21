using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    class TileMap
    {
        public Tile Start; // the starting location the path is comming from
        public Tile Goal; // the exit location the path must reach
        public Tile Current; // the current location on the path while tring to get from start to goal

        private int size;
        private Tile[,] map;

        public TileMap(int size)
        {
            this.size = size;
            this.map = new Tile[size, size];
        }

        public TileMap(TileMap tileMap)
        {
            this.size = tileMap.size;
            this.map = new Tile[size, size];

            for (int x = 0; x < tileMap.map.GetLength(0); ++x)
            {
                for (int y = 0; y < tileMap.map.GetLength(1); ++y)
                {
                    this.map[x, y] = new Tile(tileMap.map[x, y]);
                }
            }

            this.SetStart(tileMap.Start.Location);
            this.SetGoal(tileMap.Goal.Location);
        }

        public int GetSize()
        {
            return size;
        }

        public Tile Get(Point l)
        {
            return map[l.X, l.Y];
        }

        public Tile Get(int x, int y)
        {
            return map[x, y];
        }

        public bool TileValid(Point p)
        {
            return (p.X >= 0 && p.X < this.GetSize()) && (p.Y >= 0 && p.Y < this.GetSize());
        } // overload of ValidTile to use a passed point parameter

        public Tile this[Point key]
        {
            get { return Get(key); }
            set { map[key.X, key.Y] = value; }
        }

        public Tile this[int i, int j]
        {
            get { return map[i, j]; }
            set { map[i, j] = value; }
        }

        // "Prime" the map so all tiles are in their default state and their locations set to the proper cordinates in the map
        public void InitMap()
        {
            for (int i = 0; i < map.GetLength(0); ++i)
            {
                for (int j = 0; j < map.GetLength(0); ++j)
                {
                    map[i, j] = new Tile(new Point(i, j));
                }
            }
        }

        public void ResetStates()
        {
            for (int i = 0; i < map.GetLength(0); ++i)
            {
                for (int j = 0; j < map.GetLength(0); ++j)
                {
                    map[i, j].State = TileStates.Unchecked;
                }
            }
        }

        public void ResetTraversal()
        {
            foreach (Tile t in map)
            {
                if (t.State == TileStates.Unchecked)
                {
                    continue;
                }

                t.State = TileStates.Unchecked;
                t.Parent = null;
            }
        }

        public double DistanceToGoal(Tile tile)
        {
            return Point.Distance(tile.Location, Goal.Location);
        }

        public void SetStart(Point l)
        {
            this.Start = Get(l);
        }

        public void SetGoal(Point l)
        {
            this.Goal = Get(l);
        }

        // Sets the a tile at location l to the value of p
        public void SetPassable(Point l, bool p)
        {
            this.Get(l).Passable = p;
        }

        // Returns if the tile at the location is passible
        public bool GetPassable(Point l)
        {
            return this.Get(l).Passable;
        }

        // Sets the scalar cost of a tile at point l to the value of c
        public void SetMovementCost(Point l, int c)
        {
            this.Get(l).CostScalar = c;
        }

        // Retunrs the cost of the tile
        public int GetMovementCost(Point l)
        {
            return this.Get(l).CostScalar;
        }

    }
}
