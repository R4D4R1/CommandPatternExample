public abstract class ActionBase
{
    protected readonly MovementPlayer Player;
    protected ActionBase(MovementPlayer movementPlayer)
    {
        Player = movementPlayer;
    }

    public abstract void Execute();
    public abstract void Undo();
}
