using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [SerializeField] private float sensitivity;

    [Header("X-Axis Boundaries")]
    [SerializeField] private float upperBoundary;
    [SerializeField] private float lowerBoundary;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;

    [Header("Conditions")]
    [SerializeField] private bool canCameraMove;

    [Header("Game State Listener")]
    [SerializeField] private EventListener gameStateListener;

    [Header("Map Deactivation Listener On Select Screen")]
    [SerializeField] private EventListener deactivationListener;

    [Header("Sensitivity Controller")]
    [SerializeField] private Slider sSlider;

    [Header("Mouse Delta Adjuster")]
    [SerializeField] private Slider dAdjuster;

    float xRotation;
    float yRotation;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        miniMapEventListener.AddEvent(HandleCamera);
        gameStateListener.AddEvent(HandleCamera);

    }

    void Update()
    {

        sSlider.gameObject.SetActive(GameManager.instance.gamePaused);
        sensitivity = sSlider.value;
        if (canCameraMove)
        {
            if (!GameManager.instance.gamePaused)
            {


#if UNITY_WEBGL
                float deltaX = Input.GetAxisRaw("Mouse X");
                float deltaY = Input.GetAxisRaw("Mouse Y");

                deltaX = Mathf.Clamp(deltaX,-2,2);
                deltaY = Mathf.Clamp(deltaY, -2, 2);

                xRotation += -1 * deltaY * sensitivity;
                xRotation = ClampXAxis(xRotation);
                yRotation += deltaX * sensitivity;

                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

                player.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
#else

                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
      
                xRotation += -1 * mouseDelta.y * sensitivity;
                xRotation = ClampXAxis(xRotation);

                yRotation += mouseDelta.x * sensitivity;

                transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

                player.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
#endif

            }
        }
    }

    private float ClampXAxis(float x)
    {
       return  Mathf.Clamp(x, lowerBoundary, upperBoundary);
    }

    public void HandleCamera() => canCameraMove = !canCameraMove;

    public void SetCameraRotation(float x, float y)
    {
        xRotation = x;
        yRotation = y;
    }

}
