using UnityEngine;

[ExecuteInEditMode]
public class CameraScript : MonoBehaviour
{
    #region References
    //references
    [SerializeField] private Transform camTarget;
    private Camera cam;
    #endregion

    #region Settings
    //position settings
    [Header("Position Settings")]
    [SerializeField] private float smoothen;
    [SerializeField] private Vector3 offset;

    //zoom settings
    [Header("Zoom"), SerializeField]
    private float zoom;
    [SerializeField]private float zoomMax, zoomMin;
    #endregion

    #region Monobehaviour Methods
    private void OnValidate()
    {
        if (cam == null)
        { cam = GetComponent<Camera>(); }
        if (camTarget == null)
        { camTarget = GameObject.FindGameObjectWithTag("Player")?.transform; }
        // ? operator seems to be avoiding NullReference Exeption that occurs when the CameraScript loaded before Player
    }

    private void Awake()
    {
        //cam = GetComponent<Camera>();
        //camTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void LateUpdate()
    {
        Vector3 ultimatePos = camTarget.position + offset;
        Vector3 processedPos = Vector3.Lerp(transform.position.normalized, ultimatePos, smoothen);
        transform.position = processedPos;

    }
    #endregion

    #region Zoom
    /// <summary>
    /// Handles HandleZoom
    /// </summary>
    private void HandleZoom()
    {
        zoom -= Input.GetAxis("Mouse ScrollWheel");
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        cam.orthographicSize = zoom;
    }
    #endregion
}
