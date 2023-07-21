using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour, IDamagable
{

    [SerializeField] private MinionSO minionSO;

    private Collider minionCollider;

    private NavMeshAgent minionNavAgent;
    [SerializeField] private Transform targetDestinationLeftTeam;
    [SerializeField] private Transform targetDestinationRightTeam;
    private Transform currentTargetDestination;
    [SerializeField] private LayerMask layerAttackable;

    public Team team;


    private void Start()
    {
        minionNavAgent = GetComponent<NavMeshAgent>();
        minionNavAgent.speed = minionSO.minionWalkSpeed;
        minionNavAgent.radius = minionSO.distanceToOtherMinions;

        minionCollider = GetComponent<Collider>();

        //update team
        Debug.Log(team);


        if (team == Team.LeftTeam)
        {
            currentTargetDestination = targetDestinationLeftTeam;
            gameObject.tag = "MinionLeftTeam";
            Debug.Log("running to: " + currentTargetDestination.name);

        }
        else
        {
            currentTargetDestination = targetDestinationRightTeam;
            gameObject.tag = "MinionRightTeam";

        }
    }


    private void Update()
    {

        //After Spawning, Minions walk directly in a straight line through map
        if (minionNavAgent != null)
        {

            MoveMinion();
        }

        if (IsEnemyInAutoAttackRange())
        {
            AttackEnemy();
        }

    }


    private void MoveMinion()
    {
        minionNavAgent.SetDestination(currentTargetDestination.position);
    }

    private bool IsEnemyInAutoAttackRange()
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
                        return true;
                    }
                    else
                        return false;
                }
                if (eo.TryGetComponent(out TurretController tc))
                {
                    if (tc.team != team)
                    {
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
                    //run to attackable object
                    currentTargetDestination = enemies[0].transform;

                    //attack target
                    //if turret is neutral - add points to team
                    //if not - deal damage
                    d.GetDamaged(minionSO.minionAttackDamage, minionCollider);


                    Debug.Log("wanna attack: " + enemy);
                    //check the team of the target
                    //only attack if it's in the enemys' team
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
        }
        else
        {
            Die();
        }
    }

}
