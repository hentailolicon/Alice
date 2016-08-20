using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alice
{
    class MapAlgo
    {
        public const int x = 5;                         //地图的长度
        public const int y = 7;                         //地图的宽度
        private static int startX;                      //起始横坐标
        private static int startY;                      //起始纵坐标
        public int roomNum = 15;                        //期望房间数(有可能地图太小生成不了期望的房间数)
        private int initRoom = 0;                       //已生成的房间数
        private int unlinkableCount = 0;                //不能再连接其他房间的房间计数
        private int[,] map = new int[x, y];             //地图矩阵，0代表没有房间，1代表房间已生成，9代表此处与多个房间相邻，不能生成房间
        private int[,] linkable = new int[x, y];        //房间的可连接性矩阵，用于记录房间的四周是否还能生成其他房间，0代表房间不能再生成其他房间，1代表可以
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

        //初始化地图矩阵和连接矩阵，并初始化起始房间
        private void Init()
        {
            initRoom = 0;
            unlinkableCount = 0;
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

        // 检查可连接性，若此处周围只有一个房间，说明此处可被连通，返回true，
        // 否则说明此处不能被连通（连通将构成回路），返回false，并将此处标记为9
        private bool CheckLink(int i, int j)
        {
            if (map[i, j] > 0)
            {
                return false;
            }
            //计算周围的房间数
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
            //去除周围标记为9的影响
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

        //检查房间周围是否还能生成房间
        private bool CheckChoose(int i, int j)
        {
            //记录房间周围已生成的和不能生成的房间数
            int tmp = 4;
            if (i + 1 < x)
            {
                tmp--;
                if (map[i + 1, j] > 0)
                {
                    tmp++;
                }
            }
            if (i > 0)
            {
                tmp--;
                if (map[i - 1, j] > 0)
                {
                    tmp++;
                }
            }
            if (j + 1 < y)
            {
                tmp--;
                if (map[i, j + 1] > 0)
                {
                    tmp++;
                }
            }
            if (j > 0)
            {
                tmp--;
                if (map[i, j - 1] > 0)
                {
                    tmp++;
                }
            }
            //若为4，说明此房间周围已不能再生成房间
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

        //选择将要生成连接的房间
        private void ChooseLink(int i, int j)
        {
            if (!CheckChoose(i, j))
            {
                return;
            }
            //随机选择一个连接方向
            int val = rand.Next(4);
            int tmp = val;
            bool success = false;
            bool flag = false;
            do
            {
                //检测四周的房间能否连通，不能则tmp++进入下一个if
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
                //当val>=2时，且2,3所对应的房间又不能被连接时，前面可能存在能连接的房间就被跳过了，
                //于是再循环一次检测是否还存在可以连接的房间，既保证了随机性，又保证了四周的房间都能被检测一次
                if (!success && (val >= 2))
                {
                    flag = true;
                    tmp = 0;
                    val -= 4;
                }
                else
                {
                    flag = false;
                }
            } while (flag);
            if (success)
            {
                initRoom++;
            }
        }

        //生成地图
        public int[,] create()
        {
            Init();
            while (true)
            {
                //在当前可连接的房间随机挑选一个生成连接,count代表第几个房间
                int count = rand.Next(initRoom - unlinkableCount);
                bool flag = false;
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        //检测此处是否为可连接房间
                        if ((map[i, j] == 1) && (linkable[i, j] == 1))
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
                //如果生成的房间已经达到期望数或者已经没有可连接的房间，跳出循环
                if ((roomNum == initRoom) || (unlinkableCount == initRoom))
                {
                    break;
                }
            }
            return map;
        }
    }
}
