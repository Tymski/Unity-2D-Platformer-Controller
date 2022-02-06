using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public Camera TheCameraThatFollowTheObject;
    public GameObject TheGameObjectThatCameraNeedToFollowed;
    /// <summary>
    /// 相机Y轴高度补偿的值
    /// </summary>
    public float YAxisOffset = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.TheCameraThatFollowTheObject != null)
        {
            if (this.TheGameObjectThatCameraNeedToFollowed != null)
            {
                Vector3 temp = new Vector3(TheGameObjectThatCameraNeedToFollowed.transform.position.x, TheGameObjectThatCameraNeedToFollowed.transform.position.y + YAxisOffset
                    , TheCameraThatFollowTheObject.transform.position.z);

                TheCameraThatFollowTheObject.transform.position = temp;
                return;
            }
        }
        throw (new UnityException("物体跟随为空,请检查是否赋予物体节点"));
    }
}
