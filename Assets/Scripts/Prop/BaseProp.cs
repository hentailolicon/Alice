using UnityEngine;
using System.Collections;

public class BaseProp : Prop {

    public int num;
    public string kind;

    public override void Active()
    {
        bool flag = true;
        switch (kind)
        {
            case "bomb":
                flag = GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.BOMB, num);
                break;
            case "HP":
                flag = GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.HP, num);
                break;
            case "coin":
                flag = GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.COIN, num);
                break;
        }
        if (flag)
        {
            Destroy(gameObject);
        }
    }

    public override void OtherEffect()
    {

    }
}
