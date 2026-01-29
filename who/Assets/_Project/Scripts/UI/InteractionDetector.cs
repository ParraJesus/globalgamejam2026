using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector: MonoBehaviour
{
    private IInteractive interactiveInRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactiveInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractive interactable) && interactable.CanInteract())
        {
            interactiveInRange = interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractive interactable) && interactable == interactiveInRange)
        {
            interactiveInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}
