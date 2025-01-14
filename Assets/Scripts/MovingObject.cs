using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class MovingObjects : MonoBehaviour
{
    [SerializeField] private Vector2 _moveDiraction;

    [SerializeField] private float _speed = 1;
    [SerializeField] private float _waitTime = 1;

    private bool _isMoving = false;
    private float _moveTime;

    private void Update()
    {
        if (!_isMoving)
            StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        _isMoving = true;
        _moveTime = _speed * Time.deltaTime;

        var currentPosition = gameObject.transform.position;
        var finalPosition = gameObject.transform.position + new Vector3(_moveDiraction.x, _moveDiraction.y, transform.position.z);
        for (float i = 0; i < 1; i += _moveTime)
        {
            currentPosition = Vector2.Lerp(currentPosition, finalPosition, _moveTime);
            transform.position = currentPosition;

            yield return new WaitForSeconds(_moveTime);
        }

        StartCoroutine(WaitForRestart(_waitTime));
        yield return null;
    }

    private IEnumerator WaitForRestart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _moveDiraction = -_moveDiraction;
        _isMoving = false;
    }
}
