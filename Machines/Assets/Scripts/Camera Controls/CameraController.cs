using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // Motion variables
    [SerializeField] private float speed;
    [SerializeField] private float speedMult = 0.2f;

    private Vector2 travel;

    // Bounds variables
    [SerializeField] private Vector3 min;
    [SerializeField] private Vector3 max;

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
        if (travel.magnitude > 0.01f)
        {
            float delta = Time.deltaTime;
            speedMult = (speedMult + delta);
            speedMult = Mathf.Clamp(speedMult, 0.0f, 1.0f);
            transform.position = new Vector3(transform.position.x + (travel.x * delta * speedMult), transform.position.y, transform.position.z + (travel.y * delta * speedMult));
        }
        else
        {
            speedMult -= Time.deltaTime * 3f;
            speedMult = Mathf.Clamp(speedMult, 0.0f, 1.0f);
        }
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

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        travel.x = direction.y * speed;
        travel.y = direction.x * -speed;
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<float>();
        Vector3 v = new Vector3(0, -scroll * 0.5f, 0);
        transform.position += v;
    }
}
