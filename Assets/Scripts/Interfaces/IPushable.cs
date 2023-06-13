using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    //for letting enemies go in the opposite direction
    public void GetPushedAway(float duration, Vector2 direction);

    //for pull enemies over 
    public void GetPulledOver(float duration, Vector2 direction);
}
