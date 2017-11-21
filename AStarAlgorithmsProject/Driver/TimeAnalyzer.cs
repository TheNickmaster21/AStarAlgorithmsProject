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
            TileMap tileMap = new TileMap(200);
            tileMap.InitMap();
            tileMap.SetStart(new Point(1, 1));

            for (int i = 1; i < 200; i += 2)
            {
                for (int o = 0; o < 200; o++)
                {
                    if ((i + o) % 100 != 0) //Make openings in the lines
                    {
                        tileMap.SetPassable(new Point(i, o), false);
                    }
                }
            }

            Solver solver = new AStarSolver();

            for (int i = 80; i < 200; i += 10)
            {
                tileMap.SetGoal(new Point(i, i));

                System.Console.WriteLine();
                TileMap freshMap = new TileMap(tileMap);
                freshMap.ResetStates();
                System.Console.WriteLine(solver.getPath(freshMap).Count + " tiles long"); 

                long total = 0;
                for (int x = 1; x <= 10; x++)
                {
                    freshMap = new TileMap(tileMap);
                    freshMap.ResetStates();
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    solver.getPath(freshMap);
                    total += stopwatch.ElapsedMilliseconds;
                }

                System.Console.WriteLine(total / 10 + " ms"); 
            }
        }
    }
}
