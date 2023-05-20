using System.Linq;
using UnityEngine;
using System.Collections.Generic;
public class w_Heragzon : MonoBehaviour, IDamagable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass heragzonSO;
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();


    public LayerMask layerAttackable;
    private Camera mainCamera;

    private float standardHealthAmount;
    private float standardChardAmount;

    public bool inRange1;
    private Vector3 ability1Range;


    private void Awake()
    {
        mainCamera = Camera.main;
        //layerAttackable = LayerMask.NameToLayer("Attackable");

        ability1Range = new Vector3(heragzonSO.ability1Range, heragzonSO.ability1Range, heragzonSO.ability1Range);

        standardHealthAmount = heragzonSO.healthAmount;
        standardChardAmount = heragzonSO.chardAmount;
    }



    //On... Methods are for PlayerInput Component
    //(methods send unity messages when player triggered button)

    private void Update()
    {
        //check if something attackable is in range

        
        _targetsInRange = Physics.OverlapSphere(transform.position, heragzonSO.ability1Range, layerAttackable).Where((n) => n.tag == "Turret").ToList();

        foreach(Collider t in _targetsInRange)
            Debug.Log(t.tag);

        // foreach(var target in targetsInRange)
        // {
        //     Debug.Log(target.tag);
        //     if(target.CompareTag("Turret"))
        //     {
        //         inRange1 = true;
        //     }
        //     else
        //     {
        //         inRange1 = false;
        //     }
           
        // }
       
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
                if (Vector3.Distance(transform.position, hit.point) <= heragzonSO.autoAttackRange)
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
            d.GetDamaged(heragzonSO.autoAttackDamage);
        }

        Debug.Log("AutoAttack");
    }
    #endregion

    #region Abilities
    public void OnAbility1()
    {
        if (inRange1)
        {

            Debug.Log("Ability1");
        }


        //falls ein ziel in der range ist, set active eine trigger area in richtung des ziels, in der das ziel schaden erleidet
        //gegner bet�uben
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
    #endregion

    #region Damage & Death
    public void GetDamaged(float damage)
    {
        if (heragzonSO.healthAmount > 0.0f)
        {
            heragzonSO.healthAmount -= damage;
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        //play death animation
        //either destroy game object after some time
        //or make it invisible and after death timer just make it visible in spawn
    }

    private void ResetStats()
    {
        heragzonSO.healthAmount = standardHealthAmount;
        heragzonSO.chardAmount = standardChardAmount;
    }

    public void Respawn()
    {
        ResetStats();
        //position at spawn point
    }

    #endregion


    private void OnDrawGizmos()
    {
        //visual for autoattack range
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, heragzonSO.autoAttackRange);

        //visual for Ability 1 range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, heragzonSO.ability1Range);
    }
}
