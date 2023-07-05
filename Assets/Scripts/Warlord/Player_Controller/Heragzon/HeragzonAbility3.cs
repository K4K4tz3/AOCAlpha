using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeragzonAbility3 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass heragzonSO;
    [SerializeField] private Collider parentCollider;

    private void OnTriggerStay(Collider other)
    {

        //damage wird jz jeden frame gemacht, ändern auf pro sekunde 
        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IDamagable d))
        {
            switch (tag)
            {
                case "Building":
                    d.GetDamaged(heragzonSO.ability3DmgBuilding, parentCollider);
                    Debug.Log("ability 3 trigger stay HERAGZON");
                    break;
                case "Warlord":
                    d.GetDamaged(heragzonSO.ability3DmgWarlord, parentCollider);
                    Debug.Log("ability 3 trigger stay HERAGZON");
                    break;
                case "HostileMinion":
                    d.GetDamaged(heragzonSO.ability3DmgMinion, parentCollider);
                    Debug.Log("ability 3 trigger stay HERAGZON");
                    break;
            }
        }
    }
}
