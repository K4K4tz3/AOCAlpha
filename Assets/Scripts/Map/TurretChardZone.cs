using System.Collections;
using UnityEngine;

public class TurretChardZone : MonoBehaviour
{
    [Tooltip("Turret SO can be used because it has the same regeneration stats")]
    [SerializeField] private TurretBaseClass turretSO;
    private TurretController controller;

    private void Awake()
    {
        controller = GetComponentInParent<TurretController>();
    }


    //if a affiliate Warlord enters the zone, his chards are regenerated 
    private void OnTriggerEnter(Collider other)
    {
        if (controller.team == Team.None)
            return;

        else
        {
            //if warlord && if it has IRegeneratable implemented
            //if turret is in the same team -> regenerate chards 
            if (other.gameObject.TryGetComponent(out PlayerController pc) && other.gameObject.TryGetComponent(out IRegeneratable reg))
            {
                if (pc.team == controller.team)
                {
                    //regenerate chards 
                    StartCoroutine(RegenerateChards(turretSO.chardRegenerationRate, reg));
                }
                else
                    return;
            }
            else
                return;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController pc) && other.gameObject.TryGetComponent(out IRegeneratable reg))
        {
            if (pc.team == controller.team)
            {
                StopCoroutine(RegenerateChards(turretSO.chardRegenerationRate, reg));
            }
            else
                return;
        }
        else
            return;

    }


    private IEnumerator RegenerateChards(float regenerationAmount, IRegeneratable reg)
    {
        reg.RegenerateChards(regenerationAmount);
        yield return new WaitForSeconds(1f);

    }
}
