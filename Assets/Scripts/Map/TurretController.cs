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

    //Turrets können eingenommen werden
    //Sie starten mit 0 Punkten und in einem neutralen Zustand
    //Der Turm kann eingenommen werden, indem er angegriffen wird 
    //Der jenige, der dem Turm von 200 punkten den meisten Schaden macht, nimmt den Turm ein

    [SerializeField] private TurretBaseClass turretSO;
    [SerializeField] private FocusState currentFocusState;
    [SerializeField] private AffiliateState currentAffiliateState;


    [SerializeField] private Collider currentTarget;
    [SerializeField] private LayerMask layerAttackable;
    [SerializeField] private GameObject targetGameObject;

    private float attackCooldown;
    private float currentCooldown = 0f;

    

    private void Awake()
    {
        //Turrets start neutral
        currentFocusState = FocusState.neutral;
        currentAffiliateState = AffiliateState.neutral;

        attackCooldown = turretSO.turretCooldown;
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



    #region Damage & Death
    public void GetDamaged(float damage, Collider damageDealer)
    {
        //1. When game starts, turret has 200 points
        //2. If enemy hits turret, credit him points (damage)

        
        

        //if turret has 200 points and is neutral, check how many damage what team did 
        Debug.Log("Turret is getting damaged");
    }

    public void Die()
    {
        //play destroy animation
    }

    #endregion
}
