using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class w_Xera : MonoBehaviour, IDamagable, IStunnable, IControllable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass xeraSO;
    private Renderer warlordRenderer;

    private int layerAttackable;
    private Camera mainCamera;

    private float standardHealthAmount;
    private float standardChardAmount;
    private float standardAutoAttackDamage;

    #region Range Check
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();
    #endregion


    private bool qAvailable = true;
    private bool wAvailable = true;
    private bool eAvailable = true;
    private bool eSmokeActive;
    public bool eExplosion = false;

    #region Damage Collider
    private GameObject areaAbility1;
    private GameObject areaAbility2;
    private GameObject areaAbility3;
    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");
        warlordRenderer = GetComponent<Renderer>();

        standardHealthAmount = xeraSO.healthAmount;
        standardChardAmount = xeraSO.chardAmount;
        standardAutoAttackDamage = xeraSO.autoAttackDamage;

        areaAbility1 = this.gameObject.transform.GetChild(1).gameObject;
        areaAbility2 = this.gameObject.transform.GetChild(2).gameObject;
        areaAbility3 = this.gameObject.transform.GetChild(3).gameObject;
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
                if (targetTag.tag == "AllyMinion")
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

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == layerAttackable)
            {
                if (Vector3.Distance(transform.position, hit.point) <= xeraSO.autoAttackRange)
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
            d.GetDamaged(xeraSO.autoAttackDamage);
        }

        Debug.Log("AutoAttack");
    }
    #endregion


    #region Coroutines
    IEnumerator Ability1Cooldown()
    {
        qAvailable = false;
        yield return new WaitForSeconds(xeraSO.ability1Cooldown);
        qAvailable = true;
    }

    IEnumerator Ability1Duration()
    {
        areaAbility1.SetActive(true);
        yield return new WaitForSeconds(xeraSO.ability1Duration);
        areaAbility1.SetActive(false);

        yield return null;
    }

    IEnumerator Ability2Cooldown()
    {
        wAvailable = false;
        yield return new WaitForSeconds(xeraSO.ability2Cooldown);
        wAvailable = true;

    }

    IEnumerator Ability2Buff()
    {
        xeraSO.autoAttackDamage = 12;
        yield return new WaitForSeconds(xeraSO.ability2Duration);
        xeraSO.autoAttackDamage = standardAutoAttackDamage;
    }

    IEnumerator Ability3Cooldown()
    {
        eAvailable = false;
        yield return new WaitForSeconds(xeraSO.ability3Cooldown);
        eAvailable = true;
    }

    IEnumerator Ability3Duration()
    {
        areaAbility3.SetActive(true);
        eSmokeActive = true;
        //Make Xera invisible
        warlordRenderer.enabled = false;

        yield return new WaitForSeconds(xeraSO.ability3Duration);
        areaAbility3.SetActive(false);
        eSmokeActive = false;

        yield return null;
    }
    #endregion


    #region Abilities
    public void OnAbility1()
    {
        //start Cooldown
        if (qAvailable)
        {
            StartCoroutine(Ability1Cooldown());
            StartCoroutine(Ability1Duration());
            Debug.Log("Ability1");

        }
    }

    public void OnAbility2()
    {
        if (wAvailable)
        {
            StartCoroutine(Ability2Cooldown());

            //check if minion is in range
            var target = CheckForValidTarget(xeraSO.ability2Range, transform.position);

            if (target != null)
            {
                //select minion
                RaycastHit hit;
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, xeraSO.ability3Range))
                {
                    //ONLY ALLY MINIONS
                    if (hit.transform.gameObject.tag == "AllyMinion")
                    {
                        //kill selected minion
                        SacrificeMinion(hit.transform.gameObject);

                        //start buff timer
                        StartCoroutine(Ability2Buff());
                    }
                }
            }
            Debug.Log("Ability2");
        }

    }

    public void OnAbility3()
    {
        if (eAvailable)
        {
            StartCoroutine(Ability3Cooldown());

            //Activate Smoke area
            StartCoroutine(Ability3Duration());


            //if e pressed again while smoke still active -> Deal damage
            if (Keyboard.current.eKey.wasPressedThisFrame && eSmokeActive)
            {
                warlordRenderer.enabled = true;
                eSmokeActive = false;
                eExplosion = true;
            }
            Debug.Log("Ability3");

        }

    }

    #endregion

    #region Damage & Death
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
    public void GetStunned(float duration)
    {

    }

    private void ResetStats()
    {
        xeraSO.healthAmount = standardHealthAmount;
        xeraSO.chardAmount = standardChardAmount;
    }

    public void Respawn()
    {
        ResetStats();
        //position at spawn point
    }

    public void GetControlled(float duration)
    {
        //stop gettin input
        //start attacking target near 
    }

    private void SacrificeMinion(GameObject minion)
    {
        //destroy minion
    }
    #endregion




}
