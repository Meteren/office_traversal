using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Indicator")]
    public TextMeshProUGUI indicator;

    [Header("Selection Screen")]
    public GameObject selectionScreen;

    [Header("Info Panel")]
    public InfoPanel infoPanel;

    [Header("Text Timer")]
    [SerializeField] private float timer;

    [Header("Game State Listener")]
    [SerializeField] private EventListener gameStateListener;

    [Header("Minimap Controller")]
    [SerializeField] private MiniMapController miniMapController;

    [Header("Minimap Cam")]
    [SerializeField] private MiniMapCam miniMapCam;

    [Header("Map Deactivation Listener On Select Screen")]
    [SerializeField] private EventListener deactivationListener;

    [Header("Map Icons")]
    [SerializeField] public List<GameObject> mapIcons;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && !miniMapController.inMovement)
        {
                     
            GameManager.instance.gamePaused = !GameManager.instance.gamePaused;
            if (miniMapController.ShouldOpenMap)
            {
                miniMapCam.MiniMapCamConstantPosY = miniMapCam.transform.position.y;
                deactivationListener.eventController.ExecuteListeners();
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                gameStateListener.eventController.ExecuteListeners();
            }
           
            selectionScreen.SetActive(GameManager.instance.gamePaused);      
                
        }
       
    }

    public void ShowIndicator(string text)
    {
        StartCoroutine(Timer(text));
    }

    public IEnumerator Timer(string text)
    {
        indicator.gameObject.SetActive(true);
        indicator.text = text;
        yield return new WaitForSeconds(timer);
        indicator.gameObject.SetActive(false);
    }

}
