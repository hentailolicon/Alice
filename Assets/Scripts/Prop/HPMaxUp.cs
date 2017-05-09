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
        if (GameManager.instance.GetPlayerProp() == null)
        {
            gameObject.SetActive(false);
            GameManager.instance.SetPlayerProp(this);
        }
    }

    public override void OtherEffect()
    {
        GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.HPMax, 20);
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
