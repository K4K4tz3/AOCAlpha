using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    #endregion

    #region Damage Collider
    private GameObject damageAreaAbility1;
    private GameObject damageAreaAbility2;
    private GameObject damageAreaAbility3;
    #endregion

    private float standardHealthAmount;
    private float standardChardAmount;

    private bool qPossible;
    private bool qAvailable = true;
    private bool wAvailable = true;
    private bool eAvailable = true;



    //On... Methods are for PlayerInput Component
    //(methods send unity messages when player triggered button)

    //To Do:

    //3. Check damage amount if it is per second or not 


    private void Awake()
    {
        mainCamera = Camera.main;
        standardHealthAmount = heragzonSO.healthAmount;
        standardChardAmount = heragzonSO.chardAmount;

        // Area for the Damage
        damageAreaAbility1 = this.gameObject.transform.GetChild(1).gameObject;
        damageAreaAbility2 = this.gameObject.transform.GetChild(2).gameObject;
        damageAreaAbility3 = this.gameObject.transform.GetChild(3).gameObject;

    }
    private void Update()
    {

        if (CheckForAbilityRange(heragzonSO.ability1Range, new Vector3(0, 0, 1.5f)))
        {
            qPossible = true;
        }
        else
        {
            qPossible = false;
        }
    }
    private bool CheckForAbilityRange(float range, Vector3 position)
    {
        //check if something attackable is in range
        //transform.position + this vector so that the sphere not inside and behind the warlord
        _targetsInRange = Physics.OverlapSphere(transform.position + position, range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();

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

    #region AutoAttacks
    public void OnAutoAttack()
    {
        //Can only attack something "Attackable"
        //if player rightklicks on something with the layer "attackable", do aa  

        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, layerAttackable))
        {
            //looking at target 
            transform.LookAt(hit.point);

            if (CheckForAbilityRange(heragzonSO.autoAttackRange, Vector3.zero))
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

    #region Coroutines
    IEnumerator Ability1Duration()
    {
        damageAreaAbility1.SetActive(true);
        yield return new WaitForSeconds(heragzonSO.ability1Duration);
        damageAreaAbility1.SetActive(false);

        yield return null;
    }

    IEnumerator Ability1Cooldown()
    {
        qAvailable = false;
        yield return new WaitForSeconds(heragzonSO.ability1Cooldown);
        qAvailable = true;
    }

    IEnumerator Ability2Duration()
    {
        damageAreaAbility2.SetActive(true);
        yield return new WaitForSeconds(heragzonSO.ability2Duration);
        damageAreaAbility2.SetActive(false);
        yield return null;
    }

    IEnumerator Ability2Cooldown()
    {
        wAvailable = false;
        yield return new WaitForSeconds(heragzonSO.ability2Cooldown);
        wAvailable = true;
    }

    IEnumerator Ability3Duration()
    {
        damageAreaAbility3.SetActive(true);
        yield return new WaitForSeconds(heragzonSO.ability3Duration);
        damageAreaAbility3.SetActive(false);
        yield return null;
    }

    IEnumerator Ability3Cooldown()
    {
        eAvailable = false;
        yield return new WaitForSeconds(heragzonSO.ability3Cooldown);
        eAvailable = true;
    }
    #endregion

    #region Abilities
    public void OnAbility1()
    {
        //if q is pressed, first check if there is sonething attackable in range
        //because heragzons q can only be used if a target is in range

        if (qPossible && qAvailable)
        {
            #region look at target
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, layerAttackable))
            {
                //looking at target 
                transform.LookAt(hit.point);
            }
            #endregion

            //reduce Chards
            heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;

            Debug.Log("Q triggered");

            //set DMG Area active           
            //nach nem timer wird wieder false gesetzt
            StartCoroutine(Ability1Duration());
            StartCoroutine(Ability1Cooldown());

            #region Previous solution
            //everything that's attackable and inside this area gets damaged
            //OverlapBox = Damage Area
            //var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 0f), ability1Range, transform.rotation, layerAttackable);

            ////var targets = Physics.OverlapSphere(transform.localPosition + new Vector3(0, 0, 1.5f), heragzonSO.ability1Range / 2, layerAttackable);

            //if (targets.Length > 0)
            //{
            //    //reduce chards if there is something attackable in range
            //    //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;
            //    Debug.Log("doin q damage yeah");

            //    foreach (Collider c in targets)
            //    {
            //        //check if the targets have IDamagable implemented
            //        if (c.gameObject.TryGetComponent(out IDamagable d))
            //        {
            //            //what did the warlord hit? -> different damage amount
            //            //targets are stunned if hit (only warlords and minions)
            //            var tag = c.tag;

            //            switch (tag)
            //            {
            //                case "Building":
            //                    d.GetDamaged(heragzonSO.ability1DmgBuilding);
            //                    Debug.Log("Ability1 Building");
            //                    break;
            //                case "Warlord":
            //                    d.GetDamaged(heragzonSO.ability1DmgWarlord);
            //                    if (c.gameObject.TryGetComponent(out IStunnable w))
            //                    {
            //                        w.GetStunned(heragzonSO.ability1Duration);
            //                    }
            //                    Debug.Log("Ability1 Warlord");
            //                    break;
            //                case "Minion":
            //                    d.GetDamaged(heragzonSO.ability1DmgMinion);
            //                    if (c.gameObject.TryGetComponent(out IStunnable m))
            //                    {
            //                        m.GetStunned(heragzonSO.ability1Duration);
            //                    }
            //                    Debug.Log("Ability1 Minion");
            //                    break;
            //            }
            //        }
            //    }
            //}
            #endregion
        }
    }
    public void OnAbility2()
    {
        //reduce chards 
        //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;
        if (wAvailable)
        {
            //reduce Chards
            heragzonSO.chardAmount -= heragzonSO.ability2ChardCost;

            StartCoroutine(Ability2Duration());
            StartCoroutine(Ability2Cooldown());

            Debug.Log("Ability2 triggered");
        }

        #region Previous Solution
        //Check for targets in Damage Area
        //var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 3), ability2Range, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z), layerAttackable);

        ////Damage per second !! 
        //if (targets.Length > 0)
        //{
        //    //if there are targets 
        //    foreach (Collider c in targets)
        //    {
        //        //check if the targets have IDamagable implemented
        //        if (c.gameObject.TryGetComponent(out IDamagable d))
        //        {
        //            //what did the warlord hit? -> different damage amount

        //            var tag = c.tag;

        //            switch (tag)
        //            {
        //                case "Building":
        //                    d.GetDamaged(heragzonSO.ability2DmgBuilding);
        //                    Debug.Log("Ability2 Building");
        //                    break;
        //                case "Warlord":
        //                    d.GetDamaged(heragzonSO.ability2DmgWarlord);
        //                    Debug.Log("Ability2 Warlord");
        //                    break;
        //                case "Minion":
        //                    d.GetDamaged(heragzonSO.ability2DmgMinion);
        //                    Debug.Log("Ability2 Minion");
        //                    break;
        //            }
        //        }
        //    }

        //}
        #endregion

    }
    public void OnAbility3()
    {     
        if (eAvailable)
        {
            //reduce Chards
            heragzonSO.chardAmount -= heragzonSO.ability3ChardCost;

            StartCoroutine(Ability3Duration());
            StartCoroutine(Ability3Cooldown());
        }

        #region Previous Solution
        //Check for targets in Range
        //var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 3), ability3Range, Quaternion.identity, layerAttackable);

        ////Damage per second !! 
        //if (targets.Length > 0)
        //{
        //    //if there are targets 
        //    foreach (Collider c in targets)
        //    {
        //        //check if the targets have IDamagable implemented
        //        if (c.gameObject.TryGetComponent(out IDamagable d))
        //        {
        //            //what did the warlord hit? -> different damage amount

        //            var tag = c.tag;

        //            switch (tag)
        //            {
        //                case "Building":
        //                    d.GetDamaged(heragzonSO.ability3DmgBuilding);
        //                    Debug.Log("Ability3 Building");
        //                    break;
        //                case "Warlord":
        //                    d.GetDamaged(heragzonSO.ability3DmgWarlord);
        //                    Debug.Log("Ability3 Warlord");
        //                    break;
        //                case "Minion":
        //                    d.GetDamaged(heragzonSO.ability3DmgMinion);
        //                    Debug.Log("Ability3 Minion");
        //                    break;
        //            }
        //        }
        //    }

        //}
        #endregion
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


}
