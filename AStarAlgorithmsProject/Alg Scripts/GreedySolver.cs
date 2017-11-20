using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    class GreedySolver : Solver
    {

        /// <summary>
        /// finds a path via greedy
        /// </summary>
        /// <returns></returns>
        override public bool Solve(TileMap tileMap)
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
                foreach (Tile neighbor in FindNeighbors(tileMap))
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
        private List<Tile> FindNeighbors(TileMap tileMap)
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
    }
}
