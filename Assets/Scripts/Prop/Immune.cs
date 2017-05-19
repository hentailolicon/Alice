using UnityEngine;
using UnityEngine.UI;
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
        GameManager.instance.effectImage.SetActive(true);
        GameObject.Find("EffectImage").GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
        GameObject.Find("TimeText").GetComponent<Text>().text = "10";
        GameManager.instance.SetPlayerImmuneTime(10f);
        GameManager.instance.SetPlayerProp(null);
        Destroy(gameObject);
    }
}
