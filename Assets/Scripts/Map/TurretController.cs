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

    private string turretNeutralTag = "NeutralTurret";
    private string turretLeftTeamTag = "LeftTeamTurret";
    private string turretRightTeamTag = "RightTeamTurret";


    private Collider currentTarget;
    [SerializeField] private LayerMask layerAttackable;
    private Collider turretCollider;


    //cd here in script bc of reset - not in so 
    private float attackCooldown = 3.5f;
    private float currentCooldown = 0f;                     //CD is 0 in the beginning so the turret can shoot right away


    //points wrsl auch hier besser für die türme einzeln
    private float maxTurretPoints = 200;
    private float currentTurretPoints;                //Set points in the beginning (awake) to default -> 200 
    private float defaultPointsLeftTeam = 0;
    private float defaultPointsRightTeam = 0;

    private float currentPointsLeftTeam = 0;
    private float currentPointsRightTeam = 0;

    private float defaultTurretHP = 500;                    //HP are "activated" when the tower is assigned to a team
    private float currentTurretHP;

    [SerializeField] private Transform shotSpawnPoint;
    [SerializeField] private GameObject turretShotPrefab;

    private TurretHealthbar healthbar;
    private TurretLTPointsBar leftTeamPointsBar;
    private TurretRTPointsBar rightTeamPointsBar;

    [SerializeField] private GameObject healthbarObj;
    [SerializeField] private GameObject leftTeamBarObj;
    [SerializeField] private GameObject rightTeamBarObj;

    #region Team Assignment
    [SerializeField] GameObject teamManagerObject;
    private TeamManager teamManager;
    public Team team;
    #endregion


    private void Awake()
    {
        currentTurretPoints = maxTurretPoints;
        turretCollider = GetComponent<Collider>();

        healthbar = GetComponentInChildren<TurretHealthbar>();
        leftTeamPointsBar = GetComponentInChildren<TurretLTPointsBar>();
        rightTeamPointsBar = GetComponentInChildren<TurretRTPointsBar>();

        if (healthbar != null)
        {
            healthbar.UpdateHealthbar(currentTurretHP, defaultTurretHP);

        }
        leftTeamPointsBar.UpdateLTPointsBar(currentPointsLeftTeam, currentTurretPoints);
        rightTeamPointsBar.UpdateRTPointsBar(currentPointsRightTeam, currentTurretPoints);

        //healthbar.gameObject.SetActive(false);

        //Turrets start neutral
        currentFocusState = FocusState.neutral;
        currentAffiliateState = AffiliateState.neutral;
        gameObject.tag = turretNeutralTag;

        currentTurretHP = defaultTurretHP;

        //for debug
        turretSO.totalTurretPoints = currentTurretPoints;
        turretSO.pointsLeftTeam = defaultPointsLeftTeam;
        turretSO.pointsRightTeam = defaultPointsRightTeam;
        turretSO.turretHP = defaultTurretHP;

        teamManager = teamManagerObject.GetComponent<TeamManager>();
        team = Team.None;

    }

    private void Update()
    {
        //Debug.Log("Current Focus State: " + currentFocusState);
        //Debug.Log("Current Cooldown: " + currentCooldown);
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
            currentTarget = targetEnemy;
            currentFocusState = FocusState.alerted;
        }

    }

    private bool IsEnemyOfTargetType(Collider enemy)
    {
        string enemyTag = enemy.tag;
        if (enemyTag == "MinionLeftTeam" || enemyTag == "MinionRightTeam")
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
        if (enemyTag == "MinionLeftTeam" || enemyTag == "MinionRightTeam")
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
            //if turret is neutral, attack anything with IDamagable interface implemented
            if (currentAffiliateState == AffiliateState.neutral)
            {
                if (target.TryGetComponent(out IDamagable d))
                {
                    //deal damage to anything
                    InstantiateShoot(target, target.transform.position);
                    d.GetDamaged(turretSO.turretDamage, turretCollider);
                    Debug.Log("Attacking enemy: " + target.name);

                }
            }
            else
            {
                //Check if target == Warlord && check team
                //playerController for checking team 
                if (target.tag == "Warlord" && target.TryGetComponent(out PlayerController pc))
                {
                    //if turret and warlord are not in the same team
                    if ((pc.team == Team.LeftTeam && currentAffiliateState == AffiliateState.rightTeam) || (pc.team == Team.RightTeam && currentAffiliateState == AffiliateState.leftTeam))
                    {
                        //deal damage to warlord
                        if (target.TryGetComponent(out IDamagable d))
                        {
                            InstantiateShoot(target, target.transform.position);
                            d.GetDamaged(turretSO.turretDamage, turretCollider);
                            Debug.Log("Attacking warlord: " + target.name);
                        }
                    }
                }
                //if target is not a warlord, check for team 
                else if (target.TryGetComponent(out MinionController mc))
                {
                    if ((mc.team == Team.LeftTeam && currentAffiliateState == AffiliateState.rightTeam) || (mc.team == Team.RightTeam && currentAffiliateState == AffiliateState.leftTeam))
                    {
                        //deal damage to minion
                        InstantiateShoot(target, target.transform.position);
                        mc.GetDamaged(turretSO.turretDamage, turretCollider);
                        Debug.Log("Attacking enemy: " + target.name);
                    }
                }
            }

            currentCooldown = attackCooldown;
            /*currentFocusState = FocusState.neutral;*/ // Transition back to neutral state

        }
        currentCooldown -= Time.deltaTime;

        //currentFocusState = FocusState.neutral;


        if (currentCooldown > 0f)
        {
            currentFocusState = FocusState.neutral; // Transition back to neutral state
        }


    }
    public void TriggerAggro(Collider targetWarlord)
    {
        currentFocusState = FocusState.aggro;
        currentTarget = targetWarlord;
    }

    private void InstantiateShoot(Collider target, Vector3 directionToTarget)
    {

        Debug.Log("instantiating shot");

        //instantiate the bullet at it's spawn point
        GameObject turretShot = Instantiate(turretShotPrefab, shotSpawnPoint.transform.position, shotSpawnPoint.transform.rotation);

        //Calculate direction to target
        directionToTarget = (target.transform.position - turretShot.transform.position).normalized;

        //Get ShotMovement script from the shot to move it
        ShotMovement shotMovement = turretShot.GetComponent<ShotMovement>();

        //pass the direction to the movement Script
        shotMovement.SetMoveDirection(directionToTarget);



    }



    #region Damage & Death
    public void GetDamaged(float damage, Collider damageDealer)
    {

        //Check if turret is destroyed
        //if so, return and don't do anything
        if (currentFocusState == FocusState.destroyed)
            return;


        //check if damageDealer has a playerController Script to get team membership
        // && if turret still has more than 0 points
        if (damageDealer.gameObject.TryGetComponent(out PlayerController pc) && currentTurretPoints > 0)
        {
            //Credit the points to the right team
            //And reduce total turret points
            if (pc.team == Team.LeftTeam)
            {
                currentPointsLeftTeam += damage;
                currentTurretPoints -= damage;
                leftTeamPointsBar.UpdateLTPointsBar(currentPointsLeftTeam, maxTurretPoints);
                Debug.Log($"Turret is getting damaged by a warlord from {pc.team}");
            }
            if (pc.team == Team.RightTeam)
            {
                currentPointsRightTeam += damage;
                currentTurretPoints -= damage;
                rightTeamPointsBar.UpdateRTPointsBar(currentPointsRightTeam, maxTurretPoints);
                Debug.Log($"Turret is getting damaged by a warlord from {pc.team}");
            }
        }
        //Minion check:
        if (damageDealer.gameObject.TryGetComponent(out MinionController mc) && currentTurretPoints > 0)
        {
            if (mc.team == Team.LeftTeam)
            {
                currentPointsLeftTeam += damage;
                currentTurretPoints -= damage;
                leftTeamPointsBar.UpdateLTPointsBar(currentPointsLeftTeam, maxTurretPoints);
                Debug.Log($"Turret is getting damaged by a minion from {mc.team}");
            }
            if (mc.team == Team.RightTeam)
            {
                currentPointsRightTeam += damage;
                currentTurretPoints -= damage;
                rightTeamPointsBar.UpdateRTPointsBar(currentPointsRightTeam, maxTurretPoints);
                Debug.Log($"Turret is getting damaged by a minion from {mc.team}");
            }
        }

        //if turret points fall to 0 and the turret doesn't belong to any team
        if (currentTurretPoints <= 0 && currentAffiliateState == AffiliateState.neutral)
        {
            leftTeamBarObj.SetActive(false);
            rightTeamBarObj.SetActive(false);
            healthbarObj.SetActive(true);
            //check which team has more points and change affiliate state
            if (currentPointsLeftTeam > currentPointsRightTeam)
            {
                currentAffiliateState = AffiliateState.leftTeam;
                teamManager.AssignTeamToObject(this.gameObject, Team.LeftTeam);
                team = teamManager.GetObjectsTeam(this.gameObject);
                gameObject.tag = turretLeftTeamTag;
                Debug.Log($"Turret is now captured by {currentAffiliateState}");

            }
            else
            {
                currentAffiliateState = AffiliateState.rightTeam;
                teamManager.AssignTeamToObject(this.gameObject, Team.RightTeam);
                team = teamManager.GetObjectsTeam(this.gameObject);
                gameObject.tag = turretRightTeamTag;
                Debug.Log($"Turret is now captured by {currentAffiliateState}");

            }
        }

        //Reduce HP if hp > 0 and the turret is assigned to a team
        if (damageDealer.gameObject.TryGetComponent(out PlayerController p) && currentTurretHP > 0 && currentAffiliateState != AffiliateState.neutral)
        {
            //first check what team is doing damage
            //only the team that's not in the same team as the turret can do damage

            if ((p.team == Team.LeftTeam && currentAffiliateState == AffiliateState.rightTeam) || (p.team == Team.RightTeam && currentAffiliateState == AffiliateState.leftTeam))
            {
                currentTurretHP -= damage;
                healthbar.UpdateHealthbar(currentTurretHP, defaultTurretHP);
                Debug.Log($"Turret is getting damaged by {p.team}");
            }
            else
            {
                Debug.Log($"Turret {currentAffiliateState} and DamageDealer {p.team} are in the same team. No Damage dealt");
            }

        }

        //destroy turret
        if (currentTurretHP <= 0)
        {
            //Set focusState to destroyed
            currentFocusState = FocusState.destroyed;
            Die();
        }



    }

    public void Die()
    {
        //play destroy animation
    }

    #endregion

}
