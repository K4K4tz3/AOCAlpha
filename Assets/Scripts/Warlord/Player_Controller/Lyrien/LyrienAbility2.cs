using UnityEngine;

public class LyrienAbility2 : MonoBehaviour
{
    [SerializeField] private WarlordBaseClass lyrienSO;
    private w_Lyrien lyrienController;

    private void Start()
    {
        lyrienController = GetComponent<w_Lyrien>();
    }

    private void OnTriggerStay(Collider other)
    {
        //check ob gepushed oder angezogen werden soll

        var tag = other.tag;
        if (other.gameObject.TryGetComponent(out IPushable p))
        {
            switch (tag)
            {
                case "Warlord":
                    //get view direction and take the opposite for pushing direction
                    Vector2 viewDirectionWarlord = other.gameObject.transform.forward * -1;
                    if (!lyrienController.wPressedTwice)
                    {
                        p.GetPushedAway(lyrienSO.ability3Duration, viewDirectionWarlord);
                        Debug.Log("ability 3 trigger stay push");
                    }
                    else
                    {
                        p.GetPulledOver(lyrienSO.ability3Duration, viewDirectionWarlord);
                        Debug.Log("ability 3 trigger stay pull");
                    }
                    break;
                case "Minion":
                    //get view direction and take the opposite for pushing direction
                    Vector2 viewDirectionMinion = other.gameObject.transform.forward * -1;
                    if (!lyrienController.wPressedTwice)
                    {
                        p.GetPushedAway(lyrienSO.ability3Duration, viewDirectionMinion);
                        Debug.Log("ability 3 trigger stay push");
                    }
                    else
                    {
                        p.GetPulledOver(lyrienSO.ability3Duration, viewDirectionMinion);
                        Debug.Log("ability 3 trigger stay pull");
                    }
                    break;
            }
        }
    }
}
