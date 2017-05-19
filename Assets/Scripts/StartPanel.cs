using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartPanel : MonoBehaviour {

    int count;
    public int end;
    Image opeartion;
    Image arrow;
    Text loading;
    float moveTime;
    bool isOpeartion;
	// Use this for initialization
	void Start () {
        isOpeartion = false;
        end = 0;
        count = 0;
        opeartion = GameObject.Find("Opeartion").GetComponent<Image>();
        arrow = GameObject.Find("Arrow").GetComponent<Image>();
        loading = GameObject.Find("Loading").GetComponent<Text>();
        loading.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isOpeartion)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                end = 1000;
            }
            MoveImg(opeartion.rectTransform);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(-1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(1);
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Run();
            }
        }
	}

    void Move(int val)
    {
        float x = 20f;
        float y = 150f;
        if(val<0)
        {
            if(count>0)
            {
                count--;
                arrow.rectTransform.localPosition += new Vector3(x, y, 0);
            }
        }
        else
        {
            if(count < 2)
            {
                count++;
                arrow.rectTransform.localPosition += new Vector3(-x, -y, 0);
            }
        }
    }

    void Run()
    {
        switch(count)
        {
            case 0:
                loading.enabled = true;
                Application.LoadLevel("1");
                break;
            case 1:
                isOpeartion = true;
                break;
            case 2:
                Application.Quit();
                break;
        }
    }


    public void MoveImg(RectTransform start)
    {
        if (Mathf.Abs(start.localPosition.y - end) > 10f)
        {
            float y = Mathf.Lerp(start.localPosition.y, end, Time.deltaTime * 15);
            start.localPosition = new Vector3(start.localPosition.x, y, start.localPosition.z);
        }
        else
        {
            if (end == 1000)
            {
                isOpeartion = false;
                end = 0;
            }
        }

    }
}
