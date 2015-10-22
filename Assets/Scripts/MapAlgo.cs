using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alice
{
    class MapAlgo
    {
        public const int MAZE_MAX = 50;
        public const int x = 3;
        public const int y = 3;
        public static int[,] tmpmap = new int[MAZE_MAX + 2, MAZE_MAX + 2];
        public static int[,] map = new int[x * 2 - 1, y * 2 + 1];
        public static int[,] d = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        public Random Rand = new Random();
        public int GetX()
        {
            return 2 * x - 1;
        }
        public int GetY()
        {
            return 2 * y + 1;
        }
        private int search(int x, int y)
        {
            int zx = x * 2, zy = y * 2, next, turn, i;
            tmpmap[zx, zy] = 0;
            int random = Rand.Next(2);
            if (random == 1)
                turn = 1;
            else
                turn = 3;
            for (i = 0, next = Rand.Next(4); i < 4; i++, next = (next + turn) % 4)
                if (tmpmap[zx + 2 * d[next, 0], zy + 2 * d[next, 1]] == 1)   //这里是zx + 2*d[next][0]...
                {
                    tmpmap[zx + d[next, 0], zy + d[next, 1]] = 0;      // 这里是zx + d[next][0]...
                    search(x + d[next, 0], y + d[next, 1]);         // 这里是x + d[next][0]
                }
            return 0;
        }
        private void Make_Maze(int x, int y)
        {
            int z1, z2;
            for (z1 = 0, z2 = 2 * y + 2; z1 <= 2 * x + 2; z1++)  // 数组最外围置0，表示无墙
            {
                tmpmap[z1, 0] = 0;
                tmpmap[z1, z2] = 0;
            }
            for (z1 = 0, z2 = 2 * x + 2; z1 <= 2 * y + 2; z1++)
            {
                tmpmap[0, z1] = 0;
                tmpmap[z2, z1] = 0;
            }
            tmpmap[1, 2] = 0; tmpmap[2 * x + 1, 2 * y] = 0;      //入口和出口
            search(Rand.Next(x) + 1, Rand.Next(y) + 1);
        }
        public int[,] create()
        {
            for (int i = 0; i <= x * 2 + 2; ++i)
                for (int j = 0; j <= y * 2 + 2; ++j)
                    tmpmap[i, j] = 1;
            Make_Maze(x, y);
            for (int z2 = 0; z2 < y * 2 - 1; z2++)
            {
                for (int z1 = 0; z1 < x * 2 + 1; z1++)
                {
                    map[z2, z1] = tmpmap[y * 2 + 1 - z1, z2 + 2];
                }
            }
            return map;
        }
    }
}
