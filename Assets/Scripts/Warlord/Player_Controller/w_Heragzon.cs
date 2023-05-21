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
    private Vector3 ability1Range;
    private Vector3 ability2Range;
    private Vector3 ability3Range;
    #endregion

    #region VFX/Damage Areas
    private GameObject damageAreaAbility1;
    private GameObject damageAreaAbility2;
    private GameObject damageAreaAbility3;
    #endregion

    private float standardHealthAmount;
    private float standardChardAmount;

    public Quaternion gameObjectRotation;
    public Vector3 rot;

    //On... Methods are for PlayerInput Component
    //(methods send unity messages when player triggered button)
    //To Do:
    //1. Cooldown timer for abilities ! 
    //2. Reduce Chard amount -> What Order?
    //3. Check damage amount if it is per second or not 
    //4. Areas set active false setzen

    private bool inRangeforA1;


    private void Awake()
    {
        mainCamera = Camera.main;
        standardHealthAmount = heragzonSO.healthAmount;
        standardChardAmount = heragzonSO.chardAmount;

        //Range for the OverlapBox Check -> Needs to be half of the extends
        ability1Range = new Vector3(heragzonSO.ability1Range / 2, heragzonSO.ability1Range / 2, heragzonSO.ability1Range / 2);
        ability2Range = new Vector3(heragzonSO.ability2Range / 4, heragzonSO.ability2Range / 4, heragzonSO.ability2Range / 2);
        ability3Range = new Vector3(heragzonSO.ability2Range / 4, heragzonSO.ability2Range / 4, heragzonSO.ability2Range / 2);

        //Possible Area for the Damage/Sword VFX Ability 1
        damageAreaAbility1 = this.gameObject.transform.GetChild(1).gameObject;
        damageAreaAbility2 = this.gameObject.transform.GetChild(2).gameObject;
        damageAreaAbility3 = this.gameObject.transform.GetChild(3).gameObject;

    }
    private void Update()
    {
        gameObjectRotation = transform.rotation;
        //rotation of game object
        rot = new Vector3(0, gameObjectRotation.y, 0);

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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = this.transform.localToWorldMatrix;

        //visual for Ability 1 range
        Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, 0, 2), new Vector3(heragzonSO.ability1Range, heragzonSO.ability1Range, heragzonSO.ability1Range));
        Gizmos.DrawWireSphere(Vector3.zero + new Vector3(0, 0, 1.5f), heragzonSO.ability1Range / 2);

        //Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, 0, 3), new Vector3(heragzonSO.ability2Range/3, heragzonSO.ability2Range, heragzonSO.ability2Range));
        //Gizmos.DrawWireSphere(Vector3.zero + new Vector3(0,0,3), heragzonSO.ability2Range/2);


    }

    #region Abilities
    public void OnAbility1()
    {
        //if q is pressed, first check if there is sonething attackable in range
        //because heragzons q can only be used if a target is in range

        if (CheckForAbilityRange(heragzonSO.ability1Range, new Vector3(0, 0, 1.5f)))
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, layerAttackable))
            {
                //looking at target 
                transform.LookAt(hit.point);
            }
            Debug.Log("Q triggered wohoo");

            //set VFX active
            damageAreaAbility1.SetActive(true);

            //everything that's attackable and inside this area gets damaged
            //OverlapBox = Damage Area
            //var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 2), ability1Range, Quaternion.LookRotation(rot), layerAttackable);

            //Changed OverlapBox to Sphere bc Box did not move with the player and so there was no regular dmg, only sometimes
            var targets = Physics.OverlapSphere(transform.position + new Vector3(0, 0, 2), heragzonSO.ability1Range/2,layerAttackable);

            if (targets.Length > 0)
            {
                //reduce chards if there is something attackable in range
                //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;
                Debug.Log("doin q damage yeah");

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
            inRangeforA1 = true;

        }
    }


    public void OnAbility2()
    {
        //reduce chards 
        //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;

        damageAreaAbility2.SetActive(true);

        Debug.Log("Ability2 triggered");

        //Check for targets in Damage Area
        var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 3), ability2Range, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z), layerAttackable);

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

        //reduce chards 
        //heragzonSO.chardAmount -= heragzonSO.ability1ChardCost;

        damageAreaAbility3.SetActive(true);

        //Check for targets in Range
        var targets = Physics.OverlapBox(transform.position + new Vector3(0, 0, 3), ability3Range, Quaternion.identity, layerAttackable);

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
                            d.GetDamaged(heragzonSO.ability3DmgBuilding);
                            Debug.Log("Ability3 Building");
                            break;
                        case "Warlord":
                            d.GetDamaged(heragzonSO.ability3DmgWarlord);
                            Debug.Log("Ability3 Warlord");
                            break;
                        case "Minion":
                            d.GetDamaged(heragzonSO.ability3DmgMinion);
                            Debug.Log("Ability3 Minion");
                            break;
                    }
                }
            }

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


}
