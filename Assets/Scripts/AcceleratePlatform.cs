using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class AcceleratePlatform : MonoBehaviour
{
    [SerializeField] private Vector3 _accelerationDiraction = Vector3.up;
    [SerializeField] private float _accelerationAmount = 10f;

    private void TryAccelerateObject(GameObject gameObject)
    {
        gameObject.TryGetComponent<Rigidbody2D>(out var rigidbody2D);
        if (rigidbody2D == null)
            return;

        AccelerateObject(gameObject, _accelerationDiraction, _accelerationAmount);
    }

    private void AccelerateObject(GameObject gameObject, Vector2 diraction, float accelerationAmount)
    {
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        rigidbody2D.AddForce(diraction * accelerationAmount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent<Rigidbody2D>(out var rigidbody2D);
        if (rigidbody2D == null)
            return;

        if (rigidbody2D.bodyType.ToString() != "Static")
            TryAccelerateObject(collision.gameObject);
    }
}
