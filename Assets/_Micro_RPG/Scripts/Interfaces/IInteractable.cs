public interface IInteractable : IClickable
{
    bool CanInteract { get; }
    void Interact();
}
