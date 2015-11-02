using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public int speed = 5;                                     //武器速度
    public int damage = 10;                                   //武器攻击力
    public int range = 4;                                     //武器射程
    public int attackSpeed = 3;                               //攻速

    private float startX;
    private float force = 0f;
	// Use this for initialization
    void Start()
    {
        startX = transform.position.x;
        if (Input.GetAxis("Horizontal") < 0)
            speed = -speed;
    }
	
	// Update is called once per frame
	void Update () 
    {
          if(Mathf.Abs(transform.position.x - startX) >= range)
          {
              Destroy(gameObject);
          }

	}

    void FixedUpdate()
    {
        force -= Time.deltaTime*2;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        if(Mathf.Abs(transform.position.x - startX) >= range - 1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, force);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != "Player" && other.tag != "Weapon")
            Destroy(gameObject);
    }
}
