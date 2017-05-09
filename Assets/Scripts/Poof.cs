using UnityEngine;
using System.Collections;

public class Poof : MonoBehaviour {

    public string poofName;

    private Animator anim;
    private bool isPlay = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isPlay)
        {
            AnimatorStateInfo animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime >= 1 && animatorInfo.IsName("Base Layer." + poofName))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            anim.SetBool("isPlay", true);
            isPlay = true;
        }
	}
}
