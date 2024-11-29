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
    }

    private void Move(float horizontal)
    {
        Vector3 force = speed * Time.deltaTime * horizontal * rb.transform.TransformDirection(Vector3.forward);
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    private void Steer(float horizontal, float vertical, float roll)
    {
        float deltaRotationSpeed = rotationSpeed * Time.deltaTime;

        Vector3 rotationX = deltaRotationSpeed * horizontal * rb.transform.up;
        Vector3 rotationY = deltaRotationSpeed * -vertical * rb.transform.right;
        Vector3 rotationZ = deltaRotationSpeed * roll * rb.transform.forward;

        rb.AddTorque(rotationX + rotationY + rotationZ, ForceMode.VelocityChange);
    }
}
