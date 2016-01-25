using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float speed = 5f;                                     //武器速度
    public float damage = 10f;                                   //武器攻击力
    public float range = 4f;                                     //武器射程
    public float attackSpeed = 3f;                               //攻速

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
        if(other.tag != "Player" && other.tag != "PlayerWeapon")
            Destroy(gameObject);
    }
}
