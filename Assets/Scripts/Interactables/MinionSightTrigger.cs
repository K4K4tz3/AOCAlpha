using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSightTrigger : MonoBehaviour
{
    private MinionController controller;


    void Start()
    {
        controller = GetComponentInParent<MinionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController pc))
        {
            if(pc.team != controller.team)
            {
                controller.IsEnemyInSight = true;
                controller.CurrentTargetDestination = other.gameObject.transform;
            }
            else
                return;
        }
        else if(other.TryGetComponent(out TurretController tc))
        {
            if (tc.team != controller.team)
            {
                controller.IsEnemyInSight = true;
                controller.CurrentTargetDestination = other.gameObject.transform;
            }
            else
                return;
        }
        else if(other.TryGetComponent(out MinionController mc))
        {
            if(mc.team != controller.team)
            {
                controller.IsEnemyInSight = true;
                controller.CurrentTargetDestination = other.gameObject.transform;
            }
            else
                return;
        }
        else 
            return ;



        Debug.Log("Enemy is in sight: " + other);
     
    }

    private void OnTriggerExit(Collider other)
    {
        controller.IsEnemyInSight = false;
    }
}
