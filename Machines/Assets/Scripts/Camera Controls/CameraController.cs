using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 min;
    public Vector3 max;

    // Gizmos variables
    private Vector3 center;
    private Vector3 size;

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        BoundsCheck();
    }

    /// <summary>
    /// Performs a bounds check on the camera and sets its position if outside bounds
    /// </summary>
    private void BoundsCheck()
    {
        Vector3 pos = transform.position;

        if(pos.x < min.x)
        {
            pos.x = min.x;
        }
        if(pos.y < min.y)
        {
            pos.y = min.y;
        }
        if(pos.z < min.z)
        {
            pos.z = min.z;
        }

        if(pos.x > max.x)
        {
            pos.x = max.x;
        }
        if(pos.y > max.y)
        {
            pos.y = max.y;
        }
        if(pos.z > max.z)
        {
            pos.z = max.z;
        }

        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        center = min + (max - min) / 2f;
        size = max - min;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(center, size);
    }
}
