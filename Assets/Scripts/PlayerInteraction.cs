using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] LayerMask interactMask;
    [SerializeField] float interactRange;
    private void FixedUpdate() {
        if (Input.GetButtonDown("Interact")) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, interactRange, interactMask)) {
                if(hit.transform.tag == "Interactable") {
                    hit.transform.GetComponent<Interactable>().Interact();
                }
            }
        }
    }
}
