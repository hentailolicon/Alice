using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alice
{
    class MapAlgo
    {
        public const int x = 5;
        public const int y = 7;
        private static int startX;
        private static int startY;
        public int roomNum = 15;
        private int initRoom = 0;
        private int unlinkableCount = 0;
        private int[,] map = new int[x, y];
        private int[,] linkable = new int[x, y];
        private Random rand = new Random();
        public static int GetX()
        {
            return x;
        }
        public static int GetY()
        {
            return y;
        }

        public static int GetStartX()
        {
            return startX;
        }
        public static int GetStartY()
        {
            return startY;
        }

        private void Init()
        {
            startX = rand.Next(1, x - 1);
            startY = rand.Next(1, y - 1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    map[i, j] = 0;
                    linkable[i, j] = 1;
                }
            }
            map[startX, startY] = 1;
            initRoom++;
        }

        private bool CheckLink(int i, int j)
        {
            if (map[i, j] > 0)
            {
                return false;
            }
            int tmp = 0;
            if (i + 1 < x)
            {
                tmp += map[i + 1, j];
            }
            if (i > 0)
            {
                tmp += map[i - 1, j];
            }
            if (j + 1 < y)
            {
                tmp += map[i, j + 1];
            }
            if (j > 0)
            {
                tmp += map[i, j - 1];
            }
            while (tmp > 4)
            {
                tmp -= 9;
            }
            if (tmp == 1)
            {
                map[i, j] = 1;
                return true;
            }
            else
            {
                map[i, j] = 9;
                return false;
            }

        }

        private bool CheckChoose(int i, int j)
        {
            int tmp = 0;
            if ((i + 1 < x) && map[i + 1, j] > 0)
            {
                tmp++;
            }
            if ((i > 1) && map[i - 1, j] > 0)
            {
                tmp++;
            }
            if ((j + 1 < y) && map[i, j + 1] > 0)
            {
                tmp++;
            }
            if ((j > 1) && map[i, j - 1] > 0)
            {
                tmp++;
            }
            if (tmp == 4)
            {
                linkable[i, j] = 0;
                unlinkableCount++;
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ChooseLink(int i, int j)
        {
            if (!CheckChoose(i, j))
            {
                return;
            }
            int tmp = rand.Next(4);
            bool success = false;
            if (tmp == 0 && (i + 1 < x))
            {
                success = CheckLink(i + 1, j);
                if (!success)
                {
                    tmp++;
                }
            }
            if (tmp == 1 && (i > 0))
            {
                success = CheckLink(i - 1, j);
                if (!success)
                {
                    tmp++;
                }
            }
            if (tmp == 2 && (j + 1 < y))
            {
                success = CheckLink(i, j + 1);
                if (!success)
                {
                    tmp++;
                }
            }
            if (tmp == 3 && (j > 0))
            {
                success = CheckLink(i, j - 1);
                if (!success && (i + 1 < x))
                {
                    success = CheckLink(i + 1, j);
                }
            }
            if (success)
            {
                initRoom++;
            }
        }
        public int[,] create()
        {
            Init();
            int startTime = System.Environment.TickCount;
            while (true)
            {
                int count = rand.Next(initRoom - unlinkableCount);
                bool flag = false;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (map[i, j] == 1 && linkable[i, j] == 1)
                        {
                            if (count == 0)
                            {
                                ChooseLink(i, j);
                                flag = true;
                                break;
                            }
                            else
                            {
                                count--;
                            }
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                if (roomNum == initRoom)
                {
                    break;
                }
                int endTime = System.Environment.TickCount;
                if (endTime - startTime > 100)
                {
                    break;
                }
            }
            return map;
        }
    }
}
