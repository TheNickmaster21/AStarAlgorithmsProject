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
        public Tile Parent; // the adjacent tile preceeding this one in the calculated path

        private Point location; // the tiles location in the 2d map array
        private double costFromStart; // Used to track how much distance this tile has to the start
        private int costScalar; // not really used right now, but can be used to indicate any extra cost a it takes to get to a tile. Like difficult terrain costing twice as much movement.
        private bool passable; // if this tile on the map cannot be move to or through, this is false
        private TileStates state; // instance of the enum TileStates to indicate if it's been closed, open, or unchecked by the algorithim

        //Default constructor
        public Tile(Point location, double costFromStart = 0, bool passible = true, TileStates state = TileStates.Unchecked, int cost = 1)
        {
            this.location = location;
            this.costFromStart = costFromStart;
            this.passable = passible;
            this.state = state;
            this.costScalar = cost;
        }

        //Accessor Methods
        public Point Location
        { get { return location; } }
        public bool Passable
        { get { return passable; } set { passable = value; } }
        public TileStates State
        { get { return state; } set { state = value; } }
        public int CostScalar
        { get { return costScalar; } set { costScalar = value; } }
        public double CostFromStart
        { get { return costFromStart; } set { costFromStart = value; } }
    }
}
