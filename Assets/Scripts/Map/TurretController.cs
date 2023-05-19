using System;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamagable
{

    [SerializeField] private TurretBaseClass turretSO;


    public void GetDamaged(float damage)
    {
        if (turretSO.turretHealth > 0.0f)
        {
            turretSO.turretHealth -= damage;

        }
        else if (turretSO.turretHealth <= 0.0f)
        {
            DestroyTurret();

        }
    }

    private void DestroyTurret()
    {
        GameObject.Destroy(gameObject);
    }
}
