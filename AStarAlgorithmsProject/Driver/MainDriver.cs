using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    /// <summary>
    /// Class for manipulating the funtionality of the program. Such as updating the grid we are pathing through, and passing it to the algorithim 
    /// </summary>
    class MainDriver
    {
        AStarSolver aSS; // I know what I did. 
        Point[,] points; // The cordinates for whatever interactable map we decide on.

        public MainDriver(int size = 10) // default constructor. Currently, design does not support resizing the map during run time
        {
            aSS = new AStarSolver(size);
            points = new Point[size, size];

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    points[i, j] = new Point(i, j);
                }
            }

            // hard coding impassable terrain - testing use only
            aSS.SetPassalbe(new Point(8, 8), false);
            aSS.SetPassalbe(new Point(8, 9), false);
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

            List<Point> path = aSS.GetPath(start,end); // what calls the A* algorthim and returns a list of all point in the solution path. Should be empty if no solution exsists.

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    bool pathPoint = false;
                    foreach (Point p in path)
                    {
                        if (p.X == points[i, j].X && p.Y == points[i, j].Y)
                        {
                            data += "[X]";
                            pathPoint = true;
                            break;
                        }
                    }
                    if (!pathPoint)
                    {
                        data += "[O]";
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
