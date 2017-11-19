using System.Collections.Generic;

namespace AStarAlgorithmsProject
{
    abstract class Solver
    {
        public List<Point> getPath(TileMap tileMap)
        {
            List<Point> path = new List<Point>();

            if (Solve(tileMap))
            {
                Tile cursor = tileMap.Goal;
                while (cursor != null)
                {
                    path.Add(cursor.Location);
                    cursor = cursor.Parent;
                }
                path.Reverse();
            }
            tileMap.ResetTraversal();
            return path;
        }

        abstract public bool Solve(TileMap tileMap);
    }
}
