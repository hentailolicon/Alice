using UnityEngine;
using System.Collections;

public class Immune : Prop
{
    void Start()
    {
        propName = "绝对领域";
        effect = "10秒内免疫所有伤害";
    }

    public override void Active()
    {
        OneTimePropActive();
    }

    public override void OtherEffect()
    {
        GameManager.instance.SetPlayerImmuneTime(10f);
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
