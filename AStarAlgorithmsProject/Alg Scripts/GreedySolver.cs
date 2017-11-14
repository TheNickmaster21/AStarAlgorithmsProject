using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    class GreedySolver
    {
        private TileMap tileMap;
        private List<KeyValuePair<double, Tile>> openTiles;

        public GreedySolver(int mapSize)
        {
            tileMap = new TileMap(mapSize);
            tileMap.InitMap();
        }

        /// <summary>
        /// Calls the solve method then reorders the list of points to the correct path order
        /// </summary>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public List<Point> getPath(Point start, Point goal)
        {
            List<Point> path = new List<Point>();

            tileMap.Start = tileMap.Get(start);
            tileMap.Goal = tileMap.Get(goal);

            if (SolveGreedy())
            {
                Tile cursor = tileMap.Goal;
                while (cursor != null)
                {
                    path.Add(cursor.Location);
                    cursor = cursor.Parent;
                }
                path.Reverse();
            }

            return path;
        }

        /// <summary>
        /// finds a path via greedy
        /// </summary>
        /// <returns></returns>
        private bool SolveGreedy()
        {
            Dictionary<Tile, double> costSoFar = new Dictionary<Tile, double>();
            P_Queue<Tile> openTiles = new P_Queue<Tile>();

            openTiles.Enqueue(tileMap.Start, 0);
            costSoFar[tileMap.Start] = 0;

            while (openTiles.Count != 0)
            {
                tileMap.Current = openTiles.Dequeue();
                tileMap.Current.State = TileStates.Closed;

                if (tileMap.Current.Equals(tileMap.Goal))
                    return true;

                double newCost;
                foreach (Tile neighbor in FindNeighbors())
                {
                    newCost = tileMap.DistanceToGoal(neighbor);

                    if (neighbor.State == TileStates.Unchecked || newCost < costSoFar[neighbor])
                    {
                        neighbor.State = TileStates.Open;

                        costSoFar[neighbor] = newCost;
                        openTiles.Enqueue(neighbor, costSoFar[neighbor]);
                        neighbor.Parent = tileMap.Current;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Finds neighbors of the current tile
        /// </summary>
        /// <returns></returns>
        private List<Tile> FindNeighbors()
        {
            List<Tile> result = new List<Tile>();
            int neighborX = 0;
            int neighborY = 0;

            foreach (Point p in SolverUtils.GetAdvancedDirections()) // looks at a possible cordinate for every tile neighboring the current tile
            {
                neighborX = tileMap.Current.Location.X + p.X;
                neighborY = tileMap.Current.Location.Y + p.Y;
                Point newP = new Point(neighborX, neighborY);

                if (tileMap.TileValid(newP)) // continue to evaluate only if the tile is on the map
                {
                    Tile neighbor = tileMap.Get(newP); // the tile being evaluated
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

        // Sets the a tile at location l to the value of p
        public void SetPassable(Point l, bool p)
        {
            tileMap.Get(l).Passable = p;
        }

        // Returns if the tile at the location is passible
        public bool GetPassable(Point l)
        {
            return tileMap.Get(l).Passable;
        }

        // Sets the scalar cost of a tile at point l to the value of c
        public void SetMovementCost(Point l, int c)
        {
            tileMap.Get(l).CostScalar = c;
        }
    }
}
