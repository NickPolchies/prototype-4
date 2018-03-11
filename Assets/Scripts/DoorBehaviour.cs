using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorBehaviour : MonoBehaviour {
    public string ID;

    private Animator animator;
    public bool open;
    public bool clockwise;
    public GameObject clockwisePortal;
    public GameObject anticlockwisePortal;
    [SerializeField] DoorBehaviour linkedDoorClockwise;
    [SerializeField] DoorBehaviour linkedDoorAnticlockwise;
    public DoorBehaviour linkedDoorActive;

    private void Start() {
        open = false;

        animator = GetComponent<Animator>();
    }

    public void Interact() {
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (open) {
            Close();
            Debug.Log("Close local");
            if (linkedDoorActive != null) {
                Debug.Log("Linked not null: " + linkedDoorActive.ID);
                linkedDoorActive.Close();
            }
        }
        else {
            clockwise = Vector3.Dot(player.position - transform.position, transform.right) > 0;
            linkedDoorActive = clockwise ? linkedDoorClockwise : linkedDoorAnticlockwise;

            Open();

            if(linkedDoorActive != null) {
                linkedDoorActive.clockwise = clockwise;
                linkedDoorActive.linkedDoorActive = clockwise ? linkedDoorActive.linkedDoorClockwise : linkedDoorActive.linkedDoorAnticlockwise;
                linkedDoorActive.Open();
            }
        }
    }

    void Open() {
        open = true;

        if (clockwisePortal != null) {
            clockwisePortal.SetActive(clockwise);
        }
        if (anticlockwisePortal != null) {
            anticlockwisePortal.SetActive(!clockwise);
        }

        animator.SetBool("Clockwise", clockwise);
        animator.SetTrigger("Open");
    }

    void Close() {
        Debug.Log("Close Function");
        open = false;

        animator.SetTrigger("Close");
    }
}
