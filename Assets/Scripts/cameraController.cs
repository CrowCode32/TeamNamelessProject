using JetBrains.Annotations;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] public float sens = 500f;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;

        if (invertY)
        {
            rotX += mouseY;
        } else
        {
            rotX -= mouseY;
        }

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //Rotate the camera to look up/down
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //Rotate the player to look left/right
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
