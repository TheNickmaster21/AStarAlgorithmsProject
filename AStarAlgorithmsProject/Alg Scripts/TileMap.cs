using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarAlgorithmsProject
{
    class TileMap
    {
        private Tile[,] map;

        public TileMap(int size)
        {
            map = new Tile[size, size];
        }

        public int getSize()
        {
            return map.GetLength(0);
        }

        public Tile get(Point l)
        {
            return map[l.X, l.Y];
        }

        // "Prime" the map so all tiles are in their default state and their locations set to the proper cordinates in the map
        public void initMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    map[i, j] = new Tile(new Point(i, j));
                }
            }
        }

    }
}
