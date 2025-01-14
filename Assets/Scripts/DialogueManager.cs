using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField] public int globalCount { get; private set; } = 0;
    private List<DialogueInteraction> dialogueInteractions = new List<DialogueInteraction>();

    private void Awake() => DontDestroyOnLoad(gameObject);

    public void IncreaseGlobalCount() => globalCount++;

}
