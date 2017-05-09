using UnityEngine;
using System.Collections;

public class Enemy5 : AI {


    public new void Start()
    {
        base.Start();
        tearSpeed = bloodtear.GetComponent<Bloodtear>().speed;
    }
    void FixedUpdate()
    {
        Track();
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerWeapon")
        {
            Attacked(other.GetComponent<Weapon>().damage, CreateTear);
        }
        else if (other.tag == "BombExplosion")
        {
            Attacked(other.GetComponent<BombExplosion>().damage, CreateTear);
        }
    }

    void CreateTear()
    {
        RadioShoot(8);
    }
}
