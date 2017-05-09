using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Boss : AI
{
    public new void Start()
    {
        base.Start();
        moveSpeed = 5;
        tearSpeed = bloodtear.GetComponent<Bloodtear>().speed;
    }

    // Update is called once per frame
    public new void Update()
    {
        if (Time.time - thankTime >= 2.0f && state == NORMAL)
        {
            thankTime = Time.time;
            ChooseMoveWay();
            if (state == WALK)
            {
                ChooseAttactWay();
            }
        }
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case WALK:
                collideDamage = 10f;
                RandomWalk(1.5f, 0.5f, 0.8f);
                break;
            case CHASE:
                collideDamage = 15f;
                if (Time.time - thankTime >= 1f)
                {
                    SetState(SlOWDOWN);
                }
                else
                {
                    RunTowards();
                }
                break;
            case SlOWDOWN:
                SlowDown();
                break;
            case NORMAL:
                collideDamage = 10f;
                break;
        }
    }
    
    void ChooseMoveWay()
    {
        int val = Random.Range(0, 100);
        if (val < 20)
        {
            if (GameManager.enemyCount <= 3)
            {
                GameManager.instance.CreateEnemy(GameManager.instance.GetComponent<Room>().enemy[3], transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
            }
            else
            {
                walkTime = Time.time;
                RandomWalk(1.5f, 0.5f, 0.8f);
            }
        }
        else if(val <90)
        {
            walkTime = Time.time;
            RandomWalk(1.5f, 0.5f, 0.8f);
        }
        else
        {
            SetState(CHASE);
            target = GameManager.instance.GetVelocity(transform.position, player.position, moveSpeed * 2);
            RunTowards();
        }
    }

    void ChooseAttactWay()
    {
        char RP = GetRelativePosition();
        if (RP != 'n')
        {
            ScatterShoot(RP);
        }
        else
        {
            int val = Random.Range(0, 100) % 4;
            switch (val)
            {
                case 0:
                    RadioShoot(4);
                    break;
                case 1:
                    RadioShoot(20);
                    break;
                case 2:
                    RandomShoot(30);
                    break;
                case 3:
                    XShoot();
                    break;
            }
        }
    }



    //X弹，同时向矩形四个顶点呈X形状发射
    void XShoot()
    {
        GameObject[] tear = new GameObject[4];
        tear[0] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear[0].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed, tearSpeed);
        tear[1] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear[1].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed, tearSpeed);
        tear[2] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear[2].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed, -tearSpeed);
        tear[3] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear[3].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed, -tearSpeed);
    }

    //分散弹，向某个方向分散发射，类似霰弹
    void ScatterShoot(char RP)
    {
        GameObject[] tear = new GameObject[3];
        tear[0] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear[1] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        tear[2] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
        switch (RP)
        {
            case 'u':
                tear[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0, tearSpeed);
                tear[1].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed / 4, tearSpeed);
                tear[2].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed / 4, tearSpeed);
                break;
            case 'd':
                tear[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0, -tearSpeed);
                tear[1].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed / 4, -tearSpeed);
                tear[2].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed / 4, -tearSpeed);
                break;
            case 'l':
                tear[0].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed, 0);
                tear[1].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed, tearSpeed / 4);
                tear[2].GetComponent<Rigidbody2D>().velocity = new Vector2(-tearSpeed, -tearSpeed / 4);
                break;
            case 'r':
                tear[0].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed, 0);
                tear[1].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed, tearSpeed / 4);
                tear[2].GetComponent<Rigidbody2D>().velocity = new Vector2(tearSpeed, -tearSpeed / 4);
                break;
        }
    }

    //随机弹，同时大量发射，速度随机，num为弹幕数量
    void RandomShoot(int num)
    {
        GameObject[] tear = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            tear[i] = Instantiate(bloodtear, transform.position, Quaternion.identity) as GameObject;
            tear[i].GetComponent<Rigidbody2D>().velocity = new Vector2(GetRandomForce(1.5f, 4f), GetRandomForce(1.5f, 4f));
        }
    }
}
