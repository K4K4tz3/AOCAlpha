using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class w_Heragzon : MonoBehaviour, IDamagable, IStunnable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass heragzonSO;
    public LayerMask layerAttackable;
    private Camera mainCamera;

    #region Range Check
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();
    private Vector3 ability1Range;
    private Vector3 ability2Range;
    #endregion

    #region VFX/Damage Areas
    private GameObject damageAreaAbility1;
    private GameObject damageAreaAbility2;
    #endregion

    private float standardHealthAmount;
    private float standardChardAmount;


    private void Awake()
    {
        mainCamera = Camera.main;
        standardHealthAmount = heragzonSO.healthAmount;
        standardChardAmount = heragzonSO.chardAmount;

        //Range for the OverlapBox Check -> Needs to be half of the extends
        ability1Range = new Vector3(heragzonSO.ability1Range / 2, heragzonSO.ability1Range / 2, heragzonSO.ability1Range / 2);
        ability2Range = new Vector3(heragzonSO.ability2Range / 6, heragzonSO.ability2Range / 4, heragzonSO.ability2Range / 2);

        //Possible Area for the Damage/Sword VFX Ability 1
        damageAreaAbility1 = this.gameObject.transform.GetChild(1).gameObject;
        damageAreaAbility2 = this.gameObject.transform.GetChild(2).gameObject;

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
            if (CheckForAbilityRange(heragzonSO.autoAttackRange))
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

    //To Do:
    //1. Cooldown timer for abilities ! 
    //2. Reduce Chard amount -> What Order?
    //3. Check damage amount if it is per second or not 


    #region Abilities
    public void OnAbility1()
    {
        //if q is pressed and there are targets in range
        if (CheckForAbilityRange(heragzonSO.ability1Range))
        {
            //set VFX active
            damageAreaAbility1.SetActive(true);

            //everything that's attackable and inside this area gets damaged
            //OverlapBox = Damage Area
            var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 2), ability1Range, Quaternion.identity, layerAttackable);

            if (targets.Length > 0)
            {
                //reduce chards if there is something attackable in range
                //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;

                foreach (Collider c in targets)
                {
                    //check if the targets have IDamagable implemented
                    if (c.gameObject.TryGetComponent(out IDamagable d))
                    {
                        //what did the warlord hit? -> different damage amount
                        //targets are stunned if hit (only warlords and minions)
                        var tag = c.tag;

                        switch (tag)
                        {
                            case "Building":
                                d.GetDamaged(heragzonSO.ability1DmgBuilding);
                                Debug.Log("Ability1 Building");
                                break;
                            case "Warlord":
                                d.GetDamaged(heragzonSO.ability1DmgWarlord);
                                if (c.gameObject.TryGetComponent(out IStunnable w))
                                {
                                    w.GetStunned(heragzonSO.ability1Duration);
                                }
                                Debug.Log("Ability1 Warlord");
                                break;
                            case "Minion":
                                d.GetDamaged(heragzonSO.ability1DmgMinion);
                                if (c.gameObject.TryGetComponent(out IStunnable m))
                                {
                                    m.GetStunned(heragzonSO.ability1Duration);
                                }
                                Debug.Log("Ability1 Minion");
                                break;
                        }
                    }
                }
            }

           
        }
    }

    public void OnAbility2()
    {
        //reduce chards 
        //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;

        damageAreaAbility2.SetActive(true);

        //Check for targets in Range
        var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 3), ability2Range, Quaternion.identity, layerAttackable);

        //Damage per second !! 
        if (targets.Length > 0)
        {
            //if there are targets 
            foreach (Collider c in targets)
            {
                //check if the targets have IDamagable implemented
                if (c.gameObject.TryGetComponent(out IDamagable d))
                {
                    //what did the warlord hit? -> different damage amount

                    var tag = c.tag;

                    switch (tag)
                    {
                        case "Building":
                            d.GetDamaged(heragzonSO.ability2DmgBuilding);
                            Debug.Log("Ability2 Building");
                            break;
                        case "Warlord":
                            d.GetDamaged(heragzonSO.ability2DmgWarlord);
                            Debug.Log("Ability2 Warlord");
                            break;
                        case "Minion":
                            d.GetDamaged(heragzonSO.ability2DmgMinion);
                            Debug.Log("Ability2 Minion");
                            break;
                    }
                }
            }

        }

        
    }

    public void OnAbility3()
    {
        Debug.Log("Ability3");
        //doingAbility3 = true;
    }

    private bool CheckForAbilityRange(float range)
    {
        //check if something attackable is in range
        //transform.position + this vector so that the sphere is right in front of the warlord
        _targetsInRange = Physics.OverlapSphere(transform.position + new Vector3(0, 0, 0.5f), range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();

        if (_targetsInRange.Count > 0)
        {
            //foreach (Collider t in _targetsInRange)
            //{
            //    Debug.Log(t.tag);

            //}
            return true;

        }
        else
        {
            return false;
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

    public void GetStunned(float duration)
    {

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, 0.5f), heragzonSO.autoAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, 3), new Vector3(heragzonSO.ability2Range/4, heragzonSO.ability2Range/2, heragzonSO.ability2Range));

        //visual for Ability 1 range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, 2), new Vector3(heragzonSO.ability1Range, heragzonSO.ability1Range, heragzonSO.ability1Range));
    }
}
