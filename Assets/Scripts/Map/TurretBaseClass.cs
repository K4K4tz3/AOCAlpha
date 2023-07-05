using UnityEngine;

[CreateAssetMenu]
public class TurretBaseClass : ScriptableObject
{

    [Header("Damage & Chards")]
    [Tooltip("The damage the turret is dealing on minions and warlords. 20 dmg/s")] 
    public float turretDamage;

    [Tooltip("Fire Rate 1/s")] 
    public float turretSpeed;

    [Tooltip("Turret Attack Range is as large as regeneration range")] 
    public float turretAttackRange;

    public float chardomancerZoneRange;

    [Tooltip("Turrets charge Warlords' Chards. 5/s")] 
    public float chardRegenerationRate;
   
    [Tooltip("400 Chards for Base")] 
    public float chardStock;

    [Tooltip("Upgradable slot to add Chardomancer to turret. Chardomancer help regenerate Chards at a higher Chard-Rate")] 
    public float chardomancerSlot;

    [Header("For taking Turret")]                               //if the towerpoints fall to 0, the warlord has taken it -> so it's on the warlords side
    public float pointsLeftTeam;
    public float pointsRightTeam;
    public float totalTurretPoints;

    [Header("Health & Lifetime")]
    public float turretHP;                                      //After the turret was captured, the enemy warlord or minions can reduce it's HP by attacking it
                                                                //If it's HP are 0, the turret get's destroyed 
}


