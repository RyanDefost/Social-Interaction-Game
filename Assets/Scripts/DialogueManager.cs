using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField] public int globalCount { get; private set; } = 0;
    private List<DialogueInteraction> dialogueInteractions = new List<DialogueInteraction>();

    public UnityEvent OnLockingText = new UnityEvent();

    private void Awake() => DontDestroyOnLoad(gameObject);

    public void IncreaseGlobalCount() => globalCount++;

    public void LockOtherTexts() => OnLockingText?.Invoke();
}
