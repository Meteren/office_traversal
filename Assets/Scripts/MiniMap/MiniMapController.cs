using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapController : MonoBehaviour, IPointerDownHandler
{
    RectTransform minimapRect;
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

    [Header("Minimap Scroll Adjustments")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float minSValue;
    [SerializeField] private float maxSValue;

    [Header("Minimap Traversal Adjustments")]
    [SerializeField] private float traversalSpeed;
    [SerializeField] private Vector2 xBoundaries;
    [SerializeField] private Vector2 zBoundaries;
    [SerializeField] private BoxCollider boundary;
    void Start()
    {
        miniMapEventListener.AddEvent(OnActivation);
        minimapRect = GetComponent<RectTransform>();
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

        if (shouldOpenMap)
        {
            AdjustMinimapScroll();

            if (IsInsideMinimapArea())
            {
                if (Input.GetMouseButton(2))
                {
                    AdjustMinimapMovement();
                }
            }

        }

        
    }

    private void AdjustMinimapScroll()
    {
        float delta = Input.GetAxis("Mouse ScrollWheel");

        if (delta != 0)
        {
            miniMapCam.fieldOfView -= delta * scrollSpeed;
            miniMapCam.fieldOfView = Mathf.Clamp(miniMapCam.fieldOfView, minSValue, maxSValue);
        }
       
    }

    private void AdjustMinimapMovement()
    {
        float directionX = Input.GetAxis("Mouse X");
        float directionZ = Input.GetAxis("Mouse Y");

        Vector3 miniMapCamPos = miniMapCam.transform.position;

        Vector3 movementVector = new Vector3(miniMapCamPos.x -= directionX * traversalSpeed, 
            miniMapCamPos.y, miniMapCamPos.z -= directionZ * traversalSpeed);

        Vector3 boundaryCenter = boundary.transform.position;
        Debug.Log($"Boundary Center Position:{boundaryCenter}");
        Vector3 bounds = boundary.size;
        Vector2 xAxis = new Vector2(boundaryCenter.x + bounds.x / 2, boundaryCenter.x - bounds.x / 2);
        Vector2 zAxis = new Vector2(boundaryCenter.z + bounds.z / 2, boundaryCenter.z - bounds.z / 2);

        movementVector.x = Mathf.Clamp(movementVector.x, xAxis.y, xAxis.x);
        movementVector.z = Mathf.Clamp(movementVector.z, zAxis.y, zAxis.x);

        miniMapCam.transform.position = movementVector;
    }

    private bool IsInsideMinimapArea()
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect,Input.mousePosition,null,out Vector2 localPoint))
            if (minimapRect.rect.Contains(localPoint))
                return true;     
            else return false;
        else return false;  
    }

    private void OnActivation()
    {
        Cursor.lockState = shouldOpenMap ? CursorLockMode.Locked : CursorLockMode.None;
        shouldOpenMap = !shouldOpenMap;
    }

    public void OnPointerDown(PointerEventData eventData)
    {    
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect, eventData.position, null, out localPoint);
        Debug.Log($"Clicked: {localPoint}");

        Vector2 clickPosition = GetMinimapCamClickPos(localPoint);

        Debug.Log($"Click Position:{clickPosition}");

        if (Input.GetMouseButtonDown(0)) 
        {
            if (GetPositionToMoveIfPossible(clickPosition, out Vector3 positionToMove))
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
       
       
    }

    private Vector2 GetMinimapCamClickPos(Vector2 point)
    {

        Vector2 normalizedPoint = Rect.PointToNormalized(minimapRect.rect, point);

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
