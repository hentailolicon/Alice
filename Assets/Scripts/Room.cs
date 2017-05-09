using UnityEngine;
using System.Collections;
using System.IO;

public class Room : MonoBehaviour {

    public GameObject[] rock;
    public GameObject[] spike;
    public GameObject[] enemy;
    public GameObject[] poop;
    public int[,] room;
    public static int roomTypeNum = Directory.GetFileSystemEntries("Assets/Room", "*.txt").Length;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void CreateRoom(int roomType)
    {
        if (roomType != -1)
        {
            StreamReader rd = File.OpenText("Assets/Room/Room99"/* + roomType*/ + ".txt");
            string firstLine = rd.ReadLine();
            string[] val = firstLine.Split(',');

            int row = int.Parse(val[0]); //行数
            int col = int.Parse(val[1]);  //每行数据的个数

            room = new int[row, col]; //数组

            for (int i = 0; i < row; i++)  //读入数据并赋予数组
            {
                string line = rd.ReadLine();
                string[] data = line.Split('\t');
                for (int j = 0; j < col; j++)
                {
                    room[i, j] = int.Parse(data[j]);
                }
            }

            float roomSite_x = GameManager.site_y * GameManager.instance.px_x;
            float roomSite_y = GameManager.site_x * GameManager.instance.px_y;
            float pane = GameManager.instance.pane;
            float deviation_x = (GameManager.instance.room_x - pane) / 2;
            float deviation_y = (GameManager.instance.room_y - pane) / 2;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    int kind  = room[i, j] / 10;
                    int subclass = room[i, j] % 10 - 1;
                    switch (kind)
                    {
                        case 1:
                            Instantiate(rock[subclass], new Vector3(roomSite_x - deviation_x + j * pane, roomSite_y - deviation_y + i * pane, 9f), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(spike[subclass], new Vector3(roomSite_x - deviation_x + j * pane, roomSite_y - deviation_y + i * pane, 9f), Quaternion.identity);
                            break;
                        case 3:
                            GameManager.instance.CreateEnemy(enemy[subclass], new Vector3(roomSite_x - deviation_x + j * pane, roomSite_y - deviation_y + i * pane, 8f));
                            break;
                        case 4:
                            Instantiate(poop[subclass], new Vector3(roomSite_x - deviation_x + j * pane, roomSite_y - deviation_y + i * pane, 9f), Quaternion.identity);
                            break;
                        case 9:
                            GameManager.instance.CreateProp(subclass+2, new Vector3(roomSite_x - deviation_x + j * pane, roomSite_y - deviation_y + i * pane, 9f));
                            break;
                    }
                }
            }
        }
    }
}
