using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance = null;				//单例模式
    public float px_x = 12.8f;                              //背景图片的水平像素
    public float px_y = 7.2f;                               //背景图片的垂直像素

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
        Init();
    }
    private void Init() 
    {
        map.CreateMap();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
