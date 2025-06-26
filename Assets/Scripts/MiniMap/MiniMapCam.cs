using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField] private PlayerController pController;

    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;

    [Header("Conditions")]
    [SerializeField] private bool miniMapEnabled;
    [SerializeField] private bool init;

    [Header("Constant Position")]
    [SerializeField] private float miniMapCamConstantPosY;

    public float MiniMapCamConstantPosY { get { return miniMapCamConstantPosY; } set {  miniMapCamConstantPosY = value; } }

    private void Start()
    {
        miniMapEventListener.AddEvent(HandleMiniMapCondition);
        Camera cam = GetComponent<Camera>();
        cam.transparencySortMode = TransparencySortMode.Orthographic;
    }
    private void Update()
    {
        if (!miniMapEnabled)
        {
            if (init)
            {
                init = false;
                miniMapCamConstantPosY -= pController.transform.position.y;
            }
            transform.position = 
                new Vector3(pController.transform.position.x, 
                miniMapCamConstantPosY + pController.transform.position.y, pController.transform.position.z);
        }

               
    }

    private void HandleMiniMapCondition()
    {
        miniMapEnabled = !miniMapEnabled;
        if(!init)
            init = true;
    } 
}
