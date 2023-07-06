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


    private Collider currentTarget;
    [SerializeField] private LayerMask layerAttackable;

    private float attackCooldown;
    private float currentCooldown = 0f;                     //CD is 0 in the beginning so the turret can shoot right away

    private float defaultTurretPoints = 200;                //Set points in the beginning (awake) to default -> 200 
    private float defaultPointsLeftTeam = 0;
    private float defaultPointsRightTeam = 0;

    private float defaultTurretHP = 500;                    //HP are "activated" when the tower is assigned to a team



    private void Awake()
    {
        //Turrets start neutral
        currentFocusState = FocusState.neutral;
        currentAffiliateState = AffiliateState.neutral;

        //Reset stats 
        attackCooldown = turretSO.turretCooldown;

        turretSO.totalTurretPoints = defaultTurretPoints;
        turretSO.pointsLeftTeam = defaultPointsLeftTeam;
        turretSO.pointsRightTeam = defaultPointsRightTeam;

        turretSO.turretHP = defaultTurretHP;

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
        //TODO: 
        //destroy nicht vergessen 

        //check if damageDealer has a playerController Script to get team membership
        // && if turret still has more than 0 points
        if (damageDealer.gameObject.TryGetComponent(out PlayerController pc) && turretSO.totalTurretPoints > 0)
        {
            //Credit the points to the right team
            //And reduce total turret points
            if (pc.team == Team.LeftTeam)
            {
                turretSO.pointsLeftTeam += damage;
                turretSO.totalTurretPoints -= damage;
                Debug.Log($"Turret is getting damaged by {pc.team}");
            }
            if (pc.team == Team.RightTeam)
            {
                turretSO.pointsRightTeam += damage;
                turretSO.totalTurretPoints -= damage;
                Debug.Log($"Turret is getting damaged by {pc.team}");
            }
        }

        //if turret points fall to 0 and the turret doesn't belong to any team
        if (turretSO.totalTurretPoints <= 0 && currentAffiliateState == AffiliateState.neutral)
        {
            //check which team has more points and change affiliate state
            if (turretSO.pointsLeftTeam > turretSO.pointsRightTeam)
            {
                currentAffiliateState = AffiliateState.leftTeam;
                Debug.Log($"Turret is now captured by {currentAffiliateState}");

            }
            else
            {
                currentAffiliateState = AffiliateState.rightTeam;
                Debug.Log($"Turret is now captured by {currentAffiliateState}");

            }
        }

        //Reduce HP if hp > 0 and the turret is assigned to a team
        if (damageDealer.gameObject.TryGetComponent(out PlayerController p) && turretSO.turretHP > 0 && currentAffiliateState != AffiliateState.neutral)
        {
            //first check what team is doing damage
            //only the team that's not in the same team as the turret can do damage
           
            if ((p.team == Team.LeftTeam && currentAffiliateState == AffiliateState.rightTeam) || (p.team == Team.RightTeam && currentAffiliateState == AffiliateState.leftTeam))
            {
                turretSO.turretHP -= damage;
                Debug.Log($"Turret is getting damaged by {p.team}");
            }          
            else
            {
                Debug.Log($"Turret {currentAffiliateState} and DamageDealer {p.team} are in the same team");
            }
          
        }



    }

    public void Die()
    {
        //play destroy animation
    }

    #endregion
}
