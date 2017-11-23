using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject.Driver
{
    class TimeAnalyzer
    {

        static void Main(string[] args)
        {
            int mapSize = 100; //Change this to change the test map size

            TileMap tileMap = new TileMap(mapSize);
            tileMap.InitMap();
            tileMap.SetStart(new Point(1, 1));

            for (int i = 1; i < mapSize; i += 2)
            {
                for (int o = 0; o < mapSize; o++)
                {
                    if ((i + o) % mapSize != 0) //Make openings in the lines
                    {
                        tileMap.SetPassable(new Point(i, o), false);
                    }
                }
            }

            Solver solver = new AStarSolver(); //Change this to change which algorithm is used

            for (int i = 20; i < mapSize; i += 20) // You can mess with this to change how long the paths end up being. i should be a mutliple of 2
            {
                tileMap.SetGoal(new Point(i, i));

                System.Console.WriteLine();
                TileMap freshMap = new TileMap(tileMap);
                freshMap.ResetStates();
                System.Console.WriteLine(solver.getPath(freshMap).Count + " tiles long");

                short testTrials = 10; //Change this to change the sample size

                long total = 0;
                for (int x = 1; x <= testTrials; x++) 
                {
                    freshMap = new TileMap(tileMap);
                    freshMap.ResetStates();
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    solver.getPath(freshMap);
                    total += stopwatch.ElapsedMilliseconds;
                }

                System.Console.WriteLine(total / testTrials + " ms"); 
            }
        }
    }
}
