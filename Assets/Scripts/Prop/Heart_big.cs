﻿using UnityEngine;
using System.Collections;

public class Heart_big : Prop
{

    void Start()
    {
        propName = "血包";
        effect = "立即回复50HP";
    }

    public override void Active()
    {
        OneTimePropActive();
    }

    public override void OtherEffect()
    {
        GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.HP, 50);
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
