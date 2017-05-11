using UnityEngine;
using System.Collections;

public class BoomX10 : Prop {

    void Start()
    {
        propName = "炸弹X10";
        effect = "立即获得10枚炸弹";
    }

    public override void Active()
    {
        OneTimePropActive();
    }

    public override void OtherEffect()
    {
        GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.BOMB, 10);
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
