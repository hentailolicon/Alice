using UnityEngine;
using System.Collections;

public class Bloodtear : MonoBehaviour {

    public float speed = 4f;                                     //武器速度
    public float damage = 10f;                                   //武器攻击力
    public float range = 7f;                                     //武器射程

    private float startX;
    private float startY;
    private bool isPlay = false;
    // Use this for initialization
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlay)
        {
            AnimatorStateInfo animatorInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime >= 1 && animatorInfo.IsName("Base Layer.Bloodtear_play"))
            {
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isPlay)
        {
            if (Mathf.Pow((transform.position.x - startX), 2) + Mathf.Pow((transform.position.y - startY), 2) >= Mathf.Pow(range, 2))
            {
                PlayEffect();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rock" || other.tag == "Background")
        {
            PlayEffect();
        }
        else if(other.tag == "Player")
        {
            PlayEffect();
        }
    }

    void PlayEffect()
    {
        GetComponent<Animator>().SetBool("isPlay", true);
        isPlay = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }
}
