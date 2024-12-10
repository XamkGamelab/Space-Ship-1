using System.Collections;
using UnityEngine;

// https://www.youtube.com/watch?v=hXColAuMx-I

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxLinearVelocity;
    [SerializeField] private float maxAngularVelocity;

    [SerializeField] private float stabilizationDuration;

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
            StartCoroutine(Stabilize());
    }

    // Stabilize ship
    private IEnumerator Stabilize()
    {
        float elapsed = 0.0f;

        Vector3 targetLinearVelocity = rb.linearVelocity / 2.0f;
        Vector3 targetAngularVelocity = Vector3.zero;

        while (stabilizationDuration > elapsed)
        {
            float deltaDuration = elapsed / stabilizationDuration;
            elapsed += Time.deltaTime;

            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, targetAngularVelocity, deltaDuration);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetLinearVelocity, deltaDuration);

            if (rb.angularVelocity == targetAngularVelocity || rb.linearVelocity == targetLinearVelocity)
                yield break;

            yield return null;
        }
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
        Vector3 rotationY = deltaRotationSpeed * -vertical  * rb.transform.right;
        Vector3 rotationZ = deltaRotationSpeed * roll       * rb.transform.forward;

        rb.AddTorque(rotationX + rotationY + rotationZ, ForceMode.VelocityChange);
    }
}
