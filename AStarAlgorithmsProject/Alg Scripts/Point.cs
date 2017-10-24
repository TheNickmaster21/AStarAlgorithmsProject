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

        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        // Accessor Methods
        public int X
        { get { return x; } set { x = value; } }
        public int Y
        { get { return y; } set { y = value; } }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return point != null &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = -624234986;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

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
