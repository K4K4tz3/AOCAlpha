using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //target
    //offset
    //angle

    //follow & unfollow
    //maybe zoom

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothness;
    private Vector3 cameraPos;
    [SerializeField] private Quaternion angle;

    void Start()
    {
        offset = transform.position - target.position;
    }


    void Update()
    {
        if(target != null)
        {
            
            cameraPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, cameraPos, smoothness * Time.fixedDeltaTime);
        }
    }
}
