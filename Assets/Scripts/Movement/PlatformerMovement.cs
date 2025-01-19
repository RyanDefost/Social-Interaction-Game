using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlatformerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;

    [SerializeField] protected float _jumpMultiplier;


    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GroundDetector _groundDetector;
    private Rigidbody2D _rigidBody;

    private bool _canMove = true;
    private bool _isGrounded = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _groundDetector = GetComponentInChildren<GroundDetector>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
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
        {
            SetAnimation(Vector2.up, "JumpPlatform");

            if (!Input.anyKey)
                StartCoroutine(DragMovment());

            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            StartCoroutine(Jump(Vector2.up, _jumpMultiplier));
    }

    private IEnumerator Move(Vector2 moveDiraction, float speed)
    {
        if (_isGrounded)
            SetAnimation(moveDiraction, "MovePlatfomr");

        moveDiraction *= speed;

        _rigidBody.velocity += moveDiraction * Time.deltaTime;

        float clampedVelocityX = SetClampedSpeed(_rigidBody.velocity.x, 0f, _maxSpeed);
        _rigidBody.velocity = new Vector2(clampedVelocityX, _rigidBody.velocity.y);

        yield return null;
    }

    private IEnumerator Jump(Vector2 diraction, float jumpHeight)
    {
        _rigidBody.velocity += (diraction * jumpHeight) * Time.deltaTime;

        float clampedVelocityY = _rigidBody.velocity.y;
        clampedVelocityY = Mathf.Clamp(clampedVelocityY, 0, _maxSpeed * jumpHeight);
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, clampedVelocityY);

        yield return null;
    }

    private float SetClampedSpeed(float value, float minSpeed, float maxSpeed)
    {
        bool isNagative = value < 0 ? true : false;

        value = Mathf.Abs(value);
        value = Mathf.Clamp(value, minSpeed, maxSpeed);

        if (isNagative)
            return -value;
        else
            return value;
    }

    private IEnumerator DragMovment()
    {
        while (_rigidBody.velocity.x > 0)
        {
            if (_rigidBody.velocity.x > 0)
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x - 0.1f, _rigidBody.velocity.y);
            else
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x - -0.1f, _rigidBody.velocity.y);

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    private void SetAnimation(Vector2 diraction, string animationName)
    {
        _spriteRenderer.flipX = diraction.x < 0 ? true : false;
        _animator.Play(animationName);
    }

    public void SetMovability(bool canMove) => _canMove = canMove;
}
