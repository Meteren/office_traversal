using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour
{
    [Header("Player Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("Player Controller")]
    [SerializeField] private PlayerController playerController;

    [Header("Buildings")]
    [SerializeField] private List<GameObject> buildings;
    private int selectedBuildingIndex;
    [Header("Buttons")]
    [SerializeField] private List<Button> buttons;

    [HideInInspector] public GameObject currentBuilding;

    [Header("Time To Wait For Transition")]
    [SerializeField] private float levelLoadTime;
    [SerializeField] private float buttonActivationTime;

    [Header("Transition Controller")]
    [SerializeField] private Animator transitAnim;

    [Header("Minimap Renderer Controller")]
    [SerializeField] private MinimapCamRenderController miniMapCamRendererController;

    [Header("Camera Movement")]
    [SerializeField] private CameraMovement cameraMovement;

    [HideInInspector] public bool inittedLevelChange;
    
    public List<GameObject> Buildings {  get { return buildings; } }


    private void Update()
    {
        SetButtons();
        
    }
    public void SelectBuilding(int index)
    {
        selectedBuildingIndex = index;

        for(int i = 0; i < buildings.Count; i++)
        {
            if(selectedBuildingIndex == i)
            {
                SetBuilding(buildings[i]);
            }
        }
            
    }
    private void SetButtons()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if (i == selectedBuildingIndex)
                buttons[i].GetComponent<Image>().color = Color.red;
            else
                buttons[i].GetComponent<Image>().color = Color.white;
        }
    }

    public void SetBuilding(GameObject building)
    {
        
        if (GameManager.instance.isStarted)
        {
            StartCoroutine(TransitScenes(building));
            return;
        }
        Process(building);
        
    }
    public void ClearBuilding()
    {
        LevelData levelData = currentBuilding.GetComponent<LevelData>();
        miniMapCamRendererController.RemoveObjectsToBeEffected(levelData.levelOccludables);
        Destroy(currentBuilding);
    }

    public IEnumerator TransitScenes(GameObject building)
    {
        HandleButtons(false);
        transitAnim.SetBool("transit", true);
        inittedLevelChange = true;
        yield return new WaitForSeconds(levelLoadTime);
        ClearBuilding();
        Process(building);
        yield return new WaitForSeconds(levelLoadTime);
        transitAnim.SetBool("transit",false);
        yield return new WaitForSeconds(buttonActivationTime);
        inittedLevelChange = false;
        HandleButtons(true);

    }

    private void Process(GameObject building)
    {
        GameObject settedBuilding = Instantiate(building, Vector3.zero, Quaternion.identity);
        Transform spawnTransform = settedBuilding.transform.Find("SpawnPoint");
        LevelData levelData = settedBuilding.GetComponent<LevelData>();
        miniMapCamRendererController.AddObjectsToBeEffected(levelData.levelOccludables);
        playerController.rb.position = spawnTransform.position;
        playerCamera.transform.localRotation = Quaternion.Euler(0,0,0);
        cameraMovement.SetCameraRotation(0,0);
        currentBuilding = settedBuilding;
    }
    private void HandleButtons(bool shouldActivate)
    {
        foreach(var b in buttons)
        {
            if (shouldActivate)
                b.enabled = true;
            else
                b.enabled = false;  
        }
    }
}
