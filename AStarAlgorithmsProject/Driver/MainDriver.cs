using System.Collections.Generic;

namespace AStarAlgorithmsProject
{
    /// <summary>
    /// Class for manipulating the funtionality of the program. Such as updating the grid we are pathing through, and passing it to the algorithim 
    /// </summary>
    class MainDriver
    {
        AStarSolver solver; // I know what I did. 

        //DijkstraSolver solver; //Not sure how we want to handle the different solvers so it's here for now

        Point[,] points; // The cordinates for whatever interactable map we decide on.

        public MainDriver(int size = 10) // default constructor. Currently, design does not support resizing the map during run time
        {
            solver = new AStarSolver(size);

            //solver = new DijkstraSolver(size);

            points = new Point[size, size];

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    points[i, j] = new Point(i, j);
                }
            }

            // hard coding impassable terrain - testing use only
            solver.SetPassable(new Point(5, 3), false);
            solver.SetPassable(new Point(5, 4), false);
            solver.SetPassable(new Point(5, 5), false);
            solver.SetPassable(new Point(5, 6), false);
            solver.SetPassable(new Point(4, 6), false);
            solver.SetPassable(new Point(3, 6), false);
            solver.SetPassable(new Point(2, 6), false);

            solver.SetPassable(new Point(8, 8), false);
            solver.SetPassable(new Point(8, 9), false);
            //  aSS.SetPassalbe(new Point(9, 8), false); //Uncomment to see a map where no solution exsists based on the start and end in PrintMap
        }

        //
        // Methods for, like, changing what points are blocked or their cost and so on.
        //

        // Test method for displaying the map and the path A* tool to get the the end point.
        public string TestMap()
        {
            // Hard coded test values
            Point start = new Point(0, 0);
            Point end = new Point(9, 9);
            // end test values

            string data = "";
            string pathPoints = "";

            List<Point> path = solver.GetPath(start, end); // what calls the A* algorthim and returns a list of all point in the solution path. Should be empty if no solution exsists.

            //List<Point> path = solver.getDijkstraPath(start, end); //returns the path found via dijkstra

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    Point p = points[i, j];
                    int index = path.IndexOf(p);
                    if (index > -1)
                    {
                        if (index < 10)
                            data += "[ " + path.IndexOf(p) + "]";
                        else
                            data += "[" + path.IndexOf(p) + "]";
                    }
                    else if (!solver.GetPassable(p))
                    {
                        data += "[XX]";

                    }
                    else
                    {
                        data += "[  ]";
                    }
                }

                data += "\n";
            }

            foreach (Point p in path)
            {
                pathPoints += "(" + p.X + "," + p.Y + ")\n";
            }

            data += pathPoints;

            return data;
        }

    }
}
