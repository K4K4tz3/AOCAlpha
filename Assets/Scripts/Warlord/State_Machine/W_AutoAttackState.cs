
public class W_AutoAttackState : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
      
    }
    public override void UpdateState(PlayerController controller)
    {
    }

    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.doingAutoAttack = false;
        controller.SwitchState(state);
    }

}
