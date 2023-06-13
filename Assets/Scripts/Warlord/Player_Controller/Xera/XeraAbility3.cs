using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XeraAbility3 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass xeraSO;
    private w_Xera xeraController;

    private void Start()
    {
        xeraController = GetComponent<w_Xera>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(xeraController.eExplosion)
        {
            var tag = other.tag;
            if (other.gameObject.TryGetComponent(out IDamagable d))
            {
                switch (tag)
                {                    
                    case "Warlord":
                        d.GetDamaged(xeraSO.ability3DmgWarlord);
                        Debug.Log("ability 3 trigger stay");
                        break;
                    case "Minion":
                        d.GetDamaged(xeraSO.ability3DmgMinion);
                        Debug.Log("ability 3 trigger stay");
                        break;
                }
            }

            xeraController.eExplosion = false;
        }
    }
}
