using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Alice;
public class GameManager : MonoBehaviour 
{
    public enum PlayerAttribute
    {
        DAMAGE = 1,
        HP = 2,
        HPMax = 3,
        LUCK = 4,
        SPEED = 5,
        BOMB = 6,
        COIN = 7
    }

    public static GameManager instance = null;				//单例模式
    public float px_x = 12.8f;                              //背景图片的水平像素
    public float px_y = 7.2f;                               //背景图片的垂直像素
    public float mini_x = 0.21f;                            //小地图图片的水平像素
    public float mini_y = 0.19f;                            //小地图图片的垂直像素
    public float room_x = 9.1f;                             //房间图片的水平像素
    public float room_y = 4.9f;                             //房间图片的垂直像素
    public float pane = 0.7f;                               //房间里每个矩形格子的边长
    public GameObject[] propObj;                            //道具object
    public GameObject[] poof;                               //特效
    public static int[,] roomTypeBoard;                     //房间类型矩阵
    public static int[,] mapBoard;                          //地图逻辑矩阵
    public static int site_x;                               //玩家逻辑行坐标
    public static int site_y;                               //玩家逻辑列坐标
    public static int enemyCount = 0;                       //房间敌人计数
    public static MiniMap miniMap;

    public List<Prop> props;                                //道具效果列表
    public PlayerAttributeValue PAV;                        //记录所有效果结算后的玩家各项数值

