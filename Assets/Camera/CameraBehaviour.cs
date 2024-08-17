using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Camera cam;

    private Vector3 mousePositionLastFrame;

    [SerializeField] private float currentZoomMultiplier;
    public float ZoomMin;
    public float ZoomMax;
    public float ZoomMinMultiplier;
    public float ZoomMaxMultiplier;

    public Vector2 CameraBounds_X;
    public Vector2 CameraBounds_Y;

    public bool DrawDebug = false;

    void Start()
    {
        cam = Camera.main;

        mousePositionLastFrame = Input.mousePosition;
    }

    void LateUpdate()
    {
        DoPan();
        DoZoom();
        DoCameraBounds();
        
        mousePositionLastFrame = Input.mousePosition;
    }

    private void DoPan()
    {
        // fixes camera flying when mouse is offscreen and comes back (because mouse pos isn't updated when unfocused)
        if (Input.GetMouseButtonDown(2)) { mousePositionLastFrame = Input.mousePosition; }

        // we use world space so that the mouse stays "at the same point" when dragging
        Vector3 worldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 worldMousePositionLastFrame = cam.ScreenToWorldPoint(mousePositionLastFrame);
        // based on mouse delta
        Vector3 panning = new Vector3(worldMousePositionLastFrame.x - worldMousePosition.x,
                                       worldMousePositionLastFrame.y - worldMousePosition.y,
                                       0.0f);
        
        // move camera if button pressed
        if (Input.GetMouseButton(2))
        {
            transform.Translate(panning);
        }
    }

    private void DoZoom()
    {
        // how zoomed in we are from 0-1
        // f\left(x\right)=\frac{x-a}{b-a}
        float zoomProgress = (cam.orthographicSize - ZoomMin) / (ZoomMax - ZoomMin);
        // lerp between zoom speeds based on how zoomed in we are
        currentZoomMultiplier = Mathf.Lerp(ZoomMinMultiplier, ZoomMaxMultiplier, zoomProgress);

        // calculate amount to zoom by
        float zooming = Input.mouseScrollDelta.y * currentZoomMultiplier;
        // do the zoom
        cam.orthographicSize -= zooming;

        // clamp min and max
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, ZoomMin, ZoomMax);
    }

    private void DoCameraBounds()
    {
        float yFOV = cam.orthographicSize;
        float xFOV = yFOV * cam.aspect;

        transform.position = new Vector3(x: Mathf.Clamp(transform.position.x, CameraBounds_X.x+xFOV, CameraBounds_X.y-xFOV),
                                         y: Mathf.Clamp(transform.position.y, CameraBounds_Y.x+yFOV, CameraBounds_Y.y-yFOV),
                                         z: transform.position.z);
        
        if (DrawDebug)
        {
            Debug.DrawLine(start: new Vector3(CameraBounds_X.x, CameraBounds_Y.x), // top
                             end: new Vector3(CameraBounds_X.y, CameraBounds_Y.x), Color.red);
            Debug.DrawLine(start: new Vector3(CameraBounds_X.x, CameraBounds_Y.y), // bottom
                             end: new Vector3(CameraBounds_X.y, CameraBounds_Y.y), Color.red);
            Debug.DrawLine(start: new Vector3(CameraBounds_X.x, CameraBounds_Y.x), // left
                             end: new Vector3(CameraBounds_X.x, CameraBounds_Y.y), Color.red);
            Debug.DrawLine(start: new Vector3(CameraBounds_X.y, CameraBounds_Y.x), // right
                             end: new Vector3(CameraBounds_X.y, CameraBounds_Y.y), Color.red);
        }
    }
}
