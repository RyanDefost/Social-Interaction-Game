using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask _interactionLayer;

    private Collision2D _currentInteraction;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            TryInteract();
    }

    public void MoveInteractor(Vector3 moveDiraction)
    {
        gameObject.transform.localPosition = moveDiraction;
    }

    public bool CheckCollision()
    {
        if (_currentInteraction == null)
            return true;

        //Compares the collision layer with the layer-mask.
        if (1 << _currentInteraction.gameObject.layer == _interactionLayer.value)
            return false;

        return true;
    }

    public bool TryInteract()
    {
        if (_currentInteraction == null)
            return false;

        _currentInteraction.gameObject.TryGetComponent<Interactable>(out var interactable);
        if (interactable != null)
            Interact(interactable);

        return true;
    }

    private void Interact(Interactable interactable) => interactable.ActivateInteraction();

    private void OnCollisionEnter2D(Collision2D collision) => _currentInteraction = collision;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision == _currentInteraction)
            _currentInteraction = null;
    }

}
