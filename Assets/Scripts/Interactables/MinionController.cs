using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour, IDamagable
{

    public enum MinionState
    {
        walking,
        attacking
    }

    [SerializeField] private MinionSO minionSO;
    [SerializeField] private MinionHealthbar healthbar;
    private float maxHealth;
    private Color teamColor;
    public Team team;

    private Collider minionCollider;
    private NavMeshAgent minionNavAgent;
    [SerializeField] private Transform targetDestinationLeftTeam;
    [SerializeField] private Transform targetDestinationRightTeam;
    private Transform currentTargetDestination;
    [SerializeField] private LayerMask layerAttackable;

    [SerializeField] private List<Transform> waypointsLeftTeam;
    [SerializeField] private List<Transform> waypointsRightTeam;



    public MinionState state;


    private void Start()
    {
        minionNavAgent = GetComponent<NavMeshAgent>();
        minionNavAgent.speed = minionSO.minionWalkSpeed;
        minionNavAgent.radius = minionSO.distanceToOtherMinions;

        minionCollider = GetComponent<Collider>();

        healthbar = GetComponentInChildren<MinionHealthbar>();
        maxHealth = minionSO.minionHealth;
        healthbar.UpdateHealthbar(minionSO.minionHealth, maxHealth);

        if (team == Team.LeftTeam)
        {
            currentTargetDestination = targetDestinationLeftTeam;
            gameObject.tag = "MinionLeftTeam";
            teamColor = Color.blue;
            Debug.Log("running to: " + currentTargetDestination.name);

        }
        else
        {
            currentTargetDestination = targetDestinationRightTeam;

            gameObject.tag = "MinionRightTeam";
            teamColor = Color.red;

        }

        gameObject.GetComponent<Renderer>().material.color = teamColor;

        state = MinionState.walking;
    }


    private void Update()
    {
        switch (state)
        {
            case MinionState.walking:
                MoveMinion(currentTargetDestination.position);
                break;
            case MinionState.attacking:
                AttackEnemy();
                break;
        }

    }


    private void MoveMinion(Vector3 destination)
    {
        minionNavAgent.SetDestination(destination);

        if (IsEnemyInAutoAttackRange())
        {
            state = MinionState.attacking;
            minionNavAgent.ResetPath();
        }
    }

    private bool IsEnemyInAutoAttackRange()
    {
        //Checks if there are enemies in auto attack range
        Collider[] enemies = Physics.OverlapSphere(transform.position, minionSO.minionSightRange, layerAttackable);

        if (enemies != null)
        {
            //if there are enemies, check what team the enemies are in
            //if they are not in the same team, return true
            foreach (Collider eo in enemies)
            {
                if (eo.TryGetComponent(out PlayerController pc))
                {
                    if (pc.team != team)
                    {
                        minionNavAgent.SetDestination(pc.transform.position);
                        Debug.Log("Enemy: " + eo + " is in team " + pc.team);
                        return true;
                    }
                    else
                        return false;
                }
                //if a minion is in range, attack it
                if (eo.TryGetComponent(out MinionController mc))
                {
                    if (mc.team != team)
                    {
                        Debug.Log("Enemy: " + eo + " is in team " + mc.team);
                        minionNavAgent.SetDestination(mc.transform.position);
                        return true;
                    }
                    else
                        return false;
                }
                if (eo.TryGetComponent(out TurretController tc))
                {
                    if (tc.team != team)
                    {
                        minionNavAgent.SetDestination(tc.transform.position);
                        Debug.Log("Enemy: " + eo + " is in team " + tc.team);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            return false;
        }

        else
            return false;

    }

    private void AttackEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, minionSO.minionAttackRange, layerAttackable);

        if (enemies != null)
        {
            foreach (Collider enemy in enemies)
            {
                //loop through all found enemies and run to the first one in list
                if (enemy.TryGetComponent(out IDamagable d))
                {

                    //check team, otherwise it will attack minions from the same team

                    //run to attackable object
                    currentTargetDestination = enemies[0].transform;
                    minionNavAgent.SetDestination(currentTargetDestination.position);



                    //
                    Debug.Log(minionNavAgent.destination);


                    d.GetDamaged(minionSO.minionAttackDamage, minionCollider);



                }
            }

        }
        else
            return;
    }



    public void Die()
    {
        //Die animation
        Destroy(gameObject);
    }


    public void GetDamaged(float damage, Collider damageDealer)
    {
        if (minionSO.minionHealth > 0)
        {
            minionSO.minionHealth -= damage;
            healthbar.UpdateHealthbar(minionSO.minionHealth, maxHealth);
        }
        else
        {
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minionSO.minionAttackRange);
    }

}
