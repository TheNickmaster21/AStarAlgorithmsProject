using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    enum TileStates { Unchecked, Open, Closed }; // enum to title and track the 3 states a tile can be in.

    static class SolverUtils
    {
        // in order: NW, N, NE, W, E, SW, S, SE
        public static Point[] GetAdvancedDirections()
        {
            return new Point[] {
            new Point(-1, 1), new Point(0, 1), new Point(1, 1),  new Point(-1, 0),
            new Point(1, 0),   new Point(-1, -1), new Point(0, -1), new Point(1, -1)
                                                };
        }

        // in order: N, W, E, S
        public static Point[] GetSimpleDirections()
        {
            return new Point[] {
            new Point(0, 1), new Point(-1, 0), new Point(1, 0), new Point(0, -1)
                                                };
        }
    }
}
