using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Door position linking
//Visual linking
//Teleportation linking

public class PortalDoor : MonoBehaviour {
    static int IDCount = 0;
    public int ID;
    public bool active;
    public bool open;
    public PortalDoor linkedDoor;
    public PortalDoor destination;
    public Transform playerCamera;
    public Material material;

    /*
    public PortalDoor linkedDoor {
        get {
            return linkedDoor;
        }

        set {
            linkedDoor = value;
        }
    }
    */

    public Animator animator;

    public new Camera camera;
    public new Renderer renderer;

    private void Start() {
        ID = IDCount++;
        animator = GetComponent<Animator>();
        camera = GetComponentInChildren<Camera>(true);
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach(Renderer rend in renderers) {
            if(rend.gameObject.layer == LayerMask.NameToLayer("Portal")) {
                renderer = rend;
                break;
            }
        }

        playerCamera = Camera.main.transform;
        
        material = renderer.material;
        material = Material.Instantiate(material);

        if (camera.targetTexture != null) {
            camera.targetTexture.Release();
        }

        RenderTexture tex = new RenderTexture(Screen.width, Screen.height, 24);
        tex.antiAliasing = 1;
        camera.targetTexture = tex;
        material.mainTexture = camera.targetTexture;
    }

    public void Interact() {
        Debug.Log("Interact");

        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (open) {
            Close();
        }
        else {
            Open();
        }
    }

    public void Open() {
        open = true;
        animator.SetBool("Clockwise", false);
        animator.SetTrigger("Open");

        if(linkedDoor != null) {
            destination = linkedDoor;
            destination.destination = this;

            active = true;
            renderer.gameObject.SetActive(true);
            renderer.material = linkedDoor.material;
            camera.gameObject.SetActive(true);

            destination.open = true;
            destination.animator.SetBool("Clockwise", false);
            destination.animator.SetTrigger("Open");
            destination.renderer.gameObject.SetActive(true);
            destination.renderer.material = material;
            destination.camera.gameObject.SetActive(true);
        }
    }

    public void Close() {
        open = false;
        active = false;
        animator.SetTrigger("Close");

        if(destination != null) {
            destination.destination = null;

            renderer.gameObject.SetActive(false);
            camera.gameObject.SetActive(false);

            destination.open = false;
            destination.animator.SetTrigger("Close");
            destination.renderer.gameObject.SetActive(false);
            destination.camera.gameObject.SetActive(false);
        }

        destination = null;
    }

    public void Teleport(Transform subject) {
        Transform from = transform;
        Transform to = destination.transform;
        var look = subject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook;

        Quaternion toRotation = Quaternion.AngleAxis(180, to.up) * to.rotation;
        subject.position = toRotation * Quaternion.Inverse(from.rotation) * (subject.position - from.position) + to.position;
        look.m_CharacterTargetRot = toRotation * Quaternion.Inverse(from.rotation) * subject.rotation;
    }

    void LateUpdate() {
        if (active) {
            UpdatePosition();
        }
    }

    public void UpdatePosition() {
        var rotatedLocal = (Quaternion.AngleAxis(180, transform.up) * transform.rotation);
        var invertedLinked = Quaternion.Inverse(linkedDoor.transform.rotation);
        var linkedToPlayerPosOffset = (playerCamera.position - linkedDoor.transform.position);

        camera.transform.position = rotatedLocal * invertedLinked * linkedToPlayerPosOffset + transform.position;
        camera.transform.rotation = rotatedLocal * invertedLinked * playerCamera.rotation;

        var rotatedLinked = (Quaternion.AngleAxis(180, linkedDoor.transform.up) * linkedDoor.transform.rotation);
        var invertedLocal = Quaternion.Inverse(transform.rotation);
        var localToPlayerPosOffset = (playerCamera.position - transform.position);

        linkedDoor.camera.transform.position = rotatedLinked * invertedLocal * localToPlayerPosOffset + linkedDoor.transform.position;
        linkedDoor.camera.transform.rotation = rotatedLinked * invertedLocal * playerCamera.rotation;
    }

}
