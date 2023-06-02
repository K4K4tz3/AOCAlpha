using UnityEngine;

public class w_Xera : MonoBehaviour, IDamagable, IStunnable, IControllable
{
    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass xeraSO;

    private int layerAttackable;
    private Camera mainCamera;

    private float standardHealthAmount;
    private float standardChardAmount;

    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");

        standardHealthAmount = xeraSO.healthAmount;
        standardChardAmount = xeraSO.chardAmount;
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

    #region Abilities
    public void OnAbility1()
    {
        Debug.Log("Ability1");
       
    }

    public void OnAbility2()
    {
        Debug.Log("Ability2");
        
    }

    public void OnAbility3()
    {
        Debug.Log("Ability3");
        
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

    public void GetControlled()
    {
        //stop gettin input
        //start attacking target near 
    }

    #endregion

    //ability 2 buffs autoattack and gives 12 more dmg for 6 sec
    //tötet das ziel (ziel= das opfer des eigenen teams)

    //ability 3 macht rauch (0dmg) und nach 4 sec eine explosion (25dmg/10dmg)
    //range 1m = range der explosion
    //unsichtbar im rauch
    //keine anderen abilities möglich, nur aa
    //erneutes drücken  der fähigkeit = explosion


}
