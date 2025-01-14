using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeInteraction : Interactable
{
    [SerializeField] private List<string> _scenes;
    [SerializeField] private float _delay;

    [SerializeField] protected bool _updatesGlobalCount = false;

    [Space]
    [SerializeField] private SpriteRenderer _transitionFade;

    private DialogueManager _dialogueManager;

    private void Awake() => _dialogueManager = FindAnyObjectByType<DialogueManager>();

    public override void ActivateInteraction() => StartCoroutine(StartSceneChange());

    private IEnumerator StartSceneChange()
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
        string nextScene = _scenes[_dialogueManager.globalCount];

        UpdateGlobalCount();

        SceneManager.LoadScene(nextScene);
        _isActivated = true;
    }

    private void UpdateGlobalCount()
    {
        if (!_updatesGlobalCount)
            return;

        if (_dialogueManager != null)
            _dialogueManager.IncreaseGlobalCount();
    }
}
