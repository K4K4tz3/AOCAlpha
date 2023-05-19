using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_Lyrien : MonoBehaviour, IDamagable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass lyrienSO;


    public void GetDamaged(float damage)
    {
        if (lyrienSO.healthAmount > 0.0f)
        {
            lyrienSO.healthAmount -= damage;
        }
        else
        {
            Die();
        }
    }
    public void Die()
    {
        
    }

    //ability 1: kein damage- kann gegner steuern für 5sec (soldat)
    //ability 2: rnge bei wwarlords x0.5, gegner fliehen
    //ability 3: jump 4m reichweite
}
