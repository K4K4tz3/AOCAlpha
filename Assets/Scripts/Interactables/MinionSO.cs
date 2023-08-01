using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MinionSO : ScriptableObject
{
    [Header("NavMesh & AI")]
    public float minionWalkSpeed;
    [Tooltip("Distance is needed to avoid minions from stacking and overlapping")] public float distanceToOtherMinions;


    [Header("Damage & Health")]
    public float minionAttackDamage;
    public float minionAttackSpeed;


    [Header("Waves")]
    public float minionCountPerWave;
    [Tooltip("This is the time that goes by to spawn a new minion wave")] public float spawnTimer;
   
}