    private System.Random rand = new System.Random();
    private Map map;                                        //地图
    private Room room;                                      //房间
    private bool enemyExistFlag = false;                    //敌人存在标志
    private List<GameObject> doors = new List<GameObject>();
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
        room = GetComponent<Room>();
        Init();
    }

    private void Init() 
    {
        map.CreateMap();
        mapBoard = map.mapBoard;
        roomTypeBoard = map.roomTypeBoard;
        site_x = MapAlgo.GetStartX();
        site_y = MapAlgo.GetStartY();
        miniMap.Init();

        GameObject[] tmp = GameObject.FindGameObjectsWithTag("uDoor");
        doors.AddRange(tmp);
        tmp = GameObject.FindGameObjectsWithTag("dDoor");
        doors.AddRange(tmp);
        tmp = GameObject.FindGameObjectsWithTag("lDoor");
        doors.AddRange(tmp);
        tmp = GameObject.FindGameObjectsWithTag("rDoor");
        doors.AddRange(tmp);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PAV = new PlayerAttributeValue();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(enemyExistFlag)
        {
            if(enemyCount == 0)
            {
                ShowDoorAnima(false);
                enemyExistFlag = false;
                CreateProp(0, new Vector3(site_y * px_x, site_x * px_y, 8.5f));
                Instantiate(poof[0], new Vector3(site_y * px_x, site_x * px_y, 9f), Quaternion.identity);
            }
        }

	}

    //获得某个方向的速率，力的方向为start指向end，最大速率为maxForce
    public Vector3 GetVelocity(Vector3 start, Vector3 end, float maxVelocity)
    {
        Vector3 force = (end - start) * 10f;

        //防止被0除
        float safeVal = 0.01f;
        if(force.x == 0 && force.y == 0)
        {
            return new Vector3(0, 0, 0);
        }
        if (Mathf.Abs(force.y) < safeVal)
        {
            if(force.y < 0)
            {
                force.y = -safeVal;
            }
            else
            {
                force.y = safeVal;
            }
        }
        if (Mathf.Abs(force.x) < safeVal)
        {
            if (force.x < 0)
            {
                force.x = -safeVal;
            }
            else
            {
                force.x = safeVal;
            }
        }
        //if(Mathf.Abs(ratio)>1f)
        //{
        //    force.y = force.y / Mathf.Abs(force.y) * maxVelocity;
        //    force.x = force.y / ratio;
        //}
        //else
        //{
        //    force.x = force.x / Mathf.Abs(force.x) * maxVelocity;
        //    force.y = force.x * ratio;
        //}
        float ratio = force.y / force.x;
        force.y = Mathf.Sin(Mathf.Atan(ratio)) * maxVelocity;
        if (force.x < 0)
        {
            force.y = -force.y;
        }
        force.x = force.y / ratio;

        return force;
    }

    //得到玩家各项数值
    public float GetPlayerAttributeValue(PlayerAttribute attribute)
    {
        float value = 0f;
        switch (attribute)
        {
            case PlayerAttribute.DAMAGE:
                value =  player.damage;
                break;
            case PlayerAttribute.HP:
                value = player.HP;
                break;
            case PlayerAttribute.HPMax:
                value = player.HPMax;
                break;
            case PlayerAttribute.LUCK:
                value = player.luck;
                break;
            case PlayerAttribute.SPEED:
                value = player.speed;
                break;
            case PlayerAttribute.BOMB:
                value = player.bomb;
                break;
            case PlayerAttribute.COIN:
                value = player.coin;
                break;
        }
        return value;
    }

    //播放开关门动画
    public void ShowDoorAnima(bool isDoorClose)
    {
        foreach (GameObject obj in doors)
        {
            obj.GetComponent<Animator>().SetBool("isDoorClose", isDoorClose);
            obj.GetComponent<Collider2D>().isTrigger = !isDoorClose;
        }
    }

    public void CreateRoom()
    {
        room.CreateRoom(roomTypeBoard[site_x,site_y]);
        if (enemyCount > 0)
        {
            ShowDoorAnima(true);
            enemyExistFlag = true;
        }
    }

    public void CreateEnemy(GameObject obj, Vector3 pos)
    {
        Instantiate(obj, pos, Quaternion.identity);
        Instantiate(poof[0], pos, Quaternion.identity);
        GameManager.enemyCount++;
    }


    public void CreateProp(int val, Vector3 pos)
    {
        switch(val)
        {
            case 0:
                Instantiate(propObj[rand.Next(propObj.Length)], pos, Quaternion.identity);
                break;
            case 1:
                Instantiate(propObj[rand.Next(2, propObj.Length)], pos, Quaternion.identity);
                break;
            case 2:
                Instantiate(propObj[0], pos, Quaternion.identity);
                break;
            case 3:
                Instantiate(propObj[1], pos, Quaternion.identity);
                break;
            case 4:
                Instantiate(propObj[3], pos, Quaternion.identity);
                break;
        }
    }

    public Prop GetPlayerProp()
    {
        return player.prop;
    }

    public void SetPlayerProp(Prop p)
    {
        player.prop = p;
    }

    public void SetPlayerImmuneTime(float val)
    {
        player.Attacked(new Vector3(0, 0, 0), 0);
        player.immuneTime = val;
    }

    //更新玩家各项属性数值
    public bool UpdatePlayerAttributeValue(PlayerAttribute attribute, int value)
    {
        switch (attribute)
        {
            case PlayerAttribute.HP:
                if (player.HP == player.HPMax)
                {
                    return false;
                }
                player.HP += value;
                if(player.HP>player.HPMax)
                {
                    player.HP = player.HPMax;
                }
                if(player.HP<=0)
                {

                }
                break;
            case PlayerAttribute.HPMax:
                player.HPMax += value;
                if (player.HP > player.HPMax)
                {
                    player.HP = player.HPMax;
                }
                if(player.HPMax<=0)
                {

                }
                break;
            case PlayerAttribute.LUCK:
                player.luck += value;
                break;
            case PlayerAttribute.SPEED:
                player.speed += value;
                break;
            case PlayerAttribute.BOMB:
                player.bomb += value;
                break;
            case PlayerAttribute.COIN:
                player.coin += value;
                break;
        }
        return true;
    }

    //道具效果清算
    public void PropEffectClearing()
    {
        PAV.Reset();
        for(int i=0;i<props.Count;i++)
        {
            props[i].OtherEffect();
        }
    }
}
