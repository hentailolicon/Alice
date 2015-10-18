using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public Vector2 speed = new Vector2(3, 3);                        //玩家速度

    private Vector2 movement;                                        //玩家位移 = 玩家速度 * 方向
    private Animator anim;                                           //玩家动画控制器
    private CameraFollow camerafollow = new CameraFollow();
    private Transform camera;                                        //主视角transform
    private bool isCameraMove = false;                               //摄像机是否在移动
    private Vector2 cameraMove = new Vector2(0, 0);                  //摄像机将要移动到的坐标

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;     //获得主摄像机transform
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        if (isCameraMove)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);           //停止玩家移动
            //设置动画
            anim.SetFloat("zySpeed", 0);
            anim.SetFloat("sxSpeed", 0);
            //移动摄像机
            camerafollow.TrackPlayer(camera, cameraMove);
            //判断摄像机是否到达预定地点
            if ((Mathf.Abs(cameraMove.x - camera.position.x) < 0.01) && (Mathf.Abs(cameraMove.y - camera.position.y) < 0.01))
            {
                isCameraMove = false;
            }
        }
        else
        {
            anim.SetFloat("zySpeed", Mathf.Abs(inputX));
            anim.SetFloat("sxSpeed", Mathf.Abs(inputY));
            movement = new Vector2(speed.x * inputX, speed.y * inputY);
            GetComponent<Rigidbody2D>().velocity = movement;                     //移动玩家
        }
    }
	void OnTriggerEnter2D (Collider2D other)
	{
	/*	if (other.tag == "bd") 
		{
			other.gameObject.SetActive (false);
		//	Destroy(other.gameObject);
		}*/
        CameraMove(other);
	}
    
    //移动摄像机
    private void CameraMove(Collider2D other)
    {
        if (other.tag == "uDoor")
        {
            transform.Translate(0, 2.6f, 0);
            cameraMove.x = camera.position.x;
            cameraMove.y = camera.position.y + GameManager.instance.px_y;
        }
        else if (other.tag == "dDoor")
        {
            transform.Translate(0, -2.6f, 0);
            cameraMove.x = camera.position.x;
            cameraMove.y = camera.position.y - GameManager.instance.px_y;
        }
        else if (other.tag == "lDoor")
        {
            transform.Translate(-4.0f, 0, 0);
            cameraMove.x = camera.position.x - GameManager.instance.px_x;
            cameraMove.y = camera.position.y;
        }
        else if (other.tag == "rDoor")
        {
            transform.Translate(4.0f, 0, 0);
            cameraMove.x = camera.position.x + GameManager.instance.px_x;
            cameraMove.y = camera.position.y;
        }
        isCameraMove = true;
    }
}
