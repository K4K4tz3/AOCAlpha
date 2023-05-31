using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class w_Lyrien : MonoBehaviour, IDamagable, IStunnable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass lyrienSO;

    private int layerAttackable;
    private Camera mainCamera;

    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();

    private float standardHealthAmount;
    private float standardChardAmount;

    private bool qPossible;
    private bool qAvailable = true;
    private bool wAvailable = true;
    private bool eAvailable = true;

    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");

        standardHealthAmount = lyrienSO.healthAmount;
        standardChardAmount = lyrienSO.chardAmount;
    }

    private void Update()
    {
        if (CheckForAbilityRange(lyrienSO.ability1Range, transform.position))
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

    IEnumerator Ability2Cooldown()
    {
        wAvailable = false;
        yield return new WaitForSeconds(lyrienSO.ability2Cooldown);
        wAvailable = true;
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
            StartCoroutine(Ability1Cooldown());
            //stop movement
            //can move enemy -> "gegner angreifen lassen", also vllt eher: Gegner greifen deren nächsten gegner an (vgl. Renata ult)
            //if q pressed while in use, stop it

        }

        Debug.Log("Ability1");

    }

    public void OnAbility2()
    {
        if (wAvailable)
        {
            StartCoroutine(Ability2Cooldown());
        }
        //Stößt gegner weg oder zieht sie an sich ran
        //-> Bereich wird markiert, danach wird geprüft wo die maus ist
        //beim erneuten drücken wird entweder weggestoßen oder rangezogen

        Debug.Log("Ability2");

    }

    public void OnAbility3()
    {
        if (eAvailable)
        {
            StartCoroutine (Ability3Cooldown());
        }
        //Big jump to target (target = mouse position)
        //cancelling all enemy attacks
        //unverwundbar


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

    #endregion

    //ability 1: kein damage- kann gegner steuern für 5sec (soldat)
    //ability 2: rnge bei wwarlords x0.5, gegner fliehen
    //ability 3: jump 4m reichweite
}
