using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPassage : MonoBehaviour
{
    [SerializeField] private int floorIndex;
    private MiniMapController miniMapController;

    private void Start()
    {
        miniMapController = GameObject.FindAnyObjectByType<MiniMapController>();
    }
    private void OnTriggerStay(Collider other)
    {
        miniMapController.floorIndex = floorIndex;
    }


}
