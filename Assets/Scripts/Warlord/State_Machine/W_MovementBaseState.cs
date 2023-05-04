
public abstract class W_MovementBaseState 
{
    public abstract void EnterState(PlayerController controller);
    public abstract void UpdateState(PlayerController controller);
    public abstract void ExitState(PlayerController controller, W_MovementBaseState state);

}
