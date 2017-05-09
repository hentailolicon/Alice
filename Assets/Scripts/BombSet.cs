using UnityEngine;
using System.Collections;

public class BombSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerExit2D(Collider2D other)
    {
        gameObject.GetComponent<Collider2D>().isTrigger = false;
    }
}
