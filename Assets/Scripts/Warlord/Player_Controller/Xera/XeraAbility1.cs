using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XeraAbility1 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass xeraSO;

    private void OnTriggerStay(Collider other)
    {
        //Do Damage 
        //damage wird jz jeden frame gemacht, �ndern auf pro sekunde 
        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IDamagable d))
        {
            switch (tag)
            {
                case "Building":
                    d.GetDamaged(xeraSO.ability1DmgBuilding);
                    Debug.Log("ability 1 trigger stay");
                    break;
                case "Warlord":
                    d.GetDamaged(xeraSO.ability1DmgWarlord);
                    Debug.Log("ability 1 trigger stay");
                    break;
                case "Minion":
                    d.GetDamaged(xeraSO.ability1DmgMinion);                  
                    Debug.Log("ability 1 trigger stay");
                    break;
            }
        }
    }
}