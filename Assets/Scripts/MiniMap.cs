using UnityEngine;
using System.Collections;
using Alice;

public class MiniMap : MonoBehaviour 
{
    public GameObject[] img;                 //小地图相关图片
    private GameObject playSite;             //玩家当前位置图片
    private Transform cameraView;            //mini地图视角transform
    private Vector2 cameraMove;              //摄像机将要移动到的坐标
    private Transform bgView;                //mini地图背景transform
    private bool isMove = false;             //摄像机是否在移动
	// Use this for initialization

    //小地图初始化，mapBoard的值有1,2,3三种，1为初始值，代表此处存在房间；2代表此房间未被探索；3代表此房间已被探索。
	public void Init ()
    {
        cameraView = GameObject.FindGameObjectWithTag("MiniMap").transform;     //获得mini地图摄像机transform
        cameraMove = new Vector2(GameManager.site_y * GameManager.instance.mini_x, GameManager.site_x * GameManager.instance.mini_y);
        cameraView.position = cameraMove;
        bgView = GameObject.FindGameObjectWithTag("MiniBG").transform;
        bgView.position = new Vector3(cameraView.position.x, cameraView.position.y, 10f);
        playSite = Instantiate(img[2], new Vector3(0, 0, 8f), Quaternion.identity) as GameObject;  //实例化玩家当前所在房间的图片
        GameManager.mapBoard[GameManager.site_x, GameManager.site_y] = 2;              //设置玩家所在房间为2，即此房间已被探索过
        UpdateImg();
	}

    void Update()
    {
        if(isMove)
        {
            CameraFollow.TrackPlayer(cameraView, cameraMove);
            bgView.position = new Vector3(cameraView.position.x, cameraView.position.y, 10f);
            if ((Mathf.Abs(cameraMove.x - cameraView.position.x) < 0.01f) && (Mathf.Abs(cameraMove.y - cameraView.position.y) < 0.01f))
            {
                isMove = false;
            }
        }
    }

    //更新小地图，包括逻辑，图片和镜头位置
    public void UpdateMap(char direction)
    {
        if(direction == 'u')
        {
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y + GameManager.instance.mini_y;
        }
        else if (direction == 'd')
        {
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y - GameManager.instance.mini_y;
        }
        else if (direction == 'l')
        {
            cameraMove.x = cameraView.position.x - GameManager.instance.mini_x;
            cameraMove.y = cameraView.position.y;
        }
        else if (direction == 'r')
        {
            cameraMove.x = cameraView.position.x + GameManager.instance.mini_x;
            cameraMove.y = cameraView.position.y;
        }
        isMove = true;
        UpdateImg();
    }


    //更新小地图图片，包括当前房间和四周房间
    private void UpdateImg()
    {
        int i, j;
        i = GameManager.site_x;
        j = GameManager.site_y;
        //检测左边是否有房间，若有就实例化存在房间的图片，并将mapBoard的值设为2，防止重复实例化图片
        if ((i > 0) && (GameManager.mapBoard[i - 1, j] == 1))
        {
            Instantiate(img[0], new Vector3(j * GameManager.instance.mini_x, (i-1) * GameManager.instance.mini_y, 9.5f), Quaternion.identity);
            GameManager.mapBoard[i - 1, j] = 2;
        }
        //检测右边是否有房间
        if ((i < MapAlgo.GetX() - 1) && (GameManager.mapBoard[i + 1, j] == 1))
        {
            Instantiate(img[0], new Vector3(j * GameManager.instance.mini_x, (i+1) * GameManager.instance.mini_y, 9.5f), Quaternion.identity);
            GameManager.mapBoard[i + 1, j] = 2;
        }
        //检测上边是否有房间
        if ((j > 0) && (GameManager.mapBoard[i, j - 1] == 1))
        {
            Instantiate(img[0], new Vector3((j-1) * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 9.5f), Quaternion.identity);
            GameManager.mapBoard[i, j - 1] = 2;
        }
        //检测下边是否有房间
        if ((j < MapAlgo.GetY() - 1) && (GameManager.mapBoard[i, j + 1] == 1))
        {
            Instantiate(img[0], new Vector3((j+1) * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 9.5f), Quaternion.identity);
            GameManager.mapBoard[i, j + 1] = 2;
        }
        //若当前房间未被探索，则实例化探索过房间的图片，并将mapBoard的值设为3，防止重复实例化图片
        if (GameManager.mapBoard[i, j] == 2)
        {
            Instantiate(img[1], new Vector3(j * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 9f), Quaternion.identity);
            GameManager.mapBoard[i, j] = 3;
        }
        playSite.transform.position = new Vector3(j * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 8f);  //将玩家当前位置图片移动到正确位置
    }
}
