using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TurretBaseClass : ScriptableObject
{

    [Header("Damage & Chards")]
    public float turretDamage;
    public float chardStock;

    [Header("For Taking Turret")]
    //if the towerpoints fall to 0, the warlord has taken it -> so it's on the warlords side
    public float allyPoints;
    public float enemyPoints;
}

//turret states represent for which side the turret fights
public enum TurretState
{
    allied,
    neutral,
    hostile
}
