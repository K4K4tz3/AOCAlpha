using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XeraAbility1 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass xeraSO;
    [SerializeField] private Collider parentCollider;

    private void OnTriggerStay(Collider other)
    {
        //Do Damage 
        //damage wird jz jeden frame gemacht, ändern auf pro sekunde 
        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IDamagable d))
        {
            switch (tag)
            {
                case "Building":
                    d.GetDamaged(xeraSO.ability1DmgBuilding, parentCollider);
                    Debug.Log("ability 1 trigger stay");
                    break;
                case "Warlord":
                    d.GetDamaged(xeraSO.ability1DmgWarlord, parentCollider);
                    Debug.Log("ability 1 trigger stay");
                    break;
                case "HostileMinion":
                    d.GetDamaged(xeraSO.ability1DmgMinion, parentCollider);                  
                    Debug.Log("ability 1 trigger stay");
                    break;
            }
        }
    }
}
