using UnityEngine;
using System.Collections;

public class Prop : MonoBehaviour 
{
    public string PropName;
    public string effect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Active() { }

    public virtual void OtherEffect() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        Active();
    }
}
