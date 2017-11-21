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
            tileMap.Start = tileMap.Get(new Point(0, 0));

            for (int i = 1; i < 200; i += 2)
            {
                for (int o = 0; o < 200; o += 2)
                {
                    if (((i + o) % 100) / 10 != 0) //Make openings in the lines
                    {
                       // tileMap.SetPassable(new Point(i, o), false);
                    }
                }
            }

            Solver solver = new AStarSolver();

            for (int i = 2; i < 200; i += 40)
            {
                tileMap.Goal = tileMap.Get(new Point(i, 0));

                System.Console.WriteLine();
                System.Console.WriteLine(i); //Where goal is
                System.Console.WriteLine(solver.getPath(new TileMap(tileMap)).Count); //Length of path

                long total = 0;
                for (int x = 1; x <= 10; x++)
                {
                    TileMap freshMap = new TileMap(tileMap);
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    solver.getPath(freshMap);
                    total += stopwatch.ElapsedTicks;
                }

                System.Console.WriteLine(total / 10); //Average time to get the path
            }
        }
    }
}
