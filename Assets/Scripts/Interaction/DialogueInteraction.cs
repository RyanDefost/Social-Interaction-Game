using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueInteraction : Interactable
{
    [SerializeField] private List<DialogueSegment> _dialogueSegments = new List<DialogueSegment>();
    [SerializeField] private List<string> _fallbackText = new List<string>();

    [SerializeField] private bool _isGlobalDialogue = false;

    [SerializeField] private GameObject _textBox;
    private DialogueText _textDisplayer;
    private DialogueManager _manager;

    public UnityEvent OnEndDialogue = new UnityEvent();

    private int _localCount;
    private bool _isTextLocked;

    private bool _isSingleDilogue;
    private int _currentDialogueSegment;
    private void Awake()
    {
        _textDisplayer = _textBox.GetComponent<DialogueText>();
        _manager = FindObjectOfType<DialogueManager>();

        _isSingleDilogue = _dialogueSegments.Count <= 1 ? true : false;
    }

    private void Update()
    {
        if (!_isGlobalDialogue)
            return;

        if (_textBox.activeSelf)
            if (Input.GetKeyDown(KeyCode.Space))
                StartDialogue();
    }

    public override void ActivateInteraction() => StartDialogue();

    private void StartDialogue()
    {
        _player.SetMovability(false);

        if (!_textBox.activeSelf)
            _textBox.SetActive(true);

        if (_isTextLocked)
        {
            _textDisplayer.DisplayText(_fallbackText[0]);
            _localCount++;
            return;
        }

        SetCurrentDialogueSegment();

        if (_dialogueSegments.Count == 0)
            _textDisplayer.DisplayText(_fallbackText[0]);

        if (_localCount >= _dialogueSegments[_currentDialogueSegment].Texts.Count || _isTextLocked)
        {
            EndDialogue();
            return;
        }

        _textDisplayer.DisplayText(_dialogueSegments[_currentDialogueSegment].Texts[_localCount]);
        _localCount++;

        _isActivated = true;
    }

    private void EndDialogue()
    {
        OnEndDialogue?.Invoke();

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
}

[System.Serializable]
public class DialogueSegment
{
    public List<string> Texts;
}
