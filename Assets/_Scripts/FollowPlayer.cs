using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 startPosCamera;
    private void Start()
    {
        startPosCamera = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = startPosCamera + Player.instance.transform.position;
    }
}
