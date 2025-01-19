using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeInteraction : Interactable
{
    [SerializeField] private List<string> _scenes;
    [SerializeField] private float _delay;

    [SerializeField] private bool _updatesGlobalCount = false;
    [SerializeField] private bool _canInteract = false;

    [Space]
    [SerializeField] private SpriteRenderer _transitionFade;
    [SerializeField] private GameObject _indecator;
    private DialogueManager _dialogueManager;
    private DialogueInteraction _dialogueInteraction;

    private void Awake()
    {
        _dialogueManager = FindAnyObjectByType<DialogueManager>();
        _dialogueManager.OnLockingText.AddListener(SetInteractable);

        _dialogueInteraction = GetComponentInChildren<DialogueInteraction>();
    }

    public override void ActivateInteraction() => TryStartSceneChange();

    private void TryStartSceneChange()
    {
        if (_canInteract)
            StartCoroutine(StartSceneChange());
        else
            _dialogueInteraction.ActivateInteraction();
    }

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

    private void SetInteractable()
    {
        _indecator.SetActive(true);
        _canInteract = true;
    }
}
