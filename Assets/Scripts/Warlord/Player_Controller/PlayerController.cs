using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum Team
{
    LeftTeam,
    RightTeam,
    None

}

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

    #region State Machine Bools
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool doingAutoAttack;
    [HideInInspector] public bool doingAbility1;
    [HideInInspector] public bool doingAbility2;
    [HideInInspector] public bool doingAbility3;
    [HideInInspector] public bool isDead;
    #endregion

    #region Movement Fields
    private WarlordController inputAction;
    private NavMeshAgent navMeshAgent;
    private Camera mainCamera;

    private Vector3 lastVelocity;
    private float maxChange = 0.1f;
    #endregion

    #region Target Tags

    public List<string> _targetTags = new List<string>();
    private string turretNeutralTag = "NeutralTurret";
    private string turretLeftTeamTag = "LeftTeamTurret";
    private string turretRightTeamTag = "RightTeamTurret";
    private string minionLeftTeamTag = "MinionLeftTeam";
    private string minionRightTeamTag = "MinionRightTeam";
    private string warlordLeftTeamTag = "WarlordLeftTeam";
    private string warlordRightTeamTag = "WarlordRightTeam";
    #endregion

    #region Team Assignment
    [SerializeField] GameObject teamManagerObject;
    private TeamManager teamManager;
    public Team team;
    #endregion

    [HideInInspector] public Animator anim;

    [HideInInspector] public bool canGetInput = true;


    private void Awake()
    {
        inputAction = new WarlordController();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
        lastVelocity = navMeshAgent.velocity;

        teamManager = teamManagerObject.GetComponent<TeamManager>();
        team = Team.None;



        SwitchState(IdleState);
    }

    private void OnEnable()
    {
        inputAction.W_Controller.Enable();

    }
    private void Start()
    {
        if (teamManager.unassignedWarlords != null)
        {
            teamManager.unassignedWarlords.Add(this.gameObject);
            StartCoroutine(WaitForTeamAssignment());
        }

        //target tags get assigned after some waiting time by script
        _targetTags.Add(turretNeutralTag);

        StartCoroutine(WaitForTargetAssignment());


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
        CheckForMovement();

        currentState.UpdateState(this);
    }

    private void HandleMovement()
    {
        if (!inputAction.W_Controller.Movement.WasPressedThisFrame())
        {
            return;
        }
        else if (inputAction.W_Controller.Movement.WasPressedThisFrame() && canGetInput)
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                navMeshAgent.destination = hit.point;
            }
        }
    }
    private void CheckForMovement()
    {
        //Method checks if the player moved, based on his velocity change
        //bool for state machine

        Vector3 velocity = navMeshAgent.velocity;
        float dif = Mathf.Abs((velocity - lastVelocity).magnitude) / Time.fixedDeltaTime;
        if (dif > maxChange)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    IEnumerator WaitForTeamAssignment()
    {
        //This Coroutine is needed because we have to wait a bit for the TeamManager Script to assign teams to the warlords
        yield return new WaitForSeconds(1f);
        team = teamManager.GetObjectsTeam(this.gameObject);
    }

    IEnumerator WaitForTargetAssignment()
    {
        //This Coroutine is needed because we have to wait a bit for the TeamManager Script to assign teams to the warlords
        yield return new WaitForSeconds(1f);
        if (team == Team.LeftTeam)
        {
            _targetTags.Add(warlordRightTeamTag);
            _targetTags.Add(turretRightTeamTag);
            _targetTags.Add(minionRightTeamTag);

        }
        else if (team == Team.RightTeam)
        {
            _targetTags.Add(warlordLeftTeamTag);
            _targetTags.Add(minionLeftTeamTag);
            _targetTags.Add(turretLeftTeamTag);
        }

    }

}
