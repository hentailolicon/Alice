using UnityEngine;
using System.Collections;

public class Enemy4 : AI {

    private Animator anim;
    // Use this for initialization
    public new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        tearSpeed = bloodtear.GetComponent<Bloodtear>().speed;
    }

    // Update is called once per frame
    public new void Update()
    {
        anim.SetFloat("positionSub", player.position.x - transform.position.x);
        RandomWalk(1f, 0.5f, 0.8f);
        if (Time.time - thinkTime >= Random.Range(2f,3f))
        {
            thinkTime = Time.time;
            Shoot(tearSpeed);
        }
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Background")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

}
