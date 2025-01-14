using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TileMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;

    private Interactor _interactor;

    private bool _isMoving = false;
    private bool _firstMoveLoop = false;
    private bool _canMove = true;

    private void Start() => _interactor = gameObject.GetComponentInChildren<Interactor>();

    private void Update()
    {
        if (_isMoving || !_canMove)
            return;

        _firstMoveLoop = true;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            StartCoroutine(Move(Vector3.up));

        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            StartCoroutine(Move(Vector3.left));

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            StartCoroutine(Move(Vector3.down));

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            StartCoroutine(Move(Vector3.right));
    }

    private IEnumerator Move(Vector3 moveDiraction)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPostition = transform.position + moveDiraction;

        float timeElapsed = 0;
        float lerpDuration = _speed;
        _isMoving = true;

        //Sets the interaction position and gives the CollisionDetection time to update.
        _interactor.MoveInteractor(moveDiraction);
        if (_firstMoveLoop)
        {
            yield return new WaitForSeconds(0.02f); //Easy "fix", should be done differently.
            _firstMoveLoop = false;
        }

        //yield return new WaitForSeconds(0.1f);
        if (!_interactor.CheckCollision())
        {
            _isMoving = false;
            yield break;
        }

        while (timeElapsed <= lerpDuration)
        {
            Vector3 middleStep;
            middleStep.x = Mathf.Lerp(startPosition.x, endPostition.x, timeElapsed / lerpDuration);
            middleStep.y = Mathf.Lerp(startPosition.y, endPostition.y, timeElapsed / lerpDuration);
            middleStep.z = Mathf.Lerp(startPosition.z, endPostition.z, timeElapsed / lerpDuration);
            transform.position = middleStep;

            timeElapsed += Time.deltaTime;

            if (timeElapsed >= lerpDuration)
                transform.position = endPostition;

            yield return null;
        }

        _isMoving = false;
    }

    public void SetMovability(bool canMove) => _canMove = canMove;
}
