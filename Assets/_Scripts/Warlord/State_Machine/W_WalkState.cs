using UnityEngine;

public class W_WalkState : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
        
    }
    public override void UpdateState(PlayerController controller)
    {
        
        if (!controller.isMoving)
        {
            ExitState(controller, controller.IdleState);
        }
    }

    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.SwitchState(state);
    }

}
