
public class W_AutoAttackState : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
      
    }
    public override void UpdateState(PlayerController controller)
    {
       
        if(!controller.doingAutoAttack && controller.isMoving)
        {
            ExitState(controller, controller.WalkState);
        }
        if(!controller.doingAutoAttack && !controller.isMoving)
        {
            ExitState(controller, controller.IdleState);
        }
            
    }

    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.doingAutoAttack = false;
        controller.SwitchState(state);
    }

}
