using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour, IDamagable
{

    [SerializeField] private MinionSO minionSO;

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void GetDamaged(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void GetDamagedByTurret(float damage, float speed)
    {
       minionSO.minionHealth -= damage ;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
