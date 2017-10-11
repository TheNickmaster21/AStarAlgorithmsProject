using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    /// <summary>
    /// Simple point class to make all our coding lives a little less of a hassle
    /// Supplies the rules for finding distance between two points.
    /// </summary>
    class Point
    {
        private int x;
        private int y;

        public Point(int _x = 0, int _y = 0)
        {
            x = _x;
            y = _y;
        }

        // Accessor Methods
        public int X
        { get { return x; } set { x = value; } }
        public int Y
        { get { return y; } set { y = value; } }

        /// <summary>
        /// The euclidian distance between p1 and p2
        /// </summary>
        public static double Distance(Point p1, Point p2)
        {
            int deltaX = Math.Abs(p1.X - p2.X);
            int deltaY = Math.Abs(p1.Y - p2.Y);
            double d = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            return Math.Round(d, 3);
        }
    }
}
