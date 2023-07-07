using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour, IDamagable
{

    [SerializeField] private MinionSO minionSO;

    private NavMeshAgent minionNavAgent;
    [SerializeField] private Transform targetDestination;
    private Transform currentTargetDestination;
    [SerializeField] private LayerMask layerAttackable;



    private void Awake()
    {
        minionNavAgent = GetComponent<NavMeshAgent>();
        minionNavAgent.speed = minionSO.minionWalkSpeed;
        minionNavAgent.radius = minionSO.distanceToOtherMinions;

        currentTargetDestination = targetDestination;
    }


    private void Update()
    {

        //After Spawning, Minions walk directly in a straight line through map
        if (minionNavAgent != null)
        {
            MoveInDirection();
        }

        //if there are hostile turrets, attack them
        //if there are hostile warlords or minions, attack them
    }


    private void MoveInDirection()
    {
        minionNavAgent.SetDestination(targetDestination.position);

        Collider[] enemies = Physics.OverlapSphere(transform.position, minionSO.minionAttackRange, layerAttackable);

        //Check if the first enemy in the List is damagable 
        if (enemies != null)
        {
            if (enemies[0].TryGetComponent(out IDamagable d))
            {
                //if so, set it as target destination 
                targetDestination = enemies[0].transform;

            }
        }


    }

    private void AttackEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, minionSO.minionAttackRange, layerAttackable);


        foreach (Collider enemy in enemies)
        {
            //loop through all found enemies and run to the first one in list
            if (enemy.TryGetComponent(out IDamagable d))
            {
                currentTargetDestination = enemies[0].transform;
            }
        }
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
