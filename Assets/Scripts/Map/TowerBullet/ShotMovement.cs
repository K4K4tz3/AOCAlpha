using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMovement : MonoBehaviour
{

    private float moveSpeed = 20f;
    private Vector3 moveDirection;


  
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyImmediate(gameObject);
    }
}
