using UnityEngine;

public class RotationAction : ActionBase
{
    private readonly Quaternion _targetRotation;
    private readonly Quaternion _currentRotation;
    public RotationAction(MovementPlayer movementPlayer, Quaternion targetRotation, Quaternion currentRotation) : base(movementPlayer)
    {
        _targetRotation = targetRotation;
        _currentRotation = currentRotation;
    }

    public override void Execute()
    {
        Player.OnRotate(_targetRotation);
    }

    public override void Undo()
    {
        Player.OnRotate(_currentRotation);
    }
}
