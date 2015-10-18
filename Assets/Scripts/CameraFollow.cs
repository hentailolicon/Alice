using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;		// 摄像机开始跟随玩家的水平距离
    public float yMargin = 1f;		// 摄像机开始跟随玩家的垂直距离
    public float xSmooth = 8f;		// 摄像机水平移动的平滑度(越小平滑度越高)
    public float ySmooth = 8f;		// 摄像机垂直移动的平滑度(越小平滑度越高)
    public Vector2 maxXAndY;		// 摄像机能水平和垂直移动的距离最大值
    public Vector2 minXAndY;		// 摄像机能水平和垂直移动的距离最小值


    private Transform player;		// 玩家transform.


    void Awake()
    {
        // 获得玩家transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    private bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void FixedUpdate()
    {
        TrackPlayer();
    }


    private void TrackPlayer()
    {
        //目标坐标
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
            // 对摄像机水平位移进行渐变取值，实现平滑移动效果
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        if (CheckYMargin())
            // 对摄像机垂直位移进行渐变取值，实现平滑移动效果
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        //使摄像机移动的距离不超过允许的值
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        //设置摄像机的坐标
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
    public void TrackPlayer(Transform camera, Vector2 move)
    {
        float targetX = camera.position.x;
        float targetY = camera.position.y;

        targetX = Mathf.Lerp(camera.position.x, move.x, Time.deltaTime * 10);
        targetY = Mathf.Lerp(camera.position.y, move.y, Time.deltaTime * 10);

        camera.position = new Vector3(targetX, targetY, camera.position.z);
    }
}
