using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 5f;

    private Camera _camera;
    private PlatformerMovement _player;

    private float _currentCameraHeight = 0;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlatformerMovement>();
    }

    private void Update() => FollowPlayer();

    private void FollowPlayer()
    {
        Vector2 playerPosition = _player.transform.position;
        Vector2 cameraPosition = _camera.transform.position;

        Vector2 lerpedPosition = LerpPosition(cameraPosition, playerPosition, _lerpSpeed * Time.deltaTime);
        Vector2 lerpedHeightPosition = ChangeHeight();
        transform.position = new Vector3(lerpedPosition.x, lerpedHeightPosition.y, transform.position.z);
    }

    private Vector2 ChangeHeight()
    {
        float playerHeight = _player.transform.position.y;

        if (playerHeight < _currentCameraHeight - _camera.orthographicSize)
            _currentCameraHeight -= _camera.orthographicSize * 2;
        if (playerHeight > _currentCameraHeight + _camera.orthographicSize)
            _currentCameraHeight += _camera.orthographicSize * 2;

        Vector2 cameraPosition = new Vector2(transform.position.z, _currentCameraHeight);
        Vector2 LerpedHeigth = LerpPosition(_camera.transform.position, cameraPosition, _lerpSpeed * Time.deltaTime);
        return LerpedHeigth;
    }

    private Vector2 LerpPosition(Vector2 startPosition, Vector2 endPosition, float lerpAmount)
    {
        return Vector2.Lerp(startPosition, endPosition, lerpAmount);
    }
}
