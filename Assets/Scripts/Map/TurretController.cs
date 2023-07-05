
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamagable
{
    public enum AffiliateState                                      //turret states represent for which side the turret fights
    {
        leftTeam,
        neutral,
        rightTeam
    }

    public enum FocusState                                          //represents if the turrets is attacking only minions or warlords
    {
        neutral,
        alerted,
        aggro,
        destroyed
    }

    public enum EnemyType
    {
        minion,
        warlord
    }


    //Turrets können eingenommen werden
    //Sie starten mit 0 Punkten und in einem neutralen Zustand
    //Der Turm kann eingenommen werden, indem er angegriffen wird 
    //Der jenige, der dem Turm von 200 punkten den meisten Schaden macht, nimmt den Turm ein

    [SerializeField] private TurretBaseClass turretSO;
    [SerializeField] private FocusState currentFocusState;
    [SerializeField] private AffiliateState currentAffiliateState;
    [SerializeField] private EnemyType targetEnemyType;

    [SerializeField] private Collider currentTarget;

    [SerializeField] private LayerMask layerAttackable;
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();

    [SerializeField] private List<Collider> targetableMinions = new List<Collider>();

    [SerializeField] private GameObject targetGameObject;


    private float attackCooldown = 1f;
    private float currentCooldown = 0f;

    private void Awake()
    {
        //Turrets start neutral
        currentFocusState = FocusState.neutral;
        currentAffiliateState = AffiliateState.neutral;
    }

    private void Start()
    {
        //InvokeRepeating("DoRangeCheck", 0f, 0.5f);
    }

    private void Update()
    {

        switch (currentFocusState)
        {
            case FocusState.neutral:
                FindTarget();
                break;
            case FocusState.alerted:
                if (currentTarget != null)
                {
                    if (IsEnemyOfTargetType(currentTarget))
                    {
                        AttackTarget(currentTarget);
                    }
                    else
                    {
                        currentFocusState = FocusState.neutral;
                    }
                }
                else
                {
                    currentFocusState = FocusState.neutral;
                }
                break;
            case FocusState.aggro:
                if (currentTarget != null)
                {
                    AttackTarget(currentTarget);
                    break;
                }
                currentFocusState = FocusState.neutral;
                break;
            case FocusState.destroyed:
                break;
        }

    }


    private void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, turretSO.turretAttackRange, layerAttackable);

        Collider targetEnemy = null;

        //Minion Loop
        foreach (Collider enemy in enemies)
        {
            if (IsEnemyOfMinionType(enemy))
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                    break;
                }
            }
        }

        //Warlord Loop
        if (targetEnemy == null)
        {
            foreach (Collider enemy in enemies)
            {
                if (IsEnemyOfWarlordType(enemy))
                {
                    targetEnemy = enemy;
                }
            }
        }


        if (targetEnemy != null)
        {
            currentFocusState = FocusState.alerted;
            currentTarget = targetEnemy;
        }

    }
    private bool IsEnemyOfTargetType(Collider enemy)
    {
        string enemyTag = enemy.tag;
        if (enemyTag == "HostileMinion")
        {
            return true;
        }
        else if (enemyTag == "Warlord")
        {
            return true;
        }

        return false;
    }

    private bool IsEnemyOfMinionType(Collider enemy)
    {
        string enemyTag = enemy.tag;
        if (enemyTag == "HostileMinion")
        {
            return true;
        }
        else if (enemyTag == "Warlord")
        {
            return false;
        }

        return false;
    }

    private bool IsEnemyOfWarlordType(Collider enemy)
    {
        string enemyTag = enemy.tag;
        if (enemyTag == "HostileMinion")
        {
            return false;
        }
        else if (enemyTag == "Warlord")
        {
            return true;
        }

        return false;
    }

    private void AttackTarget(Collider target)
    {
        if (currentCooldown <= 0f)
        {
            // Attack the target enemy
            Debug.Log("Attacking enemy: " + target.name);

            // TODO: Add code to damage the enemy or perform other attack actions

            currentCooldown = attackCooldown;
        }

        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0f)
        {
            currentFocusState = FocusState.neutral; // Transition back to idle state
        }


    }
    public void TriggerAggro(Collider targetWarlord)
    {
        currentFocusState = FocusState.aggro;
        currentTarget = targetWarlord;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, turretSO.turretAttackRange);
    }


    #region Damage & Death
    public void GetDamaged(float damage, Collider damageDealer)
    {
        //if turret has 200 points and is neutral, check how many damage what team did 
        Debug.Log("Turret is getting damaged");
    }

    public void GetDamagedByTurret(float damage, float speed)
    {

    }

    public void Die()
    {
        //play destroy animation
    }

    #endregion
}
