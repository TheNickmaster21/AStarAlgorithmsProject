using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AStarAlgorithmsProject
{
    partial class AStarSolver
    {
        private static int MapSize; // the total NxN size of the map being worked with.

        private Tile[,] map; // the map A* will traverse and the tiles therin
        private List<Tile> openTiles; // the queue of tiles being evaluated to find the desired path.

        public AStarSolver(int mS) // default constructor where mS is the number of rows/columns in the map
        {
            map = new Tile[mS, mS];
            MapSize = mS;
            InitMap();
        }

        // "Prime" the map so all tiles are in their default state and their locations set to the proper cordinates in the map
        private void InitMap()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    map[i, j] = new Tile(new Point(i, j));
                }
            }
        }

        // Sets the a tile at location l to the value of p
        public void SetPassable(Point l, bool p)
        {
            map[l.X, l.Y].Passable = p;
        }

        // Returns if the tile at the location is passible
        public bool GetPassable(Point l)
        {
            return map[l.X, l.Y].Passable;
        }

        // Sets the scalar cost of a tile at point l to the value of c
        public void SetMovementCost(Point l, int c)
        {
            map[l.X, l.Y].CostScalar = c;
        }

        // Starting at the goal, traced back through the parents of each tile until it reachest the start tile, and returns a 
        // list of the points on the path in the order of start -> finish
        public List<Point> GetPath(Point s, Point g)
        {
            List<Point> p = new List<Point>();

            Tile.Start = map[s.X, s.Y];
            Tile.Goal = map[g.X, g.Y];

            if (Solve())
            {

                Tile cursor = Tile.Goal;
                while (cursor != null)
                {
                    p.Add(cursor.Location);
                    cursor = cursor.Parent;
                }
                p.Reverse();

            }

            return p;
        }

        /// <summary>
        /// The logic for A* traversal
        /// </summary>
        /// <returns>returns true if a valid path was found</returns>
        private bool Solve()
        {
            SortTileByF sF = new SortTileByF(); // sorting logic for the tiles
            openTiles = new List<Tile>() { Tile.Start }; // the tile to be checked is the initial location

            while (openTiles.Count != 0) // Continues until either there are no more tiles to evaluate, or a solution has been found
            {
                openTiles.Sort(sF); // sor the list so the tiles with the lowest cost are in the front.
                Tile.Current = openTiles[0]; // the current tile is now the most optimal tile
                Tile.Current.State = TileStates.Closed; // close the tile so it is not evaluated again

                openTiles.RemoveAt(0); // remove from the list of open tiles.

                if (Tile.Current == Tile.Goal) // if we have reached our goal exit the loop
                {
                    return true;
                }

               List<Tile> neighbors = TracePath(); // evaluate the adjacent tiles, opening them as needed, and building our path based on the least cost to traverse to each one.

                foreach (Tile neighbor in neighbors)
                {
                    if (neighbor.State == TileStates.Unchecked) // if it hasnt been checked before, we need only add it to the list of open tiles 
                    {
                        neighbor.State = TileStates.Open;
                        neighbor.Parent = map[Tile.Current.Location.X, Tile.Current.Location.Y]; // go set its parent to be the first tile to disover it by default
                        openTiles.Add(neighbor);
                    }
                    else // if the neighbor is already open, we now need to see if its an ideal node to travel to
                    {
                        double gNew; // equal to the sum of the g(x) of it's parent (aka the direct g cost of the parent) and the cost it would take to get to it from the parent tile.
                        gNew = neighbor.Parent.Distance2Start + Point.Distance(neighbor.Location, neighbor.Parent.Location) * neighbor.CostScalar;

                        if (gNew < neighbor.Distance2Start) // if the gNew cost is a better deal the cost to get to neighbor from start, put this node on the path
                        {
                            neighbor.Parent = map[Tile.Current.Location.X, Tile.Current.Location.Y];
                        }
                    }
                }
            }

            return false;
        }

        private class SortTileByF : IComparer<Tile>
        {
            public int Compare(Tile t1, Tile t2) 
            {
                return Math.Sign(t1.Cost - t2.Cost);
            }
        }  // Simple class to be used by the Sort method of the List class. 

        public bool ValidTile(int x, int y)
        {
            return (x >= 0 && x < AStarSolver.MapSize) && (y >= 0 && y < AStarSolver.MapSize);
        } // returns true if the tile at the given location is within the bounds of the map

        public bool ValidTile(Point p)
        {
            return (p.X >= 0 && p.X < AStarSolver.MapSize) && (p.Y >= 0 && p.Y < AStarSolver.MapSize);
        } // overload of ValidTile to use a passed point parameter

        //Builds the traversal path bassed on the most ideal canidate of the current tile
        private List<Tile> TracePath() 
        {
            List<Tile> result = new List<Tile>();
            int nx = 0;
            int ny = 0;

            foreach (Point p in SolverUtils.GetAdvancedDirections()) // looks at a possible cordinate for every tile neighboring the current tile
            {
                nx = Tile.Current.Location.X + p.X;
                ny = Tile.Current.Location.Y + p.Y;

                if (ValidTile(nx, ny)) // continue to evaluate only if the tile is on the map
                {
                    Tile neighbor = map[nx, ny]; // the tile being evaluated
                    if (neighbor.Passable) // continue to evaluate only if it's a tile that can be mvoed to / on
                    {
                        if (neighbor.State != TileStates.Closed) // continue to evaluate if the tile has not already been closed 
                        {
                            result.Add(neighbor);
                        }
                    }
                }
            }
            return result;
        }
    }
}
