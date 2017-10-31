using System.Collections.Generic;

namespace AStarAlgorithmsProject
{
    partial class AStarSolver
    {
        private TileMap tileMap; // the map A* will traverse and the tiles therin
        private List<KeyValuePair<double, Tile>> openTiles; // the queue of tiles being evaluated and their estimated cost

        public AStarSolver(int mS) // default constructor where mS is the number of rows/columns in the map
        {
            tileMap = new TileMap(mS);
            tileMap.InitMap();
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

        // Starting at the goal, traced back through the parents of each tile until it reachest the start tile, and returns a 
        // list of the points on the path in the order of start -> finish
        public List<Point> GetPath(Point s, Point g)
        {
            List<Point> p = new List<Point>();

            tileMap.Start = tileMap.Get(s);
            tileMap.Goal = tileMap.Get(g);

            if (/*Solve()*/ New_Solve())
            {

                Tile cursor = tileMap.Goal;
                while (cursor != null)
                {
                    p.Add(cursor.Location);
                    cursor = cursor.Parent;
                }
                p.Reverse();

            }

            return p;
        }


        /* Brandon's Improved A* Algorithim From Capstone*/
        private bool New_Solve()
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

                foreach (Tile neighbor in TracePath())
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

        /// <summary>
        /// The logic for A* traversal
        /// </summary>
        /// <returns>returns true if a valid path was found</returns>
        private bool Solve()
        {
            openTiles = new List<KeyValuePair<double, Tile>>();
            addTile(tileMap.Start);

            while (openTiles.Count != 0) // Continues until either there are no more tiles to evaluate, or a solution has been found
            {
                openTiles.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

                tileMap.Current = openTiles[0].Value; // the current tile is now the most optimal tile
                tileMap.Current.State = TileStates.Closed; // close the tile so it is not evaluated again

                openTiles.RemoveAt(0); // remove from the list of open tiles.

                if (tileMap.Current == tileMap.Goal) // if we have reached our goal exit the loop
                {
                    return true;
                }

                List<Tile> neighbors = TracePath(); // evaluate the adjacent tiles, opening them as needed, and building our path based on the least cost to traverse to each one.

                foreach (Tile neighbor in neighbors)
                {
                    if (neighbor.State == TileStates.Unchecked) // if it hasnt been checked before, we need only add it to the list of open tiles 
                    {
                        neighbor.State = TileStates.Open;
                        neighbor.Parent = tileMap.Get(tileMap.Current.Location); // go set its parent to be the first tile to disover it by default
                        addTile(neighbor, Point.Distance(neighbor.Location, neighbor.Parent.Location));
                    }
                    else // if the neighbor is already open, we now need to see if its an ideal node to travel to
                    {
                        double gNew; // equal to the sum of the g(x) of it's parent (aka the direct g cost of the parent) and the cost it would take to get to it from the parent tile.
                        gNew = neighbor.Parent.CostFromStart + Point.Distance(neighbor.Location, neighbor.Parent.Location) * neighbor.CostScalar;

                        if (gNew < neighbor.CostFromStart) // if the gNew cost is a better deal the cost to get to neighbor from start, put this node on the path
                        {
                            neighbor.Parent = tileMap.Get(tileMap.Current.Location);
                        }
                    }
                }
            }

            return false;
        }

        //Builds the traversal path bassed on the most ideal canidate of the current tile
        private List<Tile> TracePath()
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

        // Method to add tiles to the openTiles construct, movingDistance defualts to up/down or left/right movement
        private void addTile(Tile tile, double movingDistance = 1)
        {
            openTiles.Add(new KeyValuePair<double, Tile>(estimateTileCost(tile), tile));
        }

        private double estimateTileCost(Tile tile, double movingDistance = 1)
        {
            return tile.CostFromStart + tile.CostScalar + tileMap.DistanceToGoal(tile);
        }
    }
}
