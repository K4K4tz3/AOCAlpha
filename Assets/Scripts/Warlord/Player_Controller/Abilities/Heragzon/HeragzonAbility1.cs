using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeragzonAbility1 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass heragzonSO;
    [SerializeField] private Collider parentCollider;

    private void OnTriggerStay(Collider other)
    {

        //damage wird jz jeden frame gemacht, �ndern auf pro sekunde 
        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IDamagable d))
        {
            switch (tag)
            {
                case "Building":
                    d.GetDamaged(heragzonSO.ability1DmgBuilding, parentCollider);
                    Debug.Log("ability 1 trigger stay HERAGZON");
                    break;
                case "Warlord":
                    d.GetDamaged(heragzonSO.ability1DmgWarlord, parentCollider);
                    if (other.gameObject.TryGetComponent(out IStunnable w))
                    {
                        w.GetStunned(heragzonSO.ability1Duration);
                    }
                    Debug.Log("ability 1 trigger stay HERAGZON");
                    break;
                case "HostileMinion":
                    d.GetDamaged(heragzonSO.ability1DmgMinion, parentCollider);
                    if (other.gameObject.TryGetComponent(out IStunnable m))
                    {
                        m.GetStunned(heragzonSO.ability1Duration);
                    }
                    Debug.Log("ability 1 trigger stay HERAGZON");
                    break;
            }
        }
    }

}