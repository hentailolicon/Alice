using UnityEngine;
using System.Collections;

public class Poop : MonoBehaviour {

    public float chance;
    private float HP = 40f;
    private Animator anmi;
	// Use this for initialization
	void Start () {
        anmi = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (HP > 1)
        {
            if (other.tag == "PlayerWeapon")
            {
                Attacked(other.GetComponent<Weapon>().damage);
            }
            else if (other.tag == "BombExplosion")
            {
                Attacked(other.GetComponent<BombExplosion>().damage);
            }
        }
    }

    void Attacked(float damaged)
    {
        GameManager.instance.PropEffectClearing();
        HP -= (GameManager.instance.PAV.damage * damaged / 10);
        anmi.SetFloat("HP", HP);
        if(HP <= 1)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            gameObject.tag = "Untagged";
            if(Random.Range(0, 100) <= chance)
            {
                GameManager.instance.CreateProp(2, transform.position - new Vector3(0, 0, 0.5f));
            }
        }
    }
}
