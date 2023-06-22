
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    //Turrets können eingenommen werden
    //Sie starten mit 0 Punkten und in einem neutralen Zustand
    //Der Turm kann eingenommen werden, indem er angegriffen wird 
    //Der jenige, der dem Turm von 200 punkten den meisten Schaden macht, nimmt den Turm ein

    [SerializeField] private TurretBaseClass turretSO;
    private FocusState focusState;

    private LayerMask layerAttackable;
    [SerializeField] private List<Collider> _targetsInRange = new List<Collider>();
    [SerializeField] private List<string> _targetTags = new List<string>();


    private bool minionInRange;
    private bool warlordInRange; 

    private void Awake()
    {
        focusState = FocusState.neutral;
    }

    private void Update()
    {
        //if first object that comes in range is minion: focusState = neutral
        //if firt object is champion: focusState = alerted
        //if minions are there and champ attacks champ: trigger event for switching focusState to alerted as long as chsmp is in range

        CheckForRange(turretSO.turretAttackRange);

    }

    private void CheckForRange(float range)
    {
        _targetsInRange = Physics.OverlapSphere(transform.position, range, layerAttackable).Where((n) => _targetTags.Contains((string)n.tag)).ToList();
      

        //Check if anything attackable is in range 
        if (_targetsInRange.Count > 0)
        {

            Debug.Log("Found something");
            CheckForTarget(_targetsInRange);
           

        }

    }

    private void CheckForTarget(List<Collider> targetsInRange)
    {

        var currentTarget = targetsInRange.First();

        //Check whatever comes first in range

        if (currentTarget.TryGetComponent(out IDamagable d))
        {
            switch (currentTarget.tag)
            {
                case "HostileMinion":
                    //if minion enters range first, attack it
                    d.GetDamagedByTurret(turretSO.turretDamage, turretSO.turretSpeed);
                    break;
                //case "Warlord":
                //    d.GetDamagedByTurret(turretSO.turretDamage, turretSO.turretSpeed);
                //    break;
            }

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, turretSO.turretAttackRange);
    }



}
