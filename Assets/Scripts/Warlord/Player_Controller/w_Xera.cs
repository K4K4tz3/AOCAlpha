using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_Xera : MonoBehaviour, IDamagable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass xeraSO;


    public void GetDamaged(float damage)
    {
        if (xeraSO.healthAmount > 0.0f)
        {
            xeraSO.healthAmount -= damage;
        }
        else
        {
            Die();
        }
    }
    public void Die()
    {
        
    }

  //ability 2 buffs autoattack and gives 12 more dmg for 6 sec
  //t�tet das ziel (ziel= das opfer des eigenen teams)

    //ability 3 macht rauch (0dmg) und nach 4 sec eine explosion (25dmg/10dmg)
    //range 1m = range der explosion
    //unsichtbar im rauch
    //keine anderen abilities m�glich, nur aa
    //erneutes dr�cken  der f�higkeit = explosion

  
}
