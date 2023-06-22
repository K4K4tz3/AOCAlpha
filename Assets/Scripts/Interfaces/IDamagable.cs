using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void GetDamaged(float damage);

    public void GetDamagedByTurret(float damage, float speed);

    public void Die();


}
