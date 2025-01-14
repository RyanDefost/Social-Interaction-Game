using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private float _textSpeed;
    [SerializeField] private float _minSpeed;
    private TextMeshProUGUI _text;

    private Coroutine _currentCoroutine;

    private void Start() => _text = GetComponentInChildren<TextMeshProUGUI>();

    public void DisplayText(string dialog)
    {
        if (_currentCoroutine != null)
            StopAllCoroutines();

        _currentCoroutine = StartCoroutine(SetText(dialog));
    }

    /// <summary>
    /// Sets the given text one char at a time into the UI, based on the _textSpeed and _minSpeed.
    /// </summary>
    /// <param name="dialog">Given text that needs to be displayed</param>
    /// <returns>waits for the _textSpeed divided over the amount of Chars</returns>
    public IEnumerator SetText(string dialog)
    {
        char[] charArray = dialog.ToCharArray();
        string currentText = "";

        float speed = _textSpeed / charArray.Length;
        Debug.Log(speed);
        if (speed > _minSpeed)
            speed = _minSpeed;

        for (int i = 0; i < charArray.Length; i++)
        {
            currentText += charArray[i];

            yield return new WaitForSeconds(speed);
            _text.text = currentText;
        }
    }

    public void ResetText() => _text.text = " ";
}
