using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    #region State Machine Fields
    private W_MovementBaseState currentState;
    public W_IdleState IdleState = new W_IdleState();
    public W_WalkState WalkState = new W_WalkState();
    public W_AutoAttackState AAState = new W_AutoAttackState();
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

    [HideInInspector] public Animator anim;

    private void Awake()
    {
        inputAction = new WarlordController();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();


        SwitchState(IdleState);
    }

    private void OnEnable()
    {
        inputAction.W_Controller.Enable();
    }

    private void OnDisable()
    {
        inputAction.W_Controller.Disable();
    }

    public void SwitchState(W_MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }




    private void Update()
    {
        HandleMovement();
        currentState.UpdateState(this);

    }

    private void HandleMovement()
    {
        if (!inputAction.W_Controller.Movement.WasPressedThisFrame())
        {
            return;
        }
        else if (inputAction.W_Controller.Movement.WasPressedThisFrame())
        {

            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                navMeshAgent.destination = hit.point;
            }
        }


    }

    public void OnAutoAttack()
    {
        //Can only attack something "Attackable"

        Debug.Log("AutoAttack");


    }

    public void OnAbility1()
    {
        Debug.Log("Ability1");
    }

    public void OnAbility2()
    {
        Debug.Log("Ability2");
    }

    public void OnAbility3()
    {
        Debug.Log("Ability3");
    }
}
