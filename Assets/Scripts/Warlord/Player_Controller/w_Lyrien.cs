using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class w_Lyrien : MonoBehaviour
{
    private int layerAttackable;
    private Camera mainCamera;

    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass warlordBaseSO;


    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");
    }

    //On... Methods are for PlayerInput Component
    //(methods send unity messages when player triggered button)

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
                if (Vector3.Distance(transform.position, hit.point) <= warlordBaseSO.autoAttackRange)
                {
                    //doingAutoAttack = true;
                    DoAutoAttack();
                }
            }
        }
    }
    public void DoAutoAttack()
    {
        //Do damage
        Debug.Log("AutoAttack");
    }

    public void OnAbility1()
    {
        Debug.Log("Ability1");
        //doingAbility1 = true;
    }

    public void OnAbility2()
    {
        Debug.Log("Ability2");
        //doingAbility2 = true;
    }

    public void OnAbility3()
    {
        Debug.Log("Ability3");
        //doingAbility3 = true;
    }

    public void GetDamaged(float damage)
    {

    }
    public void Die()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, warlordBaseSO.autoAttackRange);
    }
}
