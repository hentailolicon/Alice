using UnityEngine;
using System;
using System.Collections;

public class Clover:Prop
{
    private System.Random rand = new System.Random();
    void Start()
    {
        propName = "四叶草";
        effect = "幸运+2，攻击时有4%的几率增加444%的基础伤害";
    }

    public override void Active()
    {
        gameObject.SetActive(false);            //设置物品为非激活状态，此状态物品消失但对象还存在。
        GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.LUCK, 2);
        if (!GameManager.instance.props.Exists(p => p.propName == propName))
        {
            GameManager.instance.props.Add(this);
        }
    }

    public override void OtherEffect()
    {
        int value = rand.Next(100);
        if (value < 4)
        {
            GameManager.instance.PAV.damage += GameManager.instance.GetPlayerAttributeValue(GameManager.PlayerAttribute.DAMAGE) * 4.44f;
        }
    }
}

