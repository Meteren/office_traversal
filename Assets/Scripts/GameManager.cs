using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("Game State")]
    public bool gamePaused;

    [Header("Event Listener")]
    [SerializeField] private EventListener gameStateListener;

    [Header("Selection Screen")]
    [SerializeField] private SelectionScreen selectionScreen;

    [HideInInspector] public bool isStarted;

    [Header("Cursor Texture")]
    [SerializeField] private Texture2D cursorTex;
 

    private void Start()
    {
        gameStateListener.AddEvent(SetCursorState);
        InitStartState();
        Cursor.SetCursor(cursorTex, new Vector2(8,8), CursorMode.Auto);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
           Quit();
    }

    public void SetCursorState() => Cursor.lockState = gamePaused ? CursorLockMode.None : CursorLockMode.Locked;

    public void InitStartState()
    {
        selectionScreen.SetBuilding(selectionScreen.Buildings[0]);
        isStarted = true;
    }

    private void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
