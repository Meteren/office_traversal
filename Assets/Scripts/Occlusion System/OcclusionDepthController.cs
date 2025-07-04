using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OcclusionDepthController : MonoBehaviour
{
    private PlayerController playerController;
    private Camera miniMapCam;
    [Header("Layer Mask")]
    [SerializeField] private LayerMask layer;
    private MeshRenderer meshRenderer;
    [HideInInspector]public List<OccludableObject> occludables = new List<OccludableObject>();
    [Header("Map Activation Listener")]
    [SerializeField] private EventListener listener;
    [Header("Map Deactivation Listener On Select Screen")]
    [SerializeField] private EventListener deactivationListener;
    private bool lockExecution = false;

    private void Start()
    {
        listener.AddEvent(HandleExecution);
        deactivationListener.AddEvent(HandleExecution);
        miniMapCam = GameObject.Find("MiniMapCamera").GetComponent<Camera>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

    }
    private void Update()
    {
        if(!lockExecution)
            ExecuteOcclusionLogic();    
    }

    private void ExecuteOcclusionLogic()
    {
        Vector3 screenPos = miniMapCam.WorldToScreenPoint(playerController.transform.position);
        Ray ray = miniMapCam.ScreenPointToRay(screenPos);
        float distance = Vector3.Distance(playerController.transform.position, miniMapCam.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, layer, QueryTriggerInteraction.Ignore);

        List<OccludableObject> listedOccludables = hits
            .Select(x =>
            {
                x.transform.TryGetComponent<OccludableObject>(out var occludable);
                return occludable;
            })
            .Where(o => o != null)
            .ToList();

        AddOccludables(listedOccludables);
        RemoveOccludables(listedOccludables);
 
    }
    private void AddOccludables(List<OccludableObject> listedOccludables)
    {
        foreach (var occludable in listedOccludables)
        {
            occludables.Add(occludable);
            occludable.SetMaterial(isObstructing:true);
        }              
    }
    private void RemoveOccludables(List<OccludableObject> listedOccludables)
    {

        foreach (var occludable in occludables.ToList())
        {
            if (!listedOccludables.Contains(occludable))
            {
                occludable.SetMaterial(false);
                occludables.Remove(occludable);

            }

        }
            
    }
    private void HandleExecution() => lockExecution = !lockExecution;
  
}
