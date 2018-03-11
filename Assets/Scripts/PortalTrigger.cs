using System.Collections;
using UnityEngine;

public class PortalTrigger : MonoBehaviour {
    [SerializeField] bool A;
    [SerializeField] PortalBehaviour portalController;

    private IEnumerator OnTriggerStay(Collider other) {
        if(other.tag == "Player") {
            Debug.Log("Colliding");
            CharacterController playerController = other.GetComponentInParent<CharacterController>();

            if (Vector3.Dot(transform.forward, playerController.velocity) > 0) {
                yield return new WaitForFixedUpdate();
                portalController.Teleport(A, other.transform);
            }
        }
    }
}
