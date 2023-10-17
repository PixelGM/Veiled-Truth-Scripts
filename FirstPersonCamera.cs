using Invector;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    #region inspector properties    

    public Transform target;
    public float smoothCameraRotation = 12f;
    public LayerMask cullingLayer = 1 << 0;

    public float xMouseSensitivity = 3f;
    public float yMouseSensitivity = 3f;
    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    public bool cursorLock = true;

    #endregion

    #region hide properties    

    private float mouseY = 0f;
    private float mouseX = 0f;
    private float currentHeight;
    private float forward = -1f;
    private float xMinLimit = -360f;
    private float xMaxLimit = 360f;

    #endregion

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
        mouseY = target.rotation.eulerAngles.x;
        mouseX = target.rotation.eulerAngles.y;
    }

    void Update()
    {
        RotateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public void RotateCamera(float x, float y)
    {
        mouseX += x * xMouseSensitivity;
        mouseY -= y * yMouseSensitivity;

        mouseY = vExtensions.ClampAngle(mouseY, yMinLimit, yMaxLimit);
        mouseX = vExtensions.ClampAngle(mouseX, xMinLimit, xMaxLimit);

        Quaternion newRot = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, smoothCameraRotation * Time.deltaTime);
    }
}