using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MinionAttackTrigger : MonoBehaviour
{
    private MinionController controller;
   


    private void Start()
    {
        controller = GetComponentInParent<MinionController>();
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out PlayerController pc))
        {
            if (pc.team != controller.team && pc.TryGetComponent(out IDamagable d))
            {
                controller.IsEnemyInAARange = true;
            }
            else
                return;
        }
        else if (other.TryGetComponent(out TurretController tc))
        {
            if (tc.team != controller.team && tc.TryGetComponent(out IDamagable d))
            {
                controller.IsEnemyInAARange = true;
            }
            else
                return;
        }
        else if (other.TryGetComponent(out MinionController mc))
        {
            if (mc.team != controller.team && mc.TryGetComponent(out IDamagable d))
            {
                controller.IsEnemyInAARange = true;
            }
            else
                return;
        }
        else
            return;
    }

    private void OnTriggerExit(Collider other)
    {
        controller.IsEnemyInAARange = false;
    }
}
