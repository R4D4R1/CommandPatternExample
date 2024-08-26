using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class MovementPlayer : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] private float movementDistance = .75f;
    [SerializeField] private float movementSpeed = 5f;  
    private Vector3 _targetPosition;
    private bool isMoving = false;

    [Header("Rotating")]
    [SerializeField] private float rotationAmount = 90f;
    [SerializeField] private float rotationSpeed = 5f;
    private Quaternion _targetRotation;
    private bool isRotating = false;

    [Header("ActionRecorder")]
    [SerializeField] private ActionRecorder actionRecorder;

    [Header("PauseMenuEvent")]
    private bool _gamePaused = false;
    [SerializeField] private UnityEvent pauseMenuEvent;
    [SerializeField] private UnityEvent unPauseMenuEvent;

    public static MovementPlayer instance = null;

    private bool _isAbleToWalk = false;
    private void Awake()
    {
        if (instance == null)
        {
            _targetPosition = transform.position;

            instance = this;
            return;
        }
        Destroy(this.gameObject);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        // MOVING
         
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_gamePaused)
            {
                _gamePaused = true;
                _isAbleToWalk = false;
                pauseMenuEvent.Invoke();
            }
            else
            {
                _gamePaused = false;
                _isAbleToWalk = true;
                unPauseMenuEvent.Invoke();
            }

        }

        if (!isRotating && !isMoving)
        {

            // WALKING 
            if (LevelsManager.instance.MovesLeft())
            {

                if (Input.GetKeyDown(KeyCode.W) && _isAbleToWalk)
                {
                    AttemptToMoveInDirection(transform.forward, "WALL AHEAD");
                }

                else if (Input.GetKeyDown(KeyCode.S) && _isAbleToWalk)
                {
                    AttemptToMoveInDirection(-transform.forward, "WALL BACK");
                }

                else if (Input.GetKeyDown(KeyCode.A) && _isAbleToWalk)
                {
                    AttemptToMoveInDirection(-transform.right, "WALL LEFT");
                }


                else if (Input.GetKeyDown(KeyCode.D) && _isAbleToWalk)
                {
                    AttemptToMoveInDirection(transform.right, "WALL RIGHT");
                }

            }


            // ROTATION

            if (Input.GetKeyDown(KeyCode.E) && _isAbleToWalk)
            {
                _targetRotation = Quaternion.Euler(0f, rotationAmount, 0f) * transform.rotation;

                StartRotationAction(_targetRotation,transform.rotation);
            }

            if (Input.GetKeyDown(KeyCode.Q) && _isAbleToWalk)
            {
                _targetRotation = Quaternion.Euler(0f, -rotationAmount, 0f) * transform.rotation;

                StartRotationAction(_targetRotation, transform.rotation);
            }
        }
    }

    private void AttemptToMoveInDirection(Vector3 direction,string wallText)
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1.1f))
        {
            if (hit.collider.transform.gameObject.layer != 6)
            {

                _targetPosition += direction * movementDistance;
                StartMoveAction(_targetPosition, transform.position);
                
                if (hit.collider.transform.gameObject.layer == 7)
                {
                    _isAbleToWalk = false;
                    LevelsManager.instance.GameWon();
                }
            }
            else
            {
                Debug.Log(wallText);
            }
        }
        else
        {
            _targetPosition += direction * movementDistance;
            StartMoveAction(_targetPosition, transform.position);
        }
    }

    public void ResetPosition()
    {
        _targetPosition = transform.position;

    }

    private void StartMoveAction(Vector3 targetPosition, Vector3 currentPosition)
    {
        var action = new MoveAction(this, targetPosition, currentPosition);
        actionRecorder.Record(action);
    }

    private void StartRotationAction(Quaternion targetRotation,Quaternion currentRotation)
    {
        var action = new RotationAction(this, targetRotation, currentRotation);
        actionRecorder.Record(action);

    }

    public void OnMove(Vector3 targetPosition)
    {
        StartCoroutine(Move(targetPosition));
    }

    public void OnRotate(Quaternion targetRotation)
    {
        StartCoroutine(Rotate(targetRotation));
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0f)
        {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    IEnumerator Rotate(Quaternion targetRotation)
    {
        isRotating = true;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0f)
        {
            yield return new WaitForEndOfFrame();
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public void RestartLevel()
    {
        _isAbleToWalk = true;
    }
}
