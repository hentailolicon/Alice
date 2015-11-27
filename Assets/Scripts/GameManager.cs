using UnityEngine;
using System.Collections;
using Alice;
public class GameManager : MonoBehaviour 
{
    public static GameManager instance = null;				//单例模式
    public float px_x = 12.8f;                              //背景图片的水平像素
    public float px_y = 7.2f;                               //背景图片的垂直像素
    public float mini_x = 0.21f;
    public float mini_y = 0.19f;
    public static int[,] mapBoard;
    public static int site_x = MapAlgo.GetX() - 1;
    public static int site_y;
    public static MiniMap miniMap;

    private Map map;                                        //地图
    
    // Use this for initialization
	void Awake()
    {
        //设置单例
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        map = GetComponent<Map>();
        miniMap = GetComponent<MiniMap>();
        Init();
    }
    private void Init() 
    {
        map.CreateMap();
        mapBoard = map.mapBoard;
        miniMap.Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //获得某个方向的力，力的方向为start指向end，水平方向或垂直方向上的最大力为maxForce
    public Vector3 GetDirectionForce(Vector3 start, Vector3 end, float maxForce)
    {
        Vector3 force = (end - start) * 10f;
        float ratio = force.y / force.x;
        if(Mathf.Abs(ratio)>1f)
        {
            force.y = force.y / Mathf.Abs(force.y) * maxForce;
            force.x = force.y / ratio;
        }
        else
        {
            force.x = force.x / Mathf.Abs(force.x) * maxForce;
            force.y = force.x * ratio;
        }
        return force;
    }
}
