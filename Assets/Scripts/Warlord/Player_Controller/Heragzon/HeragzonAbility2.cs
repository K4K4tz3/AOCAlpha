using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeragzonAbility2 : MonoBehaviour
{

    [SerializeField] private WarlordBaseClass heragzonSO;
    //könnte auch getcomponentinchildren

    private void OnTriggerStay(Collider other)
    {
        //damage wird jz jeden frame gemacht, ändern auf pro sekunde 
        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IDamagable d))
        {
            switch (tag)
            {
                case "Building":
                    d.GetDamaged(heragzonSO.ability2DmgBuilding);
                    Debug.Log("ability 2 trigger stay");
                    break;
                case "Warlord":
                    d.GetDamaged(heragzonSO.ability2DmgWarlord);
                    Debug.Log("ability 2 trigger stay");
                    break;
                case "Minion":
                    d.GetDamaged(heragzonSO.ability2DmgMinion);          
                    Debug.Log("ability 2 trigger stay");
                    break;
            }
        }
    }
}
