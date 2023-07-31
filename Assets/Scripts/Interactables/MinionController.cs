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
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    private Color teamColor;
    public Team team;

    private Collider minionCollider;
    private NavMeshAgent minionNavAgent;
    public Transform targetDestinationLeftTeam;
    public Transform targetDestinationRightTeam;
    private float desiredStoppingDistance = 10;


    private Transform currentTargetDestination;
    [SerializeField] private LayerMask layerAttackable;

    public MinionState state;


    //anstatt overlap sphere -> collider trigger enter als child object

    private void Start()
    {
        minionNavAgent = GetComponent<NavMeshAgent>();
        minionNavAgent.speed = minionSO.minionWalkSpeed;
        minionNavAgent.radius = minionSO.distanceToOtherMinions;

        minionCollider = GetComponent<Collider>();

        healthbar = GetComponentInChildren<MinionHealthbar>();
        maxHealth = 300;
        currentHealth = maxHealth;
        healthbar.UpdateHealthbar(currentHealth, maxHealth);

        if (team == Team.LeftTeam)
        {
            currentTargetDestination = targetDestinationLeftTeam;
            gameObject.tag = "MinionLeftTeam";
            teamColor = Color.blue;

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

                if (IsEnemyInSightRange())
                {
                    MoveMinion(currentTargetDestination.position);
                }
                if (IsEnemyInAttackRange())
                {
                    minionNavAgent.ResetPath();
                    state = MinionState.attacking;

                }
                else
                {
                    MoveMinion(currentTargetDestination.position);

                }

                break;
            case MinionState.attacking:
                AttackEnemy();
                break;
        }

    }


    private void MoveMinion(Vector3 destination)
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination);

        Debug.Log("Distance to Destination: " + distanceToDestination);
        Debug.Log("Desired Stopping Distance: " + desiredStoppingDistance);
        Debug.Log(IsEnemyInAttackRange());
        Debug.Log(IsEnemyInSightRange());

        if (distanceToDestination > desiredStoppingDistance)
        {
            minionNavAgent.SetDestination(destination);
        }
        else if (IsEnemyInAttackRange())
        {
            minionNavAgent.ResetPath();
            state = MinionState.attacking;
        }
    }


    private bool IsEnemyInAttackRange()
    {
        //Checks if there are enemies in auto attack range
        Collider[] enemies = Physics.OverlapSphere(transform.position, minionSO.minionAttackRange, layerAttackable);

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
                        //Debug.Log("Enemy: " + eo + " is in team " + pc.team);
                        return true;
                    }
                    else
                        return false;
                }
                //if a minion is in range, attack it
                else if (eo.TryGetComponent(out MinionController mc))
                {
                    if (mc.team != team)
                    {
                        //Debug.Log("Enemy: " + eo + " is in team " + mc.team);
                        return true;
                    }
                    else
                        return false;
                }
                else if (eo.TryGetComponent(out TurretController tc))
                {
                    if (tc.team != team)
                    {
                        //Debug.Log("Enemy: " + eo + " is in team " + tc.team);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            return true;
        }
        else
            return false;

    }

    private bool IsEnemyInSightRange()
    {
        //Checks if there are enemies in range
        Collider[] enemies = Physics.OverlapSphere(transform.position, minionSO.minionSightRange, layerAttackable);

        if (enemies != null)
        {
            // If there are enemies, set the destination to the first one found
            foreach (Collider enemy in enemies)
            {
                if (enemy.TryGetComponent(out PlayerController pc))
                {
                    if (pc != null && pc.team != team)
                    {
                        currentTargetDestination = enemy.transform;
                        Debug.Log("new destination player = " + currentTargetDestination);
                        return true;
                    }
                }
                else if (enemy.TryGetComponent(out MinionController mc))
                {
                    if (mc != null && mc.team != team)
                    {
                        currentTargetDestination = enemy.transform;
                        Debug.Log("new destination minion = " + currentTargetDestination);
                        return true;
                    }
                }
                else if (enemy.TryGetComponent(out TurretController tc))
                {
                    if (tc != null && tc.team != team)
                    {
                        currentTargetDestination = enemy.transform;
                        Debug.Log("new destination turret = " + currentTargetDestination);
                        return true;
                    }
                }
            }
        }

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
                    //check tag, otherwise it will attack minions from the same team
                    if (enemy.gameObject.tag != gameObject.tag)
                    {
                        minionNavAgent.ResetPath();
                        d.GetDamaged(minionSO.minionAttackDamage, minionCollider);
                        Debug.Log("Minion attacking " + enemy);
                    }
                }

            }
        }
        else
        {
            //default destination
            if (team == Team.LeftTeam)
            {
                currentTargetDestination = targetDestinationLeftTeam;
                minionNavAgent.SetDestination(currentTargetDestination.position);
            }
            else
            {
                currentTargetDestination = targetDestinationRightTeam;
                minionNavAgent.SetDestination(currentTargetDestination.position);
            }

            state = MinionState.walking;
            return;
        }
    }



    public void Die()
    {
        //Die animation
        Destroy(gameObject);
    }
    public void GetDamaged(float damage, Collider damageDealer)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthbar.UpdateHealthbar(currentHealth, maxHealth);
        }
        else
        {
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minionSO.minionSightRange);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, minionSO.minionAttackRange);
    }

}
