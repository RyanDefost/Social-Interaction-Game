using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _scene;
    [SerializeField] private float _delay;

    [Space]
    [SerializeField] private SpriteRenderer _transitionFade;

    private PlatformerMovement _player;

    private void Awake() => _player = FindObjectOfType<PlatformerMovement>();

    private IEnumerator StartChangingScene()
    {
        _player.SetMovability(false);

        for (float i = 0; i < 1; i += 0.1f)
        {
            _transitionFade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(_delay / 10);
        }

        ChangeScene();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(_scene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);

        if (collision.gameObject.tag == "Player")
            StartCoroutine(StartChangingScene());
    }
}
