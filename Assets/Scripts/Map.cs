using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using Alice;
public class Map : MonoBehaviour
{
    public struct coordinate
    {
        public int x;
        public int y;
        public coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    public GameObject background;             //背景图片
    public GameObject[] door;                //门的图片
    public GameObject[] bossdoor;

    public int[,] mapBoard { get; private set; }
    public int[,] roomTypeBoard { get; private set; }
    private List<coordinate> specialRooms = new List<coordinate>();
    private List<int> roomTypeNum = new List<int>();
    private Random rand = new Random();
    private MapAlgo mapalgo = new MapAlgo();


    private void Init()
    {
        mapBoard = mapalgo.create();
        roomTypeBoard = new int[MapAlgo.GetX(), MapAlgo.GetY()];
        for(int i = 0; i < MapAlgo.GetX(); i++)
        {
            for(int j = 0; j < MapAlgo.GetY(); j++)
            {
                roomTypeBoard[i, j] = -1;
            }
        }
    }

    //生成地图
    public void CreateMap()
    {
        Init();
        CreateBackground();
    }
    //生成地图背景
    private void CreateBackground()
    {
        mapBoard[0, 0] = 0;
        for (int i = 0; i < MapAlgo.GetX(); i++)
        {
            for (int j = 0; j < MapAlgo.GetY(); j++)
            {
                if (mapBoard[i, j] == 1)
                {
                    Instantiate(background, new Vector3(j * GameManager.instance.px_x, i * GameManager.instance.px_y, 10f), Quaternion.identity);
                    CreateDoor(i, j);
                    //roomTypeBoard[i, j] = rand.Next(Room.roomTypeNum);
                    CreateNormalRoom(i, j);
                }
            }
        }
        CreateBossRoom();
        CreateShopRoom();
    }

    //生成门
    private void CreateDoor(int i, int j)
    {
        int count = 0;
        if ((i > 0) && (mapBoard[i - 1, j] == 1))
        {
            Instantiate(door[1], new Vector3(j * GameManager.instance.px_x, i * GameManager.instance.px_y - 2.9f, 9f), Quaternion.identity);
            count++;
        }
        if ((i < MapAlgo.GetX() - 1) && (mapBoard[i + 1, j] == 1))
        {
            Instantiate(door[0], new Vector3(j * GameManager.instance.px_x, i * GameManager.instance.px_y + 2.9f, 9f), Quaternion.identity);
            count++;
        }
        if ((j > 0) && (mapBoard[i, j - 1] == 1))
        {
            Instantiate(door[2], new Vector3(j * GameManager.instance.px_x - 5.0f, i * GameManager.instance.px_y, 9f), Quaternion.identity);
            count++;
        }
        if ((j < MapAlgo.GetY() - 1) && (mapBoard[i, j + 1] == 1))
        {
            Instantiate(door[3], new Vector3(j * GameManager.instance.px_x + 5.0f, i * GameManager.instance.px_y, 9f), Quaternion.identity);
            count++;
        }

        if(count == 1)
        {
            specialRooms.Add(new coordinate(i, j));
        }
    }

    //生成普通房间
    private void CreateNormalRoom(int i, int j)
    {
        if(roomTypeNum.Count == 0)
        {
            for(int k=0; k<Room.roomTypeNum; k++)
            {
                roomTypeNum.Add(k);
            }
        }
        int index = rand.Next(roomTypeNum.Count);
        roomTypeBoard[i, j] = roomTypeNum[index];
        roomTypeNum.RemoveAt(index);
    }

    //生成boss房间
    private void CreateBossRoom()
    {
        int index = rand.Next(specialRooms.Count);
        int i = specialRooms[index].x;
        int j = specialRooms[index].y;
        roomTypeBoard[i, j] = 99;
        Instantiate(bossdoor[0], new Vector3(j * GameManager.instance.px_x, (i-1) * GameManager.instance.px_y + 2.9f, 8.9f), Quaternion.identity);
        Instantiate(bossdoor[1], new Vector3(j * GameManager.instance.px_x, (i+1) * GameManager.instance.px_y - 2.9f, 8.9f), Quaternion.identity);
        Instantiate(bossdoor[2], new Vector3((j+1) * GameManager.instance.px_x - 5.0f, i * GameManager.instance.px_y, 8.9f), Quaternion.identity);
        Instantiate(bossdoor[3], new Vector3((j-1) * GameManager.instance.px_x + 5.0f, i * GameManager.instance.px_y, 8.9f), Quaternion.identity);
        specialRooms.RemoveAt(index);
    }

    //生成商店房间
    private void CreateShopRoom()
    {
        int index = rand.Next(specialRooms.Count);
        roomTypeBoard[specialRooms[index].x, specialRooms[index].y] = 100;
    }
}
