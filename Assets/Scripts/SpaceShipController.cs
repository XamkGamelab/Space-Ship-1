using System.Collections;
using UnityEngine;

// https://www.youtube.com/watch?v=hXColAuMx-I

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxLinearVelocity;
    [SerializeField] private float maxAngularVelocity;

    private InputSystemActions.SpaceshipActions actions;
    private Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        actions = new InputSystemActions().Spaceship;
        actions.Enable();

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!actions.enabled)
            return;

        float horizontal = actions.Horizontal.ReadValue<float>();
        float vertical = actions.Vertical.ReadValue<float>();
        Move(horizontal);

        float steerX = Input.GetAxis("Mouse X");
        float steerY = Input.GetAxis("Mouse Y");
        Steer(steerX, steerY, vertical);

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxLinearVelocity);
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxAngularVelocity);

        Debug.Log($"{rb.linearVelocity} {rb.angularVelocity}");

        if (Input.GetKeyDown(KeyCode.Q))
            Stabilize();
    }

    // Stabilize ship
    private void Stabilize()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearDamping = 5.0f;
        Invoke(nameof(ResetDamping), 2.0f);
    }

    void ResetDamping()
    {
        rb.linearDamping = 0.0f;
    }

    private void Move(float horizontal)
    {
        if (horizontal == 0.0f)
            return;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 force = speed * Time.deltaTime * horizontal * forward;

        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, force, 1.0f);
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    private void Steer(float horizontal, float vertical, float roll)
    {
        float deltaRotationSpeed = rotationSpeed * Time.deltaTime;

        Vector3 x = -vertical  * transform.TransformDirection(Vector3.right);
        Vector3 y = horizontal * transform.TransformDirection(Vector3.up);
        Vector3 z = roll       * transform.TransformDirection(Vector3.forward);
        
        Vector3 rotation = x + y + z;

        rb.AddTorque(deltaRotationSpeed * rotation, ForceMode.VelocityChange);
    }
}
