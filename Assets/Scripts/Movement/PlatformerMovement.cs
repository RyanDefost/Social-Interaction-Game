using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlatformerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;

    [SerializeField] protected float _jumpMultiplier;


    private GroundDetector _groundDetector;
    private Rigidbody2D _rigidBody;

    private bool _canMove = true;
    private bool _isGrounded = false;

    private void Awake()
    {
        _groundDetector = GetComponentInChildren<GroundDetector>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (!_canMove)
            return;

        _isGrounded = _groundDetector.GetGroundedState();
        _groundDetector.SetGroundParentState(gameObject);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            StartCoroutine(Move(Vector2.left, _speed));

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            StartCoroutine(Move(Vector2.right, _speed));

        if (!_isGrounded)
            return;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            StartCoroutine(Jump(Vector2.up, _jumpMultiplier));
    }

    private IEnumerator Jump(Vector2 diraction, float jumpHeight)
    {
        _rigidBody.velocity += _speed * (diraction * jumpHeight) * Time.deltaTime;
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, _maxSpeed);

        yield return null;
    }

    private IEnumerator Move(Vector2 moveDiraction, float speed)
    {
        moveDiraction *= speed;

        _rigidBody.velocity += moveDiraction * Time.deltaTime;
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, _maxSpeed);

        yield return null;
    }

    public void SetMovability(bool canMove) => _canMove = canMove;
}
