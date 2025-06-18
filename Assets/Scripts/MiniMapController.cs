using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapController : MonoBehaviour, IPointerDownHandler
{
    RectTransform rect;
    Animator miniMapAnim;

    [Header("Conditions")]
    [SerializeField] private bool shouldOpenMap;
    [SerializeField] private bool inMovement;

    [Header("Minimap Cam")]
    [SerializeField] private Camera miniMapCam;
    [Header("Hit Mask")]
    [SerializeField] private LayerMask layersToHit;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;
    void Start()
    {
        miniMapEventListener.AddEvent(OnActivation);
        rect = GetComponent<RectTransform>();
        miniMapAnim = GetComponent<Animator>(); 
    }

    private void Update()
    {
        SetAnimator();

        if (Input.GetKeyDown(KeyCode.Tab) && !inMovement)
        {
            inMovement = true;
            miniMapEventListener.eventController.ExecuteListeners();
        }

        AnimatorStateInfo stateInfo = miniMapAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("up") && shouldOpenMap)
            if (stateInfo.normalizedTime >= 1)
                inMovement = false;

        if (stateInfo.IsName("down") && !shouldOpenMap)
            if (stateInfo.normalizedTime >= 1)
                inMovement = false;
    }

    private void OnActivation()
    {
        Cursor.lockState = shouldOpenMap ? CursorLockMode.Locked : CursorLockMode.None;
        shouldOpenMap = !shouldOpenMap;
    }

    public void OnPointerDown(PointerEventData eventData)
    {    
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, null, out localPoint);
        Debug.Log($"Clicked: {localPoint}");

        Vector2 clickPosition = GetMinimapCamClickPos(localPoint);

        Debug.Log($"Click Position:{clickPosition}");

        if(GetPositionToMoveIfPossible(clickPosition,out Vector3 positionToMove))
        {
            Vector3 finalPos = new Vector3(positionToMove.x, positionToMove.y + playerController.body.GetComponent<Collider>().bounds.size.y / 2, positionToMove.z);
            Debug.Log($"Final pos: {finalPos}");

            GameObject go = new GameObject("Hit Point");
            go.transform.position = finalPos;

            playerController.rb.position = new Vector3(positionToMove.x, positionToMove.y + playerController.body.GetComponent<Collider>().bounds.size.y / 2, positionToMove.z);

        }
        else
        {
            UIManager.instance.ShowIndicator("Bu bölgeye gidilemiyor.");
        }
       
    }

    private Vector2 GetMinimapCamClickPos(Vector2 point)
    {
        Vector2 normalizedPoint = Rect.PointToNormalized(rect.rect, point);

        Vector2 clickPosition = new Vector2(normalizedPoint.x * miniMapCam.targetTexture.width, 
            normalizedPoint.y * miniMapCam.targetTexture.height);

        return clickPosition;
    }

    private bool GetPositionToMoveIfPossible(Vector2 clickPos,out Vector3 positionToMove)
    {
        Ray ray = miniMapCam.ScreenPointToRay(clickPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layersToHit, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"Ray hitted on position:{hit.point}");
            positionToMove = hit.point;
            return true;
        }
        positionToMove = default;
        return false;

    }

    private void SetAnimator()
    {
        miniMapAnim.SetBool("shouldOpenMap", shouldOpenMap);
    }
}
