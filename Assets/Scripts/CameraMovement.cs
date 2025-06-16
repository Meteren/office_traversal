using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [SerializeField] private float sensitivity;

    [Header("X-Axis Boundaries")]
    [SerializeField] private float upperBoundary;
    [SerializeField] private float lowerBoundary;

    [Header("Player")]
    [SerializeField] private Transform player;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
       
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");

        Debug.Log($"X-Axis:{xAxis} - Y-Axis:{yAxis}");

        xRotation += - 1 * yAxis * sensitivity;

        Debug.Log($"x angle:{xRotation}");

        xRotation = ClampXAxis(xRotation);

        yRotation += xAxis * sensitivity;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation,0);

        player.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
    }

    private float ClampXAxis(float x)
    {
       return  Mathf.Clamp(x, lowerBoundary, upperBoundary);
    }
}
