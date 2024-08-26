using UnityEngine;

public class MoveAction : ActionBase
{
    private readonly Vector3 _targetPosition;
    private readonly Vector3 _currentPosition;
    public MoveAction(MovementPlayer movementPlayer, Vector3 targetPosition, Vector3 currentPosition) : base(movementPlayer)
    {
        _targetPosition = targetPosition;
        _currentPosition = currentPosition;
    }

    public override void Execute()
    {
        Player.OnMove(_targetPosition);
        LevelsManager.instance.ActionNumberDecrease();
    }

    public override void Undo()
    {
        Player.OnMove(_currentPosition);
        LevelsManager.instance.ActionNumberIncrease();
    }
}
