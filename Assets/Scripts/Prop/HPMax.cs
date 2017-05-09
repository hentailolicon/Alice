using UnityEngine;
using System.Collections;

public class HPMax : Prop
{

    void Start()
    {
        propName = "救赎之心";
        effect = "立即回复所有HP";
    }

    public override void Active()
    {
        if (GameManager.instance.GetPlayerProp() == null)
        {
            gameObject.SetActive(false);
            GameManager.instance.SetPlayerProp(this);
        }
    }

    public override void OtherEffect()
    {
        GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.HP, (int)GameManager.instance.GetPlayerAttributeValue(GameManager.PlayerAttribute.HPMax));
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
