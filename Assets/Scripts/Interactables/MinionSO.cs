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
    public float minionAttackRange;
    public float minionAttackDamage;
    public float minionAttackSpeed;
    public float minionHealth;
    public float minionSightRange;

    [Header("Waves")]
    public float minionCountPerWave;
    [Tooltip("When game starts, wait this amount of time before spawning the first minions")] public float firstSpawnTimer;
    [Tooltip("This is the time that goes by to spawn a new minion wave")] public float spawnTimer;
   
}
