using UnityEngine;
using System.Collections;

public class Enemy1 : AI
{
    void FixedUpdate()
    {
        Track();
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == "PlayerWeapon")
        //{
        //    HP -= GameManager.instance.PAV.damage;
        //    if (HP <= 0)
        //    {
        //        Destroy(gameObject);
        //        Vector3 randVal = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        //        Instantiate(enemy, transform.position + randVal, Quaternion.identity);
        //        randVal = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        //        Instantiate(enemy, transform.position + randVal, Quaternion.identity);
        //        GameManager.enemyCount++;
        //    }
        //}
        if (other.tag == "PlayerWeapon")
        {
            Attacked(other.GetComponent<Weapon>().damage, CreateSubEnemy);
        }
        else if (other.tag == "BombExplosion")
        {
            Attacked(other.GetComponent<BombExplosion>().damage, CreateSubEnemy);
        }
    }

    void CreateSubEnemy()
    {
        Vector3 randVal = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(GameManager.instance.GetComponent<Room>().enemy[1], transform.position + randVal, Quaternion.identity);
        randVal = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        Instantiate(GameManager.instance.GetComponent<Room>().enemy[1], transform.position + randVal, Quaternion.identity);
        GameManager.enemyCount += 2;
    }
}
