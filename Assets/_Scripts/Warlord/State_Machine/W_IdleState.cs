
using UnityEngine;

public class W_IdleState : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
        
    }

    public override void UpdateState(PlayerController controller)
    {
       
        if (controller.isMoving)
        {
            ExitState(controller, controller.WalkState);
        }
        if(controller.doingAutoAttack)
        {
            ExitState(controller, controller.AAState);
        }





    }

    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
       controller.SwitchState(state);
    }
}
