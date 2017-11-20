using System.Collections.Generic;

namespace AStarAlgorithmsProject
{
    partial class AStarSolver : Solver
    {

        /* Brandon's Improved A* Algorithim From Capstone*/
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
                {
                    return true;
                }

                foreach (Tile neighbor in TracePath(tileMap))
                {
                    double nCost = costSoFar[tileMap.Current] + Point.Distance(tileMap.Current.Location, neighbor.Location) * neighbor.CostScalar;

                    if (neighbor.State == TileStates.Unchecked || nCost < costSoFar[neighbor])
                    {
                        neighbor.State = TileStates.Open;

                        costSoFar[neighbor] = nCost;
                        double prio = nCost + Point.Distance(neighbor.Location, tileMap.Goal.Location);
                        openTiles.Enqueue(neighbor, prio);
                        neighbor.Parent = tileMap.Current;
                    }
                }
            }
            return false;
        }
        /* Brandon's Improved A* Algorithim From Capstone*/

        //Builds the traversal path bassed on the most ideal canidate of the current tile
        private List<Tile> TracePath(TileMap tileMap)
        {
            List<Tile> result = new List<Tile>();
            int nx = 0;
            int ny = 0;

            foreach (Point p in SolverUtils.GetAdvancedDirections()) // looks at a possible cordinate for every tile neighboring the current tile
            {
                nx = tileMap.Current.Location.X + p.X;
                ny = tileMap.Current.Location.Y + p.Y;
                Point newP = new Point(nx, ny);

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
