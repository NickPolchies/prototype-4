using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour {
    public int ID;

    private Animator animator;
    public bool open;
    public bool clockwise;
    public GameObject portal;
    [SerializeField] Door linkedDoor;

    private void Start() {
        open = false;

        animator = GetComponent<Animator>();
    }

    public void Interact() {
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (open) {
            Close();
            if (linkedDoor != null) {
                linkedDoor.Close();
            }
        }
        else {
            clockwise = false;
            Open();

            if (linkedDoor != null) {
                linkedDoor.clockwise = clockwise;
                //linkedDoorActive.linkedDoorActive = clockwise ? linkedDoorActive.linkedDoorClockwise : linkedDoorActive.linkedDoorAnticlockwise;
                //linkedDoorActive.Open();
            }
        }
    }

    void Open() {
        open = true;
/*
        if (clockwisePortal != null) {
            clockwisePortal.SetActive(clockwise);
        }
        if (anticlockwisePortal != null) {
            anticlockwisePortal.SetActive(!clockwise);
        }

        animator.SetBool("Clockwise", clockwise);
        animator.SetTrigger("Open");
        */
    }

    void Close() {
        open = false;

        animator.SetTrigger("Close");
    }
}
