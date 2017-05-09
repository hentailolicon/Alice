using UnityEngine;
using System.Collections;
using System.IO;

public class Prop : MonoBehaviour 
{
    public string propName;
    public string effect;

    //应用道具固定永久数值效果
    public virtual void Active() { }

    //应用道具随机或者比例数值效果
    public virtual void OtherEffect() { }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Active();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BombExplosion")
        {
            GetComponent<Rigidbody2D>().velocity = GameManager.instance.GetVelocity(other.transform.position - new Vector3(0, 0.3f, 0), transform.position, 2.5f);
        }
    }
}
