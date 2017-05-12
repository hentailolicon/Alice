using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float speed = 5f;                                     //武器速度
    public float damage = 10f;                                   //武器攻击力
    public float range = 4f;                                     //武器射程
    public float attackSpeed = 3f;                               //攻速

    public AudioClip[] audioClip;

    private AudioSource audioSource;
    private float startX;
    private float startY;
    private float force = 0f;
    private Vector2 speedVector;
    private bool isPlay = false;
	// Use this for initialization
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
        //if (Input.GetAxis("Horizontal") < 0)
        //    speed = -speed;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            speed = -speed;
            speedVector = new Vector2(speed, 0);
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            speedVector = new Vector2(0, speed);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            speedVector = new Vector2(0, -speed);
        }
        else
        {
            speedVector = new Vector2(speed, 0);
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip[0];
        audioSource.Play();
    }
	
	// Update is called once per frame
	void Update () 
    {

        if(isPlay)
        {
            AnimatorStateInfo animatorInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime >= 1 && animatorInfo.IsName("Base Layer.Tear_play"))
            {
                audioSource.clip = audioClip[1];
                audioSource.Play();
                Destroy(gameObject);
            }
        }
	}

    void FixedUpdate()
    {
        if (!isPlay)
        {
            force -= Time.deltaTime * 2;
            if (Mathf.Abs(transform.position.x - startX) >= range - 1)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, force);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = speedVector;
            }
            if ((Mathf.Abs(transform.position.x - startX) >= range) || (Mathf.Abs(transform.position.y - startY) >= range))
            {
                PlayEffect();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rock" || other.tag == "Background" || other.tag == "Enemy" || other.tag == "Poop")
        {
            PlayEffect();
        }
    }

    void PlayEffect()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetBool("isPlay", true);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        isPlay = true;
    }
}
