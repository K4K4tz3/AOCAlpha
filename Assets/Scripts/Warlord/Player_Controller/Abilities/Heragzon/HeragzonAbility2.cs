using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeragzonAbility2 : MonoBehaviour
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
                    d.GetDamaged(heragzonSO.ability2DmgBuilding, parentCollider);
                    Debug.Log("ability 2 trigger stay HERAGZON");
                    break;
                case "Warlord":
                    d.GetDamaged(heragzonSO.ability2DmgWarlord, parentCollider);
                    Debug.Log("ability 2 trigger stay HERAGZON");
                    break;
                case "HostileMinion":
                    d.GetDamaged(heragzonSO.ability2DmgMinion, parentCollider);          
                    Debug.Log("ability 2 trigger stay HERAGZON");
                    break;
            }
        }
    }
}
