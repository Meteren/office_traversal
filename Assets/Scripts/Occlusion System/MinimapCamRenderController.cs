using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MinimapCamRenderController : MonoBehaviour
{
    [Header("Camera Name")]
    [SerializeField] private string cameraName;

    [Header("Objects To Render")]
    [SerializeField] private List<Renderer> objectsToBeEffected;

    [Header("Default Materials")]
    [SerializeField] private Material fadeMaterial;
    [SerializeField] private Material defaultMaterial;

    [Header("MiniMap Controller")]
    [SerializeField] private MiniMapController miniMapController;

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }


    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {

        if(camera.name == cameraName && objectsToBeEffected.Count != 0)
        {
            if (miniMapController.ShouldOpenMap)
            {
                
                foreach (var obj in objectsToBeEffected)
                {
                    obj.GetComponent<Renderer>().material = defaultMaterial;
                }
            }
            else
            {
                Debug.Log("Rendering fade");
                foreach (var obj in objectsToBeEffected)
                {
                    obj.GetComponent<Renderer>().material = fadeMaterial;
                }

            }
        }
            
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera.name == cameraName && objectsToBeEffected.Count != 0)
        {
            foreach (var obj in objectsToBeEffected)
            {
                obj.GetComponent<Renderer>().material = defaultMaterial;
            }
        }
    }

    public void AddObjectsToBeEffected(List<GameObject> objects)
    {
        List<Renderer> objectsToAdd = GetRenderers(objects);

        foreach(var o in objectsToAdd)
        {
            objectsToBeEffected.Add(o);
        }
    }
    public void RemoveObjectsToBeEffected(List<GameObject> objects)
    {
        List<Renderer> objectsToRemove = GetRenderers(objects);

        foreach (var o in objectsToRemove)
        {
            objectsToBeEffected.Remove(o);
        }
    }

    private List<Renderer> GetRenderers(List<GameObject> objects)
    {
        return objects.Select(x =>
        {
            return x.TryGetComponent<Renderer>(out Renderer renderer) ? renderer : null;
        }).Where(x => x).ToList();
    }

}
