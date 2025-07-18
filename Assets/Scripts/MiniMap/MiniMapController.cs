using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour, IPointerDownHandler
{
    RectTransform minimapRect;
    Animator miniMapAnim;
    int buttonSelected;

    [Header("Conditions")]
    public bool shouldOpenMap;
    public bool inMovement;
    [SerializeField] private bool initUI;

    [Header("Minimap Cam")]
    [SerializeField] private Camera miniMapCam;

    [Header("Hit Mask")]
    [SerializeField] private LayerMask layersToHit;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;

    [Header("Map Deactivation Listener On Select Screen")]
    [SerializeField] private EventListener deactivationListener;


    [Header("Minimap Scroll Adjustments")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float minSValue;
    [SerializeField] private float maxSValue;

    private float offsettedMinSValue;
    private float offsettedMaxSValue;

    [Header("Minimap Traversal Adjustments")]
    [SerializeField] private float traversalSpeed;
    [SerializeField] private Vector2 xBoundaries;
    [SerializeField] private Vector2 zBoundaries;
    [SerializeField] private BoxCollider boundary;

    [Header("Ray Adjustments")]
    [SerializeField] private float rayOffset;
    [SerializeField] private LayerMask rayLayer;

    [Header("Floor Control Section")]
    [SerializeField] private List<Button> floorButtons;
    public int floorIndex;


    public bool ShouldOpenMap { get { return shouldOpenMap; } }

    void Start()
    {
        miniMapEventListener.AddEvent(ControlMinimapActivation);
        deactivationListener.AddEvent(ControlMinimapActivation);
        minimapRect = GetComponent<RectTransform>();
        miniMapAnim = GetComponent<Animator>(); 
    }

    private void Update()
    {
        SetAnimator();
        SetZoomBoundaries();

        if (Input.GetKeyDown(KeyCode.Tab) && !inMovement && !GameManager.instance.gamePaused)
        {
            initUI = true;
            inMovement = true;
            MiniMapCam miniMC = miniMapCam.GetComponent<MiniMapCam>();
            miniMC.MiniMapCamConstantPosY = miniMapCam.transform.position.y;    
            miniMapEventListener.eventController.ExecuteListeners();
            UIManager.instance.infoPanel.gameObject.SetActive(false);
        }

        AnimatorStateInfo stateInfo = miniMapAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("up") && shouldOpenMap)
            if (stateInfo.normalizedTime >= 1)
                inMovement = false;

        if (stateInfo.IsName("down") && !shouldOpenMap)
            if (stateInfo.normalizedTime >= 1)
                inMovement = false;

        SetButtonsState(shouldOpenMap);

        foreach (var element in UIManager.instance.mapIcons)
            element.gameObject.SetActive(shouldOpenMap);

        if (shouldOpenMap)
        {
            if (initUI)
            {
                buttonSelected = floorIndex;
                SetMiniMapState(buttonSelected);
                initUI = false;
            }

            if (!GameManager.instance.gamePaused)
            {
                AdjustMinimapScroll();
                SetFloorUI();

                if (IsInsideMinimapArea())
                {
                    if (Input.GetMouseButton(1))
                    {
                        AdjustMinimapMovement();
                    }
                }
            }
           
            
        }      
     
    }
    private void SetZoomBoundaries()
    {
        offsettedMinSValue = minSValue + playerController.transform.position.y;
        offsettedMaxSValue = maxSValue + playerController.transform.position.y;
    }

    private void AdjustMinimapScroll()
    {
        float delta = Input.GetAxis("Mouse ScrollWheel");

        if (delta != 0)
        {
            Vector3 miniMapCamPos = miniMapCam.transform.position;
            miniMapCamPos.y -= delta * scrollSpeed;
            miniMapCamPos.y = Mathf.Clamp(miniMapCamPos.y, offsettedMinSValue, offsettedMaxSValue);
            miniMapCam.transform.position = miniMapCamPos;
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

    public void ControlMinimapActivation()
    {
 
        Cursor.lockState = shouldOpenMap ? CursorLockMode.Locked : CursorLockMode.None;
        shouldOpenMap = !shouldOpenMap;
        if (!shouldOpenMap)
            SetLayersToDefault();
    }

    public void OnPointerDown(PointerEventData eventData)
    {    
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect, eventData.position, null, out localPoint);

        Vector2 clickPosition = GetMinimapCamClickPos(localPoint);

        if (Input.GetMouseButtonDown(0)) 
        {
            if (GetPositionToMoveIfPossible(clickPosition, out Vector3 positionToMove))
            {
                float bodyHeight = playerController.body.GetComponent<Collider>().bounds.size.y / 2;
                float playerYOffset = bodyHeight + (playerController.transform.position.y - playerController.body.position.y);
                Vector3 position = new Vector3(positionToMove.x, positionToMove.y + playerYOffset, positionToMove.z);

                MoveAwayFromObstaclesIfNeeded(position,out Vector3 finalPosition);

                playerController.rb.position = finalPosition;
                SetPlayerLayer("Player");

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

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layersToHit, QueryTriggerInteraction.Collide))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                positionToMove = hit.point;
                return true;
            }
                
        }
        positionToMove = default;
        return false;
    }

    private void MoveAwayFromObstaclesIfNeeded(Vector3 positionBefore,out Vector3 finalPosition)
    {
        List<Vector3> directions = new List<Vector3>();
        float radius = playerController.GetComponentInChildren<CapsuleCollider>().radius;

        Vector3 xAxisPos = new Vector3(positionBefore.x + radius + rayOffset, positionBefore.y, positionBefore.z);
        Vector3 zAxisPos = new Vector3(positionBefore.x, positionBefore.y, positionBefore.z + radius + rayOffset);

        Vector3 directionX = (xAxisPos - positionBefore);
        Vector3 directionZ = (zAxisPos - positionBefore);

        directions.Add(directionX);
        directions.Add(directionX * -1);
        directions.Add(directionZ);
        directions.Add(directionZ * -1);

        finalPosition = positionBefore; 

        foreach(var direction in directions)
        {
            Ray ray = new Ray(positionBefore, direction);
            VisualizeRay(positionBefore, direction);
            if (Physics.Raycast(ray,out RaycastHit hit, radius, rayLayer, QueryTriggerInteraction.Ignore))
            {
                float delta = radius - hit.distance;
                if(direction.z == 0)
                {
                    finalPosition -= new Vector3(delta * (direction.x > 0 ? 1 : -1), 0, 0);
                }                 
                                            
                if(direction.x == 0)
                {
                    finalPosition -= new Vector3(0, 0, delta * (direction.z > 0 ? 1 : -1));
                }
                    
            }
        }

    }

    private void VisualizeRay(Vector3 origin, Vector3 direction) => Debug.DrawRay(origin, direction, Color.red, 20f);


    private void SetAnimator()
    {
        miniMapAnim.SetBool("shouldOpenMap", shouldOpenMap);
    }

    private void SetButtonsState(bool state)
    {
        foreach(var button in floorButtons)
            button.gameObject.SetActive(state);

    }

    private void SetFloorUI()
    {
        
        for (int i = 0; i < floorButtons.Count; i++)
        {
            Image buttonImage = floorButtons[i].GetComponent<Image>();
            if (i == buttonSelected)
                buttonImage.color = Color.red;
            else
                buttonImage.color = Color.white;
        }             
        
    }

    public void SetMiniMapState(int index)
    {
        buttonSelected = index;
        LevelData currentLevelData = GameObject.FindAnyObjectByType<LevelData>();

        if (buttonSelected == floorIndex)
            SetPlayerLayer("Player");
        else
            SetPlayerLayer("Hidden");

        for (int i = 0; i <= index; i++)
        {
            GameObject floor = currentLevelData.levelOccludables[i];
            floor.layer = LayerMask.NameToLayer("Default");
            for (int j = 0; j < floor.transform.childCount; j++ )
            {
                GameObject child = floor.transform.GetChild(j).gameObject;

                MaterialProvider childProvider = child.GetComponent<MaterialProvider>();
                OccludableObject occludableReference = child.GetComponent<OccludableObject>();

                if (childProvider != null)
                    if(childProvider.isFloor)
                        child.gameObject.layer = LayerMask.NameToLayer("Floor");

                if (occludableReference != null)
                {
                    if(occludableReference.gameObject.layer != LayerMask.NameToLayer("Furniture"))
                    {
                        if (occludableReference.isPart)
                            child.gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                    if(occludableReference.gameObject.layer == LayerMask.NameToLayer("Furniture"))
                        occludableReference.SetState(true);

                           
                }       
                
            }
        }

        for (int i = index + 1; i < currentLevelData.levelOccludables.Count; i++)
        {
            GameObject levelOccludable = currentLevelData.levelOccludables[i];
            MaterialProvider floorProvider = levelOccludable.GetComponent<MaterialProvider>();
            
            if(floorProvider != null)
                if(floorProvider.isFloor)
                    levelOccludable.layer = LayerMask.NameToLayer("Hidden");

            for (int j = 0; j < levelOccludable.transform.childCount; j++)
            {
                GameObject child = levelOccludable.transform.GetChild(j).gameObject;
                MaterialProvider childProvider = child.GetComponent<MaterialProvider>();
                OccludableObject occludableReference = child.GetComponent<OccludableObject>();
                if (childProvider != null)
                    if (childProvider.isFloor)
                        child.gameObject.layer = LayerMask.NameToLayer("Hidden");

                if(occludableReference != null)
                {
                    if (occludableReference.gameObject.layer != LayerMask.NameToLayer("Furniture"))
                    {
                        if (occludableReference.isPart)
                            child.gameObject.layer = LayerMask.NameToLayer("Hidden");
                    }
                    if (occludableReference.gameObject.layer == LayerMask.NameToLayer("Furniture"))
                    {
                        occludableReference.SetState(false);
                    }
                        
                }
                    
            }
        }
      
    }

    public void SetLayersToDefault()
    {
        LevelData currentLevelData = GameObject.FindAnyObjectByType<LevelData>();
        SetPlayerLayer("Player");
        for (int i = 0; i < currentLevelData.levelOccludables.Count; i++)
        {
            GameObject levelOccludable = currentLevelData.levelOccludables[i];
            MaterialProvider floorProvider = levelOccludable.GetComponent<MaterialProvider>();
            if (floorProvider != null)
                if (floorProvider.isFloor)
                    levelOccludable.layer = LayerMask.NameToLayer("Default");

            for (int j = 0; j < levelOccludable.transform.childCount; j++)
            {
                GameObject child = levelOccludable.transform.GetChild(j).gameObject;
                MaterialProvider childProvider = child.GetComponent<MaterialProvider>();
                if(childProvider != null)
                    if(childProvider.isFloor)
                        child.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void SetPlayerLayer(string layer) => playerController.body.gameObject.layer = LayerMask.NameToLayer(layer);

}
