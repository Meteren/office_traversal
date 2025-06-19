using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField] private PlayerController pController;


    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;


    [Header("Conditions")]
    [SerializeField] private bool miniMapEnabled;


    private void Start()
    {
        miniMapEventListener.AddEvent(HandleMiniMapCondition);
    }
    private void Update()
    {
        if(!miniMapEnabled)
            transform.position = new Vector3(pController.transform.position.x, transform.position.y, pController.transform.position.z);   
    }

    private void HandleMiniMapCondition() => miniMapEnabled = !miniMapEnabled;
}
