using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{

    void Start()
    {
        if (BillboardSprite.cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        }
    }

    public static Transform cam;
    public Vector3 freeRotation = Vector3.one;
    protected Vector3 eangles = Vector3.zero;
    public Transform sprite;

    protected void LateUpdate()
    {
        Vector3 forward = BillboardSprite.cam.forward;
        sprite.rotation = Quaternion.LookRotation(forward, BillboardSprite.cam.up);
        //sprite.Rotate(0, 180, 0);
        eangles = sprite.transform.eulerAngles;
        eangles.x *= freeRotation.x;
        eangles.y *= freeRotation.y;
        eangles.z *= freeRotation.z;
        sprite.eulerAngles = eangles;
    }
}
