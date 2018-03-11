using UnityEngine;

public class PortalCamera : MonoBehaviour {
    [SerializeField] Transform localPortal;
    [SerializeField] Transform distantPortal;
    [SerializeField] Renderer drawingPortal;
    private Transform playerCamera;
    private Camera cam;
    [SerializeField] private Material material;

    // Use this for initialization
    void Start() {
        material = Material.Instantiate(material);

        playerCamera = Camera.main.transform;
        cam = GetComponent<Camera>();
        if(cam.targetTexture != null) {
            cam.targetTexture.Release();
        }

        RenderTexture tex = new RenderTexture(Screen.width, Screen.height, 24);
        tex.antiAliasing = 1;
        cam.targetTexture = tex;
        material.mainTexture = cam.targetTexture;

        drawingPortal.material = material;
    }

    void OnPreRender() {
        UpdatePosition();
    }

    public void UpdatePosition() {
        transform.position = (Quaternion.AngleAxis(180, localPortal.up) * localPortal.rotation) * Quaternion.Inverse(distantPortal.rotation) * (playerCamera.position - distantPortal.position) + localPortal.position;
        transform.rotation = (Quaternion.AngleAxis(180, localPortal.up) * localPortal.rotation) * Quaternion.Inverse(distantPortal.rotation) * playerCamera.rotation;
    }
}
