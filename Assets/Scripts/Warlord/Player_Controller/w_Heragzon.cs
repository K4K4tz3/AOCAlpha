using UnityEngine;

public class w_Heragzon : MonoBehaviour, IDamagable
{
    private int layerAttackable;
    private Camera mainCamera;

    //Scriptable Object for all necessary information
    [SerializeField] private WarlordBaseClass heragzonSO;
    private float standardHealthAmount;
    private float standardChardAmount;


    private void Awake()
    {
        mainCamera = Camera.main;
        layerAttackable = LayerMask.NameToLayer("Attackable");

        standardHealthAmount = heragzonSO.healthAmount;
        standardChardAmount = heragzonSO.chardAmount;  
    }

    //On... Methods are for PlayerInput Component
    //(methods send unity messages when player triggered button)

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



    public void OnAbility1()
    {
        Debug.Log("Ability1");
       //flächendamage V-Form --> instantiate prefab V (trigger area)
       //gegner betäuben
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


    private void OnDrawGizmos()
    {
        //visual for autoattack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, heragzonSO.autoAttackRange);
    }
}
