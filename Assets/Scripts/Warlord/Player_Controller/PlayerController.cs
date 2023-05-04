using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region State Machine Fields
    #endregion

    #region Movement Fields
    #endregion

    private WarlordController inputAction;
    private NavMeshAgent navMeshAgent;
    private Camera mainCamera;


    private void Awake()
    {
        inputAction = new WarlordController();
        mainCamera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
     
        
    }

    private void OnEnable()
    {
        inputAction.W_Controller.Enable();
    }

    private void OnDisable()
    {
        inputAction.W_Controller.Disable();
    }




    private void Update()
    {
        HandleMovement();
        
    }

    private void HandleMovement()
    {
        if(!inputAction.W_Controller.Movement.WasPressedThisFrame())
        {
            return;
        }
        else if(inputAction.W_Controller.Movement.WasPressedThisFrame())
        {
         
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                navMeshAgent.destination = hit.point;
            }
        }
    

    }
}
