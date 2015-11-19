using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Enemy0,
    Enemy1,
    Enemy2
}

public class AI : MonoBehaviour
{

    //敌人类型枚举
    public EnemyType enemyType = EnemyType.Enemy0;

    public float speed = 1;
    public int HP = 50;
    public Transform player;

    //敌人状态 普通状态 追击主角状态 攻击主角状态
    private const string NORMAL = "normal";
    private const string CHASE = "chase";
    private const string ATTACK = "attack";
    private const string THINK = "think";
    private const string SlOWDOWN = "slowdown";

    private float aiThankLastTime = 0;                                  //记录敌人上一次思考时间
    private string state;
    private Vector3 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time - aiThankLastTime >= 3.0f)
        {
            aiThankLastTime = Time.time;
            target = player.position;
            if(enemyType==EnemyType.Enemy2)
            {
                target = GameManager.instance.GetDirectionForce(transform.position, target, speed);
            }
            SetState(THINK);
        }
    }

    void FixedUpdate()
    {
        switch (enemyType)
        {
            case EnemyType.Enemy0:
                UpdateEnemyType0();
                break;
            case EnemyType.Enemy1:
                UpdateEnemyType1();
                break;
            case EnemyType.Enemy2:
                UpdateEnemyType2();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerWeapon")
        {
            HP -= other.GetComponent<Weapon>().damage;
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //GetComponent<Rigidbody2D>().Sleep();
            //SetState(NORMAL);
            SetState(SlOWDOWN);
        }
        if (other.gameObject.tag == "Background")
        {
            SetState(SlOWDOWN);
        }
    }

    //更新第一种敌人的AI
    private void UpdateEnemyType0()
    {
        if (state == THINK)
        {
            MoveTo();
            if ((Mathf.Abs(transform.position.x - target.x) < 0.01f) && (Mathf.Abs(transform.position.y - target.y) < 0.01f))
            {
                SetState(NORMAL);
            }
        }
    }

    //更新第二种敌人的AI
    private void UpdateEnemyType1()
    {
        Track();
    }

    private void UpdateEnemyType2()
    {
        if (state == THINK)
        {
            RunTowards();
        }
        else if(state == SlOWDOWN)
        {
            GetComponent<Rigidbody2D>().AddForce(-GetComponent<Rigidbody2D>().velocity*3f);
            if(Vector2.SqrMagnitude(GetComponent<Rigidbody2D>().velocity) < 3f)
            {
                GetComponent<Rigidbody2D>().Sleep();
                SetState(NORMAL);
            }
        }
    }

    private void SetState(string state)
    {
        this.state = state;
    }

    private void Track()
    {
        Vector3 target = GameManager.instance.GetDirectionForce(transform.position, player.position, speed);
        GetComponent<Rigidbody2D>().velocity = target;
    }
    private void MoveTo()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        targetX = Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * speed);
        targetY = Mathf.Lerp(transform.position.y, target.y, Time.deltaTime * speed);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    private void RunTowards()
    {  
        GetComponent<Rigidbody2D>().AddForce(target);
    }
}