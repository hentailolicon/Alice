using UnityEngine;
using System.Collections;
using Alice;

public class Player : MonoBehaviour
{

    public float speed = 3f;                                             //玩家移动速度
    public float HP = 30f;
    public float HPMax = 30f;
    public float luck = 2f;
    public GameObject weapon;

    //玩家状态 普通状态 移动视角状态 受到攻击状态
    private const string NORMAL = "normal";
    private const string MOVECAMERA = "movecamera";
    private const string ATTACKED = "attacked";

    private Vector2 movement;                                        //玩家位移 = 玩家速度 * 方向
    private Animator anim;                                           //玩家动画控制器
    private Transform cameraView;                                    //主视角transform
    private Vector2 cameraMove = new Vector2(0, 0);                  //摄像机将要移动到的坐标
    private float cooldown = 0;
    private float attackSpeed;
    private string state = NORMAL;
    private Vector3 repelForce;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        attackSpeed = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<Weapon>().attackSpeed;
        cameraView = GameObject.FindGameObjectWithTag("MainCamera").transform;     //获得主摄像机transform
        Vector2 start= new Vector2(MapAlgo.GetStartY() * GameManager.instance.px_x, MapAlgo.GetStartX() * GameManager.instance.px_y);
        transform.position = new Vector3(start.x,start.y,3);
        cameraView.position = new Vector3(start.x, start.y,-10);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
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
                movement = new Vector2(speed * inputX, speed * inputY);
                GetComponent<Rigidbody2D>().velocity = movement;                     //移动玩家
                if (Input.GetKey("j"))
                {
                    if (cooldown <= 0)
                    {
                        cooldown = 1f / attackSpeed;
                        Attack();
                    }
                }
                break;
            case MOVECAMERA:
                GetComponent<Rigidbody2D>().Sleep();                                  //停止玩家移动
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
                if (cooldown > 0)
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
            CameraMove(other);
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject .tag == "Enemy")
        {
            SetState(ATTACKED);
            repelForce = GameManager.instance.GetDirectionForce(other.transform.position, transform.position, 2.5f);
            cooldown = 0.3f;
        }
    }
    
    //移动摄像机
    private void CameraMove(Collider2D other)
    {
        if (other.tag == "uDoor")
        {
            transform.Translate(0, 2.6f, 0);
            cameraMove.x = cameraView.position.x;
            cameraMove.y = cameraView.position.y + GameManager.instance.px_y;
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
