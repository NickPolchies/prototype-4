using System.Collections;
using UnityEngine;

public class PortalTriggerTwo : MonoBehaviour {
    private PortalDoor portal;

    private void Start() {
        portal = GetComponentInParent<PortalDoor>();
    }

    private IEnumerator OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            CharacterController playerController = other.GetComponentInParent<CharacterController>();

            if (Vector3.Dot(transform.forward, playerController.velocity) > 0) {
                portal.Teleport(playerController.transform);
                // yield return new WaitForFixedUpdate();
                // portalController.Teleport(A, other.transform);
                yield return null;
            }
        }
    }
}
