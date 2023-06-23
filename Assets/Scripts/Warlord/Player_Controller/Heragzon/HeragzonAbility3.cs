using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeragzonAbility3 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass heragzonSO;

    private void OnTriggerStay(Collider other)
    {

        //damage wird jz jeden frame gemacht, �ndern auf pro sekunde 
        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IDamagable d))
        {
            switch (tag)
            {
                case "Building":
                    d.GetDamaged(heragzonSO.ability3DmgBuilding);
                    Debug.Log("ability 3 trigger stay HERAGZON");
                    break;
                case "Warlord":
                    d.GetDamaged(heragzonSO.ability3DmgWarlord);
                    Debug.Log("ability 3 trigger stay HERAGZON");
                    break;
                case "HostileMinion":
                    d.GetDamaged(heragzonSO.ability3DmgMinion);
                    Debug.Log("ability 3 trigger stay HERAGZON");
                    break;
            }
        }
    }
}
