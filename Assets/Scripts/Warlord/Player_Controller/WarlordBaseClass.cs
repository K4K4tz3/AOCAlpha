using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WarlordBaseClass : ScriptableObject
{
    public float autoAttackRange;
    public float autoAttackDamage;

    public float ability1Cooldown;
    public float ability1Damage;
    public float ability1ChardCost;
    public float ability1Range;

    public float ability2Cooldown;
    public float ability2Damage;
    public float ability2ChardCost;
    public float ability2Range;

    public float ability3Cooldown;
    public float ability3Damage;
    public float ability3ChardCost;
    public float ability3Range;

    public float chardAmount;
    public float healthAmount;
    public float movementSpeed;

    //definition for wind-up & wind-down in documentation
    public float windUp;
    public float windDown;

}
