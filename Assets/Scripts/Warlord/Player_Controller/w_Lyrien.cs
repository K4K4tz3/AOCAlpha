using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_Lyrien : MonoBehaviour, IDamagable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass lyrienSO;

    private int layerAttackable;
    private Camera mainCamera;

    private float standardHealthAmount;
    private float standardChardAmount;

    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");

        standardHealthAmount = lyrienSO.healthAmount;
        standardChardAmount = lyrienSO.chardAmount;
    }

    #region AutoAttacks
    public void OnAutoAttack()
    {
        //Can only attack something "Attackable"
        //if player rightklicks on something with the layer "attackable", do aa
        //range depends on the warlord

        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == layerAttackable)
            {
                if (Vector3.Distance(transform.position, hit.point) <= lyrienSO.autoAttackRange)
                {
                    //doingAutoAttack = true;
                    DoAutoAttack(hit.transform.gameObject);
                }
            }
        }
    }
    private void DoAutoAttack(GameObject enemy)
    {
        //Do damage on the klicked object
        if (enemy.gameObject.TryGetComponent(out IDamagable d))
        {
            d.GetDamaged(lyrienSO.autoAttackDamage);
        }

        Debug.Log("AutoAttack");
    }

    #endregion

    #region Abilities

    public void OnAbility1()
    {
        Debug.Log("Ability1");
        
    }

    public void OnAbility2()
    {
        Debug.Log("Ability2");
       
    }

    public void OnAbility3()
    {
        Debug.Log("Ability3");
        
    }

    #endregion

    #region Damage & Death

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

    private void ResetStats()
    {
        lyrienSO.healthAmount = standardHealthAmount;
        lyrienSO.chardAmount = standardChardAmount;
    }

    public void Respawn()
    {
        ResetStats();
        //position at spawn point
    }

    #endregion

    //ability 1: kein damage- kann gegner steuern für 5sec (soldat)
    //ability 2: rnge bei wwarlords x0.5, gegner fliehen
    //ability 3: jump 4m reichweite
}
