
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour, IDamagable
{

    //Turrets können eingenommen werden
    //Sie starten mit 0 Punkten und in einem neutralen Zustand
    //Der Turm kann eingenommen werden, indem er angegriffen wird 
    //Der jenige, der dem Turm von 200 punkten den meisten Schaden macht, nimmt den Turm ein

    [SerializeField] private TurretBaseClass turretSO;
    private FocusState focusState;
    private AffiliateState affiliateState;

    [SerializeField] private LayerMask layerAttackable;
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();

    [SerializeField] private List<Collider> targetableMinions = new List<Collider>();


    private void Awake()
    {
        //Turrets start neutral
        focusState = FocusState.neutral;
        affiliateState = AffiliateState.neutral;
    }

    private void Update()
    {
        CheckForRange(turretSO.turretAttackRange);

    }


    #region Range Checks
    private void CheckForRange(float range)
    {
        _targetsInRange = Physics.OverlapSphere(transform.position, range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();


        //Check if anything attackable is in range 
        if (_targetsInRange.Count > 0)
        {
            Debug.Log("Found something");

            //make a list with all the minions entering trigger area
            foreach (var m in _targetsInRange)
            {
                //Add the minions to a list
                if (m.CompareTag("HostileMinion") && !targetableMinions.Contains(m))
                {
                    targetableMinions.Add(m);
                }
            }

            //attack minion if there are some and tower is in neutral state
            if (targetableMinions != null && focusState == FocusState.neutral)
            {
                FocusMinion();
            }
            //attack warlord even if there are minions when tower is alerted
            else if(targetableMinions != null && focusState == FocusState.alerted)
            {
                FocusWarlord();
            }
            //if warlord enters and there are no minions, focus warlord
            else if(targetableMinions == null)
            {
                //set tower state to alerted because there are no minions 
                focusState = FocusState.alerted;
                FocusWarlord();
            }
        }
        else
        {
            targetableMinions.Clear(); 
        }

    }

    private void FocusMinion()
    {
        Debug.Log("Attacking minion lul");
    }

    private void FocusWarlord()
    {
        Debug.Log("Attacking Warlord lul");
    }


    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, turretSO.turretAttackRange);
    }


    #region Damage & Death
    public void GetDamaged(float damage)
    {
        //if turret has 200 points and is neutral, check how many damage what team did 
        Debug.Log("Turret is getting damaged");
    }

    public void GetDamagedByTurret(float damage, float speed)
    {

    }

    public void Die()
    {
        //play destroy animation
    }

    #endregion
}
