using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour
{
    [SerializeField] private DialogueInteraction _startDialogue;
    [SerializeField] private float _delay = 1;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _startDialogue.OnEndDialogue.AddListener(StartFade);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void StartFade() => StartCoroutine(FadeSprite());

    private IEnumerator FadeSprite()
    {
        for (float i = 1; i >= 0; i -= 0.1f)
        {
            _spriteRenderer.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(_delay / 10);
        }
    }
}
