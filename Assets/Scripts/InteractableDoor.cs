public class InteractableDoor : Interactable {
    private PortalDoor door;

    private void Start() {
        door = GetComponentInParent<PortalDoor>();
    }

    public override void Interact() {
        door.Interact();
    }
}
