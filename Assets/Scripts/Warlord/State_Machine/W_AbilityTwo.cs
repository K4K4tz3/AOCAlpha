

public class W_AbilityTwo : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
    
    }

    public override void UpdateState(PlayerController controller)
    {
       
    }
    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.doingAbility2 = false;
        controller.SwitchState(state);

    }

}
