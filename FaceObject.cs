using UnityEngine;

public class FaceObject : MonoBehaviour
{
    public Transform target;
    public Vector3 rotationOffset;

    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition);

        // apply the rotation offsets
        transform.rotation *= Quaternion.Euler(rotationOffset);
    }
}
