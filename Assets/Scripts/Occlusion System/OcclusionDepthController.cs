using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OcclusionDepthController : MonoBehaviour
{
    private PlayerController playerController;
    private Camera miniMapCam;
    [SerializeField] private LayerMask layer;
    private MeshRenderer meshRenderer;
    private List<OccludableObject> occludables = new List<OccludableObject>();
    [SerializeField] private EventListener listener;
    private bool lockExecution = false;

    private void Start()
    {
        listener.AddEvent(HandleExecution);
        miniMapCam = GameObject.Find("MiniMapCam").GetComponent<Camera>();
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
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layer, QueryTriggerInteraction.Ignore);

        List<OccludableObject> listedOccludables = hits
            .Select(x =>
            {
                x.transform.TryGetComponent<OccludableObject>(out var occludable);
                return occludable;
            })
            .Where(o => o != null)
            .ToList();

        if (hits.Length != 0)
        {
            AddOccludables(listedOccludables);
            RemoveOccludables(listedOccludables);
        }


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
