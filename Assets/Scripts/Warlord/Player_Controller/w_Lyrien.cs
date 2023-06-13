using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class w_Lyrien : MonoBehaviour, IDamagable, IStunnable, IControllable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass lyrienSO;

    private int layerAttackable;
    private Camera mainCamera;


    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();

    #region Damage Collider
    private GameObject AreaAbility2;
    #endregion


    private float standardHealthAmount;
    private float standardChardAmount;

    private bool qPossible;
    private bool qControllingPossible = true;
    private bool qAvailable = true;
    private bool wAvailable = true;
    private bool wPressedOnce;
    public bool wPressedTwice;
    private bool eAvailable = true;


    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");

        standardHealthAmount = lyrienSO.healthAmount;
        standardChardAmount = lyrienSO.chardAmount;

        // Area for the Damage
        AreaAbility2 = this.gameObject.transform.GetChild(2).gameObject;

    }

    private void Update()
    {
        if (CheckForAbilityRange(lyrienSO.ability1Range, transform.position))
        {
            //check if target is warlord 
            qPossible = true;
        }
        else
        {
            qPossible = false;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame && wPressedOnce)
        {
            wPressedTwice = true;
        }
    }


    private bool CheckForAbilityRange(float range, Vector3 position)
    {
        //check if something attackable is in range
        //transform.position + this vector so that the sphere not inside and behind the warlord
        _targetsInRange = Physics.OverlapSphere(transform.position + position, range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();

        if (_targetsInRange.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameObject CheckForValidTarget(float range, Vector3 position)
    {
        //check if something attackable is in range
        //transform.position + this vector so that the sphere is not inside or behind the warlord
        _targetsInRange = Physics.OverlapSphere(transform.position + position, range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();

        if (_targetsInRange.Count > 0)
        {
            foreach (var targetTag in _targetsInRange)
            {
                if (targetTag.tag == "Warlord")
                {
                    return targetTag.gameObject;

                }
                else
                {
                    return null;
                }
            }

            return null;

        }
        else
        {
            return null;
        }
    }

    #region AutoAttacks
    public void OnAutoAttack()
    {
        //Can only attack something "Attackable"
        //if player rightklicks on something with the layer "attackable", do aa
        //range depends on the warlord

        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, layerAttackable))
        {
            //looking at target 
            transform.LookAt(hit.point);

            if (CheckForAbilityRange(lyrienSO.autoAttackRange, Vector3.zero))
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
            d.GetDamaged(lyrienSO.autoAttackDamage);
        }

        Debug.Log("AutoAttack");
    }

    #endregion

    #region Coroutines
    IEnumerator Ability1Cooldown()
    {
        qAvailable = false;
        yield return new WaitForSeconds(lyrienSO.ability1Cooldown);
        qAvailable = true;
    }

    IEnumerator StopMovementAbility1()
    {
        this.GetComponent<PlayerController>().canGetInput = false;
        yield return new WaitForSeconds(lyrienSO.ability1Duration);
        this.GetComponent<PlayerController>().canGetInput = true;
    }

    IEnumerator GetControlledByAbility1()
    {
        qControllingPossible = true;
        yield return new WaitForSeconds(lyrienSO.ability1Duration);
        qControllingPossible = false;
    }

    IEnumerator Ability2Cooldown()
    {
        wAvailable = false;
        yield return new WaitForSeconds(lyrienSO.ability2Cooldown);
        wAvailable = true;
        wPressedOnce = false;
    }

    IEnumerator Ability2Duration()
    {
        AreaAbility2.SetActive(true);
        yield return new WaitForSeconds(lyrienSO.ability2Duration);
        AreaAbility2.SetActive(false);

        yield return null;
    }

    IEnumerator Ability3Cooldown()
    {
        eAvailable = false;
        yield return new WaitForSeconds(lyrienSO.ability3Cooldown);
        eAvailable = true;
    }

    #endregion

    #region Abilities

    public void OnAbility1()
    {
        //Range check! 
        if (qAvailable && qPossible)
        {
            //start cd
            StartCoroutine(Ability1Cooldown());

            //stop movement
            StartCoroutine(StopMovementAbility1());

            //choose enemy that is going to be controlled
            //vorsicht: wenn null ist, kann fehler geben -> checken mit mehreren warlords in der szene 
            if (CheckForValidTarget(lyrienSO.ability1Range, transform.position).gameObject.TryGetComponent(out IControllable c))
            {
                StartCoroutine(GetControlledByAbility1());

                if (qControllingPossible)
                {
                    c.GetControlled();

                }

            }

            //if q pressed while in use, stop it
            //TO DO 

        }

        Debug.Log("Ability1");

    }

    public void OnAbility2()
    {
        if (wAvailable)
        {

            wPressedOnce = true;


            StartCoroutine(Ability2Cooldown());

            //Area aktivieren
            StartCoroutine(Ability2Duration());


            //Check ob warlord oder minion -> Wird im collider gecheckt 

            //check ob w ein oder zweimal gedr�ckt wurde -> In Update 
            //w einmal = push
            //w zweimal = attract
            //Wird im Ability2 Script gehandled


        }





        //L�sst ziele weglaufen

        //bei nochmal dr�cken w�hrend der duration -> Ziele laufen zu lyrien hin


        //St��t gegner weg oder zieht sie an sich ran
        //-> Bereich wird markiert, danach wird gepr�ft wo die maus ist 
        //beim erneuten dr�cken wird entweder weggesto�en oder rangezogen

        Debug.Log("Ability2");

    }

    public void OnAbility3()
    {
        if (eAvailable)
        {
            StartCoroutine(Ability3Cooldown());
        }


        //Big jump to target (target = mouse position)
        //Get mouse position
        
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //"jump" to mouse Position if it's in range
        if (Physics.Raycast(ray, lyrienSO.ability3Range))
        {
            Vector3 jumpTarget = new Vector3(ray.origin.x, 0, ray.origin.z);
            //move gameobject 
            transform.Translate(jumpTarget);
        }

        //cancelling all enemy attacks
        //unverwundbar
        StopEnemyAbilities();


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

    public void GetStunned(float duration)
    { }

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

    public void GetControlled()
    {
        //stop gettin input
        //start attacking target near 
    }

    private void StopEnemyAbilities()
    {
        //be invincible 
    }

    #endregion

    //ability 1: kein damage- kann gegner steuern f�r 5sec (soldat)
    //ability 2: rnge bei wwarlords x0.5, gegner fliehen
    //ability 3: jump 4m reichweite
}