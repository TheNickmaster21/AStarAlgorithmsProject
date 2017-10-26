using System;
using System.Collections.Generic;

namespace AStarAlgorithmsProject
{

    // Tile Def and Methods
    /// <summary>
    /// Tile class for use only by the AStarSolver
    /// Static values hold on to the start, goal, and current places of the path
    /// Each tile knows the cost to get to and from it, and it's total cost.
    /// A tile knows its location on the map and its parent in the path.
    /// </summary>
    public class Tile
    {
        public static Tile Start; // the starting location the path is comming from
        public static Tile Goal; // the exit location the path must reach
        public static Tile Current; // the current location on the path while tring to get from start to goal

        public Tile Parent; // the adjacent tile preceeding this one in the calculated path

        private int costScalar; // not really used right now, but can be used to indicate any extra cost a it takes to get to a tile. Like difficult terrain costing twice as much movement.
        private bool passable; // if this tile on the map cannot be move to or through, this is false
        private Point location; // the tiles location in the 2d map array
        private TileStates state; // instance of the enum TileStates to indicate if it's been closed, open, or unchecked by the algorithim

        //Default constructor, most of the time, only the minimum point should be provided, and all other parameters left to their default value
        public Tile(Point l, bool p = true, TileStates t = TileStates.Unchecked, int cS = 1)
        {
            location = l;
            passable = p;
            state = t;
            costScalar = cS;
        }

        //Accessor Methods
        public bool Passable
        { get { return passable; } set { passable = value; } }
        public Point Location
        { get { return location; } }
        public TileStates State
        { get { return state; } set { state = value; } }
        public int CostScalar
        { get { return costScalar; } set { costScalar = value; } }

        //Tile Heuristics 
        public double Distance2Goal // the "h(x)" value of the heuristics. Or "cost to get there"
        { get { return Point.Distance(location, Tile.Goal.location); } }
        public double Distance2Start // the "g(x)" value of the huristics. Or "cost to get here"
        { get { return Point.Distance(location, Tile.Start.location); } }
        public double Cost // the "f(x)" value of the heuristic. Or "cost of path"
        { get { return Distance2Start * costScalar + Distance2Goal; } }

        public static IComparer<Tile> CostSort = new CostSortComparer();

        private class CostSortComparer : IComparer<Tile>
        {
            public int Compare(Tile t1, Tile t2)
            {
                return Math.Sign(t1.Cost - t2.Cost);
            }
        }  // Simple class to be used by the Sort method of the List class. 
    }

}
