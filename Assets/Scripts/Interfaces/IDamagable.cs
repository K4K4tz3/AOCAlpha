using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void GetDamaged(float damage, Collider damageDealer);


    public void Die();


}
