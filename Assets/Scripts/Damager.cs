using UnityEngine;
using UnityEngine.SceneManagement;

public class Damager : MonoBehaviour
{
    private string _currentScene;

    private void Awake() => _currentScene = SceneManager.GetActiveScene().name;

    private void TryDamageObject(GameObject CurrentObject)
    {
        PlatformerMovement playerMovement;
        CurrentObject.TryGetComponent<PlatformerMovement>(out playerMovement);

        if (playerMovement == null)
            return;

        DamagePlayer();
    }

    private void DamagePlayer() => SceneManager.LoadScene(_currentScene);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);

        if (collision.gameObject.tag == "Player")
            TryDamageObject(collision.gameObject);
    }
}
