using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour, IDamagable
{

    [SerializeField] private MinionSO minionSO;
 
    private NavMeshAgent minionNavAgent;
    



   


    private void Awake()
    {
        minionNavAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
   


    }

    private void Update()
    {
       

       
        //After Spawning, Minions walk directly in a straight line through map

        //if there are hostile turrets, attack them
        //if there are hostile warlords or minions, attack them
    }



    private void SpawnMinionWave(Transform[] spawnPoints)
    {
       
    }


 

    private void MoveInDirection()
    {

    }

    private void AttackEnemy()
    {

    }


    public void Die()
    {

    }

    public void GetDamaged(float damage, Collider damageDealer)
    {

    }

}
