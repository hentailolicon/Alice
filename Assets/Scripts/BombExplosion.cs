using UnityEngine;
using System.Collections;

public class BombExplosion : MonoBehaviour {

    public float damage = 50f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Rock")
        {
            Destroy(other.gameObject);
        }
    }
}
