using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class w_Heragzon : MonoBehaviour, IDamagable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass heragzonSO;

    //for range check
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();

    public LayerMask layerAttackable;
    private Camera mainCamera;

    private float standardHealthAmount;
    private float standardChardAmount;

    #region Bools for Range Check
    [SerializeField] private bool inRangeAA;
    [SerializeField] private bool inRange1;
    #endregion

    private GameObject damageAreaAbility1;
    private Vector3 ability1Range;

    private void Awake()
    {
        mainCamera = Camera.main;
        standardHealthAmount = heragzonSO.healthAmount;
        standardChardAmount = heragzonSO.chardAmount;

        //Possible Area for the Damage/Sword VFX Ability 1
        damageAreaAbility1 = this.gameObject.transform.GetChild(1).gameObject;

        //Range for the OverlapBox Check -> Needs to be half of the extends
        ability1Range = new Vector3(heragzonSO.ability1Range / 2, heragzonSO.ability1Range / 2, heragzonSO.ability1Range / 2);
    }

    //On... Methods are for PlayerInput Component
    //(methods send unity messages when player triggered button)

    #region AutoAttacks
    public void OnAutoAttack()
    {
        //Can only attack something "Attackable"
        //if player rightklicks on something with the layer "attackable", do aa  

        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, layerAttackable))
        {
            if (CheckForAbilityRange(heragzonSO.autoAttackRange, inRangeAA))
            {
                DoAutoAttack(hit.transform.gameObject);
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
        //if q is pressed and there are targets in range
        if (CheckForAbilityRange(heragzonSO.ability1Range, inRange1))
        {
            //set VFX active
            //damageAreaAbility1.SetActive(true);

            //everything that's attackable and inside this area gets damaged
            var targets = Physics.OverlapBox(transform.position, ability1Range, Quaternion.identity, layerAttackable);
            if (targets.Length > 0)
            {
                foreach (Collider c in targets)
                {
                    if (c.gameObject.TryGetComponent(out IDamagable d))
                    {
                        //what did the warlord hit? -> different damage amount
                        //check tags!
                        d.GetDamaged(heragzonSO.ability1DmgBuilding);
                        Debug.Log("Ability1");
                    }
                }
            }

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

    private bool CheckForAbilityRange(float range, bool inRange)
    {
        //check if something attackable is in range
        _targetsInRange = Physics.OverlapSphere(transform.position, range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();

        if (_targetsInRange.Count > 0)
        {
            foreach (Collider t in _targetsInRange)
            {
                Debug.Log(t.tag);

            }
            return inRange = true;

        }
        else
        {
            return inRange = false;
        }
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
        Gizmos.DrawWireCube(transform.position, new Vector3(heragzonSO.ability1Range, heragzonSO.ability1Range, heragzonSO.ability1Range));
    }
}
