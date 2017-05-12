using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Alice;
using System.Reflection;
using System;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float tearSpeed;
    public float HP = 50f;
    public float collideDamage = 10f;
    public GameObject bloodtear;
    public bool isBoss = false;
    protected Transform player;

    private Image healthBar;
    private Image healthBar_bg;
    private float hptmp;

    //敌人状态 普通状态 追击主角状态 攻击主角状态
    public const string NORMAL = "normal";
    public const string CHASE = "chase";
    public const string ATTACK = "attack";
    public const string THINK = "think";
    public const string SlOWDOWN = "slowdown";
    public const string WALK = "walk";

    public float thankTime;                                  //记录敌人上一次思考时间
    public float walkTime;
    public string state;
    public Vector3 target;

    public delegate void dele();
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thankTime = Time.time;
        walkTime = Time.time;
        state = NORMAL;
        if(isBoss)
        {
            healthBar = GameObject.Find("BossHealthBar").GetComponent<Image>();
            healthBar.enabled = true;
            healthBar.rectTransform.localPosition = new Vector3(0, 280f, 0);
            healthBar.fillAmount = 0.96f;
            healthBar_bg = GameObject.Find("BossHealthBar_bg").GetComponent<Image>();
            healthBar_bg.enabled = true;
            healthBar_bg.rectTransform.localPosition = healthBar.rectTransform.localPosition;
            hptmp = HP;
            GameManager.instance.ChangeBGM(true);
        }
    }

    public void Update()
    {
        //if (Time.time - aiThankLastTime >= 3.0f)
        //{
        //    aiThankLastTime = Time.time;
        //    target = player.position;
        //    SetState(THINK);
        //}
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerWeapon")
        {
            //Vector3 tmp = other.GetComponent<Rigidbody2D>().velocity / 20;
            //gameObject.transform.position += tmp;
            Attacked(other.GetComponent<Weapon>().damage, null);
        }
        else if (other.tag == "BombExplosion")
        {
            Attacked(other.GetComponent<BombExplosion>().damage, null);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //GetComponent<Rigidbody2D>().Sleep();
            //SetState(NORMAL);
            SetState(SlOWDOWN);
        }
        if (other.gameObject.tag == "Background")
        {
         //   SetState(SlOWDOWN);
        }
        if (target.x / GetComponent<Rigidbody2D>().velocity.x<0)
        {
            target.x = -target.x;
        }
        else if (target.y / GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            target.y = -target.y;
        }
        GetComponent<Rigidbody2D>().AddForce(target);
    }

    public void Attacked(float damaged, dele func)
    {
        GameManager.instance.PropEffectClearing();
        float finaldamage = GameManager.instance.PAV.damage * damaged /10;
        HP -= finaldamage;
        if(isBoss)
        {
            healthBar.fillAmount -= finaldamage / hptmp * 0.82f;
        }
        if (HP <= 0)
        {
            if(isBoss)
            {
                healthBar.fillAmount = 1;
                healthBar.enabled = false;
                healthBar_bg.enabled = false;
                GameManager.instance.ChangeBGM(false);
            }
            if(func != null)
            {
                func();
            }
            Destroy(gameObject);
            Instantiate(GameManager.instance.poof[1], transform.position, Quaternion.identity);
            GameManager.enemyCount--;
        }
    }

    public void SetState(string state)
    {
        this.state = state;
    }

    //以恒定速度追踪玩家
    public void Track()
    {
        Vector3 randVal = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
        Vector3 target = GameManager.instance.GetVelocity(transform.position, player.position + randVal, moveSpeed);
        GetComponent<Rigidbody2D>().velocity = target;
    }

    //快速移动到玩家位置，移动模式为由快到慢，且方向固定，不随玩家移动而改变，移动到玩家位置就会停下
    public void MoveTo()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        targetX = Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * moveSpeed);
        targetY = Mathf.Lerp(transform.position.y, target.y, Time.deltaTime * moveSpeed);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    //向玩家所在方向冲刺，移动模式为由慢到快，且方向固定，不随玩家移动而改变，移动到玩家位置也不会停下
    public void RunTowards()
    {  
        GetComponent<Rigidbody2D>().AddForce(target);
    }

    //减速，当速度下降到一定值后停下
    public void SlowDown()
    {
        GetComponent<Rigidbody2D>().AddForce(-GetComponent<Rigidbody2D>().velocity * 3f);
        if (Vector2.SqrMagnitude(GetComponent<Rigidbody2D>().velocity) < 3f)
        {
            GetComponent<Rigidbody2D>().Sleep();
            SetState(NORMAL);
        }
    }

    public void RandomWalk(float time, float minSpeed, float maxSpeed)
    {
        if (Time.time - walkTime >= time)
        {
            walkTime = Time.time;
            SetState(NORMAL);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else if (state != WALK)
        {
            float speed_x = GetRandomForce(minSpeed, maxSpeed);
            float speed_y = GetRandomForce(minSpeed, maxSpeed);
            float siteSub_x = transform.position.x % GameManager.instance.px_x;
            float siteSub_y = transform.position.y % GameManager.instance.px_y;
            if (siteSub_x > 8 && siteSub_x < 9)
            {
                speed_x = Mathf.Abs(speed_x);
            }
            else if (siteSub_x > 4 && siteSub_x < 5)
            {
                speed_x = -Mathf.Abs(speed_x);
            }
            if (siteSub_y > 4.5f && siteSub_y < 5.5f)
            {
                speed_y = Mathf.Abs(speed_y);
            }
            else if (siteSub_y > 2 && siteSub_y < 3)
            {
                speed_y = -Mathf.Abs(speed_y);
            }
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed_x, speed_y);
            SetState(WALK);
        }
    }

    public float GetRandomForce(float min, float max)
    {
        float result = max * Mathf.Cos(Random.Range(0f, Mathf.PI * 2));
        if (Mathf.Abs(result) < min)
        {
            if (result > 0)
            {
                result = min;
            }
            else
            {
                result = -min;
            }
        }
        return result;
    }

    //向玩家发射
    public void Shoot(float speed)
    {
        GameObject tear = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear.GetComponent<Rigidbody2D>().velocity = GameManager.instance.GetVelocity(transform.position, player.position, speed);
    }

    //圆形弹，以自身为圆心向所有方向发射，弹幕呈圆形，num为弹幕数量
    public void RadioShoot(int num)
    {
        float radian = Mathf.PI * 2 / num;
        GameObject[] tear = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            tear[i] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
            tear[i].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed * Mathf.Sin(radian * i), tearSpeed * Mathf.Cos(radian * i));
        }
    }
    public char GetRelativePosition()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) < 0.3f)
        {
            if (player.position.y > transform.position.y)
            {
                return 'u';
            }
            else
            {
                return 'd';
            }
        }
        else if (Mathf.Abs(player.position.y - transform.position.y) < 0.1f)
        {
            if (player.position.x > transform.position.x)
            {
                return 'r';
            }
            else
            {
                return 'l';
            }
        }
        else
        {
            return 'n';
        }
    }
}