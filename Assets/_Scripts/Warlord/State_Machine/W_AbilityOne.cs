

public class W_AbilityOne : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
    
    }
    public override void UpdateState(PlayerController controller)
    {
 
    }

    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.doingAbility1 = false;
        controller.SwitchState(state);
    }

}
