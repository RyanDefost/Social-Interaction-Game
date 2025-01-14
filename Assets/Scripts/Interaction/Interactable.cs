using UnityEngine;

abstract public class Interactable : MonoBehaviour
{
    protected private bool _isActivated;
    protected private TileMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<TileMovement>();
        Debug.Log(_player.gameObject);
    }

    public abstract void ActivateInteraction();
}
