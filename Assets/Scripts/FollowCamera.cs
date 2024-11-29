using UnityEngine;

[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    // https://stackoverflow.com/questions/65816546/unity-camera-follows-player-script
    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + target.rotation * positionOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);

        Quaternion targetRotation = target.rotation * Quaternion.Euler(rotationOffset);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed);
    }
}
