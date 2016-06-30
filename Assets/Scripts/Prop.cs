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

    //应用道具固定永久数值效果
    public virtual void Active() { }

    //应用道具随机或者比例数值效果
    public virtual void OtherEffect() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        Active();
    }
}
