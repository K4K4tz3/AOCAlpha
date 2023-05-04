

public class W_DeathState : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
      
    }
    public override void UpdateState(PlayerController controller)
    {

    }

    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.isDead = false;
        controller.SwitchState(state);

    }

}
