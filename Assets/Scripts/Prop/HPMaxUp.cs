using UnityEngine;
using System.Collections;

public class HPMaxUp : Prop
{

    void Start()
    {
        propName = "打吊针";
        effect = "HP最大值+20";
    }

    public override void Active()
    {
        OneTimePropActive();
    }

    public override void OtherEffect()
    {
        GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.HPMax, 20);
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
