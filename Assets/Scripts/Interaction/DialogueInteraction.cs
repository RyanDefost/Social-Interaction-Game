using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueInteraction : Interactable
{
    [SerializeField] private List<DialogueSegment> _dialogueSegments = new List<DialogueSegment>();
    [SerializeField] private List<string> _fallbackText = new List<string>();

    [SerializeField] private bool _isGlobalDialogue = false;

    [SerializeField] bool _LockAfterDisplay = false;
    private bool _isTextLocked = false;

    [SerializeField] private GameObject _textBox;
    private DialogueText _textDisplayer;
    private DialogueManager _manager;

    [HideInInspector] public UnityEvent OnEndDialogue = new UnityEvent();

    private bool _isSingleDilogue;
    private int _localCount;
    private int _currentDialogueSegment;
    private void Awake()
    {
        _textDisplayer = _textBox.GetComponent<DialogueText>();
        _manager = FindObjectOfType<DialogueManager>();

        _isSingleDilogue = _dialogueSegments.Count <= 1 ? true : false;
        _manager.OnLockingText.AddListener(SetLockedState);
    }

    /// <summary>
    /// Logic for Global Dialog that start at the beginning of the scene.
    /// </summary>
    private void Update()
    {
        if (!_isGlobalDialogue)
            return;

        if (_textBox.activeSelf)
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                StartDialogue();
    }

    public override void ActivateInteraction() => StartDialogue();

    private void StartDialogue()
    {
        _player.SetMovability(false);

        if (!_textBox.activeSelf)
        {
            Debug.Log("ACTIVATING THE TEXTBOX");
            _textBox.SetActive(true);
        }

        if (_isTextLocked && _LockAfterDisplay)
        {
            _textDisplayer.DisplayText(_fallbackText[0]);

            if (_localCount > 0)
                EndDialogue();
            else
                _localCount++;

            return;
        }

        SetCurrentDialogueSegment();

        if (_dialogueSegments.Count == 0)
            _textDisplayer.DisplayText(_fallbackText[0]);

        if (_localCount >= _dialogueSegments[_currentDialogueSegment].Texts.Count)
        {
            EndDialogue();
            return;
        }

        Debug.Log(_textDisplayer);

        _textDisplayer.DisplayText(_dialogueSegments[_currentDialogueSegment].Texts[_localCount]);
        _localCount++;

        _isActivated = true;
    }

    private void EndDialogue()
    {
        OnEndDialogue?.Invoke();

        if (_LockAfterDisplay)
            _manager.LockOtherTexts();

        _textDisplayer.ResetText();
        _textBox.SetActive(false);
        _localCount = 0;

        _player.SetMovability(true);

        //Stops the GlobalDialogue from always appearing.
        if (_isGlobalDialogue)
            _isGlobalDialogue = false;
    }

    private void SetCurrentDialogueSegment()
    {
        if (_isSingleDilogue)
            _currentDialogueSegment = 0;
        else
            _currentDialogueSegment = _manager.globalCount;
    }

    public bool GetGlobalStatus() => _isGlobalDialogue;
    private void SetLockedState() => _isTextLocked = true;

}

[System.Serializable]
public class DialogueSegment
{
    public List<string> Texts;
}
