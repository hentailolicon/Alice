using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Prop : MonoBehaviour 
{
    public string propName;
    public string effect;

    //应用道具固定永久数值效果
    public virtual void Active() { }

    //应用道具随机或者比例数值效果
    public virtual void OtherEffect() { }

    void Awake()
    {
        if (GameManager.roomTypeBoard[GameManager.site_x, GameManager.site_y] == 100)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Active();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BombExplosion")
        {
            GetComponent<Rigidbody2D>().velocity = GameManager.instance.GetVelocity(other.transform.position - new Vector3(0, 0.3f, 0), transform.position, 2.5f);
        }
        else if (other.tag == "Player")
        {
            Trade();
        }
    }

    public void OneTimePropActive()
    {
        if (GameManager.instance.GetPlayerProp() == null)
        {
            ShowPropInfo();
            gameObject.SetActive(false);
            GameManager.instance.SetPlayerProp(this);
            Image img = GameObject.Find("PropImage").GetComponent<Image>();
            img.enabled = true;
            img.sprite = GetComponent<SpriteRenderer>().sprite;
            Text text = GameObject.Find("PropText").GetComponent<Text>();
            text.text = propName + "\t—";
        }
    }

    public void ShowPropInfo()
    {
        GameObject.Find("PropNameText").GetComponent<Text>().text = propName;
        GameObject.Find("EffectText").GetComponent<Text>().text = effect;
        GameManager.instance.ShowPropInfo();
    }

    void Trade()
    {
        int coin = (int)GameManager.instance.GetPlayerAttributeValue(GameManager.PlayerAttribute.COIN);
        if(propName == "Heart")
        {
            UpdateCoin(coin, 3);
        }
        else if (propName == "Bomb")
        {
            UpdateCoin(coin, 5);
        }
        else
        {
            UpdateCoin(coin, 15);
        }
    }

    void UpdateCoin(int coin, int val)
    {
        if (coin >= val)
        {
            GameManager.instance.UpdatePlayerAttributeValue(GameManager.PlayerAttribute.COIN, -val);
            GetComponent<Collider2D>().isTrigger = false;
            Active();
        }
    }

}
