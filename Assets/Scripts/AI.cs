using UnityEngine;
using System.Collections;
using Alice;
using System.Reflection;
using System;

public class AI : MonoBehaviour
{
    public float speed = 1;
    public float HP = 50;
    protected Transform player;

    //敌人状态 普通状态 追击主角状态 攻击主角状态
    public const string NORMAL = "normal";
    public const string CHASE = "chase";
    public const string ATTACK = "attack";
    public const string THINK = "think";
    public const string SlOWDOWN = "slowdown";

    public float aiThankLastTime = 0;                                  //记录敌人上一次思考时间
    public string state;
    public Vector3 target;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        if (Time.time - aiThankLastTime >= 3.0f)
        {
            aiThankLastTime = Time.time;
            target = player.position;
            SetState(THINK);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerWeapon")
        {
            HP -= GameManager.instance.PAV.damage;
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


    public void SetState(string state)
    {
        this.state = state;
    }

    public void Track()
    {
        Vector3 target = GameManager.instance.GetDirectionForce(transform.position, player.position, speed);
        GetComponent<Rigidbody2D>().velocity = target;
    }
    public void MoveTo()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        targetX = Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * speed);
        targetY = Mathf.Lerp(transform.position.y, target.y, Time.deltaTime * speed);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    public void RunTowards()
    {  
        GetComponent<Rigidbody2D>().AddForce(target);
    }
    public void SlowDown()
    {
        GetComponent<Rigidbody2D>().AddForce(-GetComponent<Rigidbody2D>().velocity * 3f);
        if (Vector2.SqrMagnitude(GetComponent<Rigidbody2D>().velocity) < 3f)
        {
            GetComponent<Rigidbody2D>().Sleep();
            SetState(NORMAL);
        }
    }
}