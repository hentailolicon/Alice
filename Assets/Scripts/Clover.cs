using UnityEngine;
using System;
using System.Collections;

public class Clover:Prop
{
    private System.Random rand = new System.Random();
    public Clover()
    {
        PropName = "四叶草";
        effect = "幸运+2，攻击时有4%的几率附加444%的基础伤害";
    }

    public override void Active()
    {
        GameManager.instance.UpdatePlayerState(GameManager.PlayerState.LUCK, 2);
        GameManager.instance.props.Add(this);
    }

    public override void OtherEffect(GameManager.PlayerStateValue PSV)
    {
        int value = rand.Next(100);
        if (value < 4)
        {
            PSV.damage *= 5.44f;
        }
    }
}

