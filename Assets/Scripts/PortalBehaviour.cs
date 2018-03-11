using UnityEditor;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] Transform portalA;
    [SerializeField] Transform portalB;
    [SerializeField] PortalCamera camA;
    [SerializeField] PortalCamera camB;
    CharacterController playerController;
    UnityStandardAssets.Characters.FirstPerson.MouseLook look;

    public bool portalAActive = true, portalBActive = true;

    public bool pauseOnTeleport = false;

    private void Start() {
        look = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook;
        playerController = player.GetComponent<CharacterController>();
    }

    public void ActivatePortal(bool a) {
        if (a && portalAActive) {
            ActivatePortalHelper(portalA, portalB);
        }
        else if (portalBActive) {
            ActivatePortalHelper(portalB, portalA);
        }
    }

    private void ActivatePortalHelper(Transform from, Transform to) {
        if(Vector3.Dot(from.forward, playerController.velocity) > 0) {
            Quaternion toRotation = Quaternion.AngleAxis(180, to.up) * to.rotation;
            player.position = toRotation * Quaternion.Inverse(from.rotation) * (player.position - from.position) + to.position;
            look.m_CharacterTargetRot = toRotation * Quaternion.Inverse(from.rotation) * player.rotation;
        }
    }

    public void Teleport(bool fromPortalA, Transform subject) {
        Transform from;
        Transform to;

        if (fromPortalA) {
            from = portalA;
            to = portalB;
        }
        else {
            from = portalB;
            to = portalA;
        }

        Quaternion toRotation = Quaternion.AngleAxis(180, to.up) * to.rotation;
        subject.position = toRotation * Quaternion.Inverse(from.rotation) * (subject.position - from.position) + to.position;
        look.m_CharacterTargetRot = toRotation * Quaternion.Inverse(from.rotation) * subject.rotation;
        camA.UpdatePosition();
        camB.UpdatePosition();

        camA.GetComponent<Camera>().Render();
        camB.GetComponent<Camera>().Render();

        if(pauseOnTeleport) EditorApplication.isPaused = true;
    }
}
