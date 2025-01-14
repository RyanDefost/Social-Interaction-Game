using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private DialogueInteraction _startDialogue;

    private bool _isChangeingScene = false;

    private void Awake() => DontDestroyOnLoad(gameObject);

    private void Update() => SceneManager.sceneLoaded += OnsceneLoaded;
    private void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_isChangeingScene)
        {
            _isChangeingScene = true;
            _startDialogue = TryGetGlobalDialogue();
            StartCoroutine(WaitBeforeInteraction());
        }
    }

    private IEnumerator WaitBeforeInteraction()
    {
        yield return new WaitForEndOfFrame();

        if (_startDialogue != null)
            _startDialogue.ActivateInteraction();

        _isChangeingScene = false;
    }

    /// <summary>
    /// Tries to Get a "DialogueInteraction" with the _globalDIalogue bool set to True.
    /// </summary>
    /// <returns>A DialogueInteraction with a _globalDIalogue set to True OR Null.</returns>
    private DialogueInteraction TryGetGlobalDialogue()
    {
        DialogueInteraction[] interactions = FindObjectsOfType<DialogueInteraction>();
        DialogueInteraction _globalDialogue = null;

        foreach (var dialogue in interactions)
        {
            if (dialogue.GetGlobalStatus())
            {
                _globalDialogue = dialogue;
                break;
            }
        }

        return _globalDialogue;
    }
}
