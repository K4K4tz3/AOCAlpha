using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WarlordBaseClass : ScriptableObject
{
    [Header("Warlord Stats")]
    public float healthAmount;
    public float chardAmount;
    public float movementSpeed;

    [Header("Auto Attack")]
    public float autoAttackRange;
    public float autoAttackDamage;

    [Header("Ability 1")]
    public float ability1Cooldown;
    public float ability1DmgWarlord;
    public float ability1DmgMinion;
    public float ability1DmgBuilding;
    public float ability1ChardCost;
    public float ability1Range;
    public float ability1Duration;
    

    [Header("Ability 2")]
    public float ability2Cooldown;
    public float ability2DmgWarlord;
    public float ability2DmgMinion;
    public float ability2DmgBuilding;
    public float ability2ChardCost;
    public float ability2Range;
    public float ability2Duration;
    

    [Header("Ability 3")]
    public float ability3Cooldown;
    public float ability3DmgWarlord;
    public float ability3DmgMinion;
    public float ability3DmgBuilding;
    public float ability3ChardCost;
    public float ability3Range;
    public float ability3Duration;
    

    [Header("Animation & Character Control")]
    //definition for wind-up & wind-down in documentation
    public float ability1DelayBefore;
    public float ability2DelayBefore;
    public float ability3DelayBefore;
    public float ability1DelayAfter;
    public float ability2DelayAfter;
    public float ability3DelayAfter;

}
