using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Alice;
public class GameManager : MonoBehaviour 
{
    public enum PlayerState
    {
        HP = 1,
        HPMax = 2,
        LUCK = 3,
        SPEED = 4
    }

    public class PlayerStateValue
    {
        public float damage = 10f;
        public float speed;
        public float HP;
        public float HPMax;
        public float luck;

        public void reset()
        {
            damage = 10f;
        }
    }

    public static GameManager instance = null;				//单例模式
    public float px_x = 12.8f;                              //背景图片的水平像素
    public float px_y = 7.2f;                               //背景图片的垂直像素
    public float mini_x = 0.21f;                            //小地图图片的水平像素
    public float mini_y = 0.19f;                            //小地图图片的垂直像素
    public static int[,] mapBoard;                          //地图逻辑矩阵
    public static int site_x;                               //玩家逻辑横坐标
    public static int site_y;                               //玩家逻辑纵坐标
    public static MiniMap miniMap;

    public List<Prop> props;                                //道具效果列表
    public PlayerStateValue PSV = new PlayerStateValue();   //记录所有效果结算后的玩家各项数值

    private Map map;                                        //地图
    private Player player;

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
        site_x = MapAlgo.GetStartX();
        site_y = MapAlgo.GetStartY();
        miniMap.Init();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

    public void UpdatePlayerState(PlayerState attribute, int value)
    {
        switch (attribute)
        {
            case PlayerState.HP:
                player.HP += value;
                if(player.HP>player.HPMax)
                {
                    player.HP = player.HPMax;
                }
                if(player.HP<=0)
                {

                }
                break;
            case PlayerState.HPMax:
                player.HPMax += value;
                if(player.HPMax<=0)
                {

                }
                break;
            case PlayerState.LUCK:
                player.luck += value;
                break;
            case PlayerState.SPEED:
                player.speed += value;
                break;
        }
    }

    public void PropEffectClearing()
    {
        PSV.reset();
        for(int i=0;i<props.Count;i++)
        {
            props[i].OtherEffect(PSV);
        }
    }
}
