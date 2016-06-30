using UnityEngine;
using System.Collections;
using Alice;

public class Player : MonoBehaviour
{
    public float damage = 10f; 
    public float speed = 3f;                                             //玩家移动速度
    public int HP = 30;
    public int HPMax = 30;
    public int luck = 2;
    public GameObject weapon;

    //玩家状态 普通状态 移动视角状态 受到攻击状态
    private const string NORMAL = "normal";
    private const string MOVECAMERA = "movecamera";
    private const string ATTACKED = "attacked";

    private Vector2 movement;                                        //玩家位移 = 玩家速度 * 方向
    private Animator anim;                                           //玩家动画控制器
    private Transform cameraView;                                    //主视角transform
    private Vector2 cameraMove = new Vector2(0, 0);                  //摄像机将要移动到的坐标
    private float attackCooldown = 0;                                //攻击冷却时间
    private float attackSpeed;                                       //攻速
    private string state = NORMAL;                                   //玩家状态
    private Vector3 repelForce;                                      //玩家受到攻击时的受到的击退力
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        attackSpeed = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<Weapon>().attackSpeed;
        cameraView = GameObject.FindGameObjectWithTag("MainCamera").transform;                                                           //获得主摄像机transform
        Vector2 start= new Vector2(MapAlgo.GetStartY() * GameManager.instance.px_x, MapAlgo.GetStartX() * GameManager.instance.px_y);    //获得初始坐标
        transform.position = new Vector3(start.x,start.y,3);                                                                             //设置玩家初始坐标
        cameraView.position = new Vector3(start.x, start.y,-10);                                                                         //设置主摄像机初始坐标
    }

    // Update is called once per frame
    void Update()
    {
        //更新攻击冷却时间
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        float inputX = 0f;
        float inputY = 0f;
        if (!(Input.GetKey("a") && Input.GetKey("d")))
        {
            //获得平滑移动量
            inputX = Input.GetAxis("Horizontal");   
        }
        if(!(Input.GetKey("w") && Input.GetKey("s")))
        {
            inputY = Input.GetAxis("Vertical");
        }
        switch (state)
        {
            case NORMAL:
                //设置动画切换参数，前两个参数控制切换到哪个动画，后两个参数实现动画立即切换
                anim.SetFloat("horSpeed", inputX);
                anim.SetFloat("verSpeed", inputY);
                anim.SetFloat("horRaw", Input.GetAxisRaw("Horizontal"));
                anim.SetFloat("verRaw", Input.GetAxisRaw("Vertical"));
                //移动玩家
                movement = new Vector2(speed * inputX, speed * inputY);
                GetComponent<Rigidbody2D>().velocity = movement;  
                //攻击  
                if (Input.GetKey("j"))
                {
                    if (attackCooldown <= 0)
                    {
                        attackCooldown = 1f / attackSpeed;
                        Attack();
                    }
                }
                break;
            case MOVECAMERA:
                //停止玩家移动
                GetComponent<Rigidbody2D>().Sleep();                                
                //设置动画
                anim.SetFloat("horSpeed", 0);
                anim.SetFloat("verSpeed", 0);
                anim.SetFloat("horRaw", 0);
                anim.SetFloat("verRaw", 0);
                //移动摄像机
                CameraFollow.TrackPlayer(cameraView, cameraMove);
                //判断摄像机是否到达预定地点
                if ((Mathf.Abs(cameraMove.x - cameraView.position.x) < 0.01f) && (Mathf.Abs(cameraMove.y - cameraView.position.y) < 0.01f))
                {
                    SetState(NORMAL);
                }
                break;
            case ATTACKED:
                if (attackCooldown > 0)
                {
                    GetComponent<Rigidbody2D>().velocity = repelForce;
                }
                else
                    SetState(NORMAL);
                break;
        }
    }
	void OnTriggerEnter2D (Collider2D other)
	{
        if (other.tag.IndexOf("Door")>=0)
        {
            SceneMove(other);
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject .tag == "Enemy")
        {
            SetState(ATTACKED);
            repelForce = GameManager.instance.GetDirectionForce(other.transform.position, transform.position, 2.5f);
            attackCooldown = 0.3f;
        }
    }

    //场景移动，包括玩家，主摄像机，小地图摄像机的移动
    private void SceneMove(Collider2D other)
    {
        //判断玩家进入了哪个门
        if (other.tag == "uDoor")
        {
            //玩家移动
            transform.Translate(0, 2.6f, 0);
            //主摄像机移动
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y + GameManager.instance.px_y;
            //小地图摄像机
            GameManager.miniMap.UpdateMap('u');
        }
        else if (other.tag == "dDoor")
        {
            transform.Translate(0, -2.6f, 0);
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y - GameManager.instance.px_y;
            GameManager.miniMap.UpdateMap('d');
        }
        else if (other.tag == "lDoor")
        {
            transform.Translate(-4.0f, 0, 0);
            cameraMove.x = cameraView.position.x - GameManager.instance.px_x;
            cameraMove.y = cameraView.position.y;
            GameManager.miniMap.UpdateMap('l');
        }
        else if (other.tag == "rDoor")
        {
            transform.Translate(4.0f, 0, 0);
            cameraMove.x = cameraView.position.x + GameManager.instance.px_x;
            cameraMove.y = cameraView.position.y;
            GameManager.miniMap.UpdateMap('r');
        }
        SetState(MOVECAMERA);
    }

    //攻击
    public void Attack()
    {
        Instantiate(weapon, transform.position, Quaternion.identity);
        GameManager.instance.PropEffectClearing();
    }
    private void SetState(string state)
    {
        this.state = state;
    }
}
