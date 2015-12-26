using UnityEngine;
using System.Collections;
using Alice;

public class MiniMap : MonoBehaviour 
{
    public GameObject[] img;
    private GameObject playSite;
    private Transform cameraView;                                                                               // mini地图视角transform
    private Vector2 cameraMove;              //摄像机将要移动到的坐标
	// Use this for initialization
	public void Init ()
    {
        cameraView = GameObject.FindGameObjectWithTag("MiniMap").transform;     //获得mini地图摄像机transform
        cameraMove = new Vector2(0, GameManager.site_x * GameManager.instance.mini_y);
        cameraView.position = cameraMove;
        playSite = Instantiate(img[2], new Vector3(0, 0, 8f), Quaternion.identity) as GameObject;
        GameManager.mapBoard[GameManager.site_x, GameManager.site_y] = 2;
        GameManager.mapBoard[0, 0] = 3;
        UpdateImg();
	}

    public void UpdateMap(char direction)
    {
        if(direction == 'u')
        {
            GameManager.site_x++;
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y + GameManager.instance.mini_y;
        }
        else if (direction == 'd')
        {
            GameManager.site_x--;
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y - GameManager.instance.mini_y;
        }
        else if (direction == 'l')
        {
            GameManager.site_y--;
            cameraMove.x = cameraView.position.x - GameManager.instance.mini_x;
            cameraMove.y = cameraView.position.y;
        }
        else if (direction == 'r')
        {
            GameManager.site_y++;
            cameraMove.x = cameraView.position.x + GameManager.instance.mini_x;
            cameraMove.y = cameraView.position.y;
        }
        CameraFollow.TrackPlayer(cameraView, cameraMove);
        UpdateImg();
    }

    private void UpdateImg()
    {
        int i, j;
        i = GameManager.site_x;
        j = GameManager.site_y;
        if ((i > 0) && (GameManager.mapBoard[i - 1, j] == 1))
        {
            Instantiate(img[0], new Vector3(j * GameManager.instance.mini_x, (i-1) * GameManager.instance.mini_y, 10f), Quaternion.identity);
            GameManager.mapBoard[i - 1, j] = 2;
        }
        if ((i < MapAlgo.GetX() - 1) && (GameManager.mapBoard[i + 1, j] == 1))
        {
            Instantiate(img[0], new Vector3(j * GameManager.instance.mini_x, (i+1) * GameManager.instance.mini_y, 10f), Quaternion.identity);
            GameManager.mapBoard[i + 1, j] = 2;
        }
        if ((j > 0) && (GameManager.mapBoard[i, j - 1] == 1))
        {
            Instantiate(img[0], new Vector3((j-1) * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 10f), Quaternion.identity);
            GameManager.mapBoard[i, j - 1] = 2;
        }
        if ((j < MapAlgo.GetY() - 1) && (GameManager.mapBoard[i, j + 1] == 1))
        {
            Instantiate(img[0], new Vector3((j+1) * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 10f), Quaternion.identity);
            GameManager.mapBoard[i, j + 1] = 2;
        }
        if (GameManager.mapBoard[i, j] == 2)
        {
            Instantiate(img[1], new Vector3(j * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 9f), Quaternion.identity);
            GameManager.mapBoard[i, j] = 3;
        }
        playSite.transform.position = new Vector3(j * GameManager.instance.mini_x, i * GameManager.instance.mini_y, 8f);
    }
}
