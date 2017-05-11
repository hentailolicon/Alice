using UnityEngine;
using System.Collections;

public class Power : Prop {

    private float time;
    private bool used = false;

    void Start()
    {
        propName = "大力";
        effect = "5秒內伤害提升100%";
    }

    public override void Active()
    {
        OneTimePropActive();
    }

    public override void OtherEffect()
    {
        if (!used)
        {
            used = true;
            time = Time.time;
            if (!GameManager.instance.props.Exists(p => p.propName == propName))
            {
                GameManager.instance.props.Add(this);
            }
            GameManager.instance.SetPlayerProp(null);
        }
        if (Time.time - time < 5)
        {
            GameManager.instance.PAV.damage *= 2;
        }
        else
        {
            GameManager.instance.props.Remove(this);
            Destroy(gameObject);
        }
    }
}
