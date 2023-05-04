using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region State Machine Fields
    private W_MovementBaseState currentState;
    public W_IdleState IdleState = new W_IdleState();
    public W_WalkState WalkState = new W_WalkState();
    public W_AbilityOne AbilityOneState = new W_AbilityOne();
    public W_AbilityTwo AbilityTwoState = new W_AbilityTwo();
    public W_AbilityThree AbilityThreeState = new W_AbilityThree();
    public W_DeathState DeathState = new W_DeathState();

    #endregion

    #region Movement Fields
    private WarlordController inputAction;
    private NavMeshAgent navMeshAgent;
    private Camera mainCamera;
    #endregion



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
