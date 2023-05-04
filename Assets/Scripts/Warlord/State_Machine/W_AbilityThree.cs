

public class W_AbilityThree : W_MovementBaseState
{
    public override void EnterState(PlayerController controller)
    {
      
    }

    public override void UpdateState(PlayerController controller)
    {
        
    }
    public override void ExitState(PlayerController controller, W_MovementBaseState state)
    {
        controller.doingAbility3 = false;
        controller.SwitchState(state);
    }

}
