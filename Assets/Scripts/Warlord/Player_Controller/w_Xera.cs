using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class w_Xera : MonoBehaviour, IDamagable, IStunnable, IControllable, IPushable
{
    #region General
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass xeraSO;
    private Renderer warlordRenderer;
    private NavMeshAgent navMeshAgent;

    private int layerAttackable;
    private Camera mainCamera;

    private Collider warlordCollider;
    private PlayerController playerController;

    #endregion

    #region Floats for Respawn
    private float standardHealthAmount;
    private float standardChardAmount;
    private float standardAutoAttackDamage;
    #endregion

    #region Range Check
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();   
    #endregion

    #region Bools for Abilities
    private bool inputPossible = true;
    private bool qAvailable = true;
    private bool wAvailable = true;
    private bool eAvailable = true;
    private bool eSmokeActive;
    public bool eExplosion = false;
    private bool controlled = false;
    #endregion

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
        navMeshAgent = GetComponent<NavMeshAgent>();

        standardHealthAmount = xeraSO.healthAmount;
        standardChardAmount = xeraSO.chardAmount;
        standardAutoAttackDamage = xeraSO.autoAttackDamage;

        areaAbility1 = this.gameObject.transform.GetChild(1).gameObject;
        areaAbility2 = this.gameObject.transform.GetChild(2).gameObject;
        areaAbility3 = this.gameObject.transform.GetChild(3).gameObject;

        warlordCollider = GetComponent<Collider>();
        playerController = GetComponent<PlayerController>();
    

    }



    private GameObject CheckForValidTarget(float range, Vector3 position)
    {
        //check if something attackable is in range
        //transform.position + this vector so that the sphere is not inside or behind the warlord
        _targetsInRange = Physics.OverlapSphere(transform.position + position, range, layerAttackable).Where((n) => playerController._targetTags.Contains((string)n.tag)).ToList();

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
            d.GetDamaged(xeraSO.autoAttackDamage, warlordCollider);
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

    IEnumerator Controlled(float duration)
    {
        inputPossible = false;
        yield return new WaitForSeconds(duration);
        inputPossible = true;
    }

    IEnumerator AttackAllies(float duration)
    {
        controlled = true;
        yield return new WaitForSeconds(duration);
        controlled = false;
    }

    IEnumerator Stunned(float duration)
    {
        inputPossible = false;
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(duration);
        inputPossible = true;
        navMeshAgent.speed = xeraSO.movementSpeed;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(xeraSO.respawnTimer);
        warlordRenderer.enabled = true;
        inputPossible = true;
        navMeshAgent.speed = xeraSO.movementSpeed;
    }
    #endregion

    #region Abilities
    public void OnAbility1()
    {
        //start Cooldown
        if (qAvailable && inputPossible)
        {
            StartCoroutine(Ability1Cooldown());
            StartCoroutine(Ability1Duration());
            Debug.Log("Ability1");

        }
    }

    public void OnAbility2()
    {
        if (wAvailable && inputPossible)
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
        if (eAvailable && inputPossible)
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
    private void SacrificeMinion(GameObject minion)
    {
        //destroy minion
        if (minion.gameObject.TryGetComponent(out IDamagable d))
        {
            d.Die();
        }
    }
    public void GetDamaged(float damage, Collider damageDealer)
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

    public void GetStunned(float duration)
    {
        StartCoroutine(Stunned(duration));
    }
    public void GetControlled(float duration)
    {
        //stop gettin input
        StartCoroutine(Controlled(duration));

        //start attacking nearest target

        //find target with OverlapSphere
        var targets = Physics.OverlapSphere(transform.localPosition, xeraSO.autoAttackRange, layerAttackable);
        if (targets.Length > 0)
        {
            //look at target
            transform.LookAt(targets[0].transform);

            //move to target
            navMeshAgent.destination = targets[0].transform.position;

            StartCoroutine(AttackAllies(duration));

            //attack target
            while (controlled)
            {
                DoAutoAttack(targets[0].gameObject);
            }
        }
    }
    public void GetPushedAway(float duration, Vector2 direction)
    {
        //move warlord in the given direction
        transform.Translate(direction.x, 0, direction.y);
    }
    public void GetPulledOver(float duration, Vector2 direction)
    {
        //move warlord in the given direction
        transform.Translate(direction.x, 0, direction.y);
    }

    public void Die()
    {
        navMeshAgent.speed = 0;
        warlordRenderer.enabled = false;
        inputPossible = false;
        transform.position = xeraSO.spawnPosition;
        RespawnWarlord();
    }
    private void ResetStats()
    {
        xeraSO.healthAmount = standardHealthAmount;
        xeraSO.chardAmount = standardChardAmount;
    }
    public void RespawnWarlord()
    {
        ResetStats();
        StartCoroutine(Respawn());
    }


    #endregion




}
