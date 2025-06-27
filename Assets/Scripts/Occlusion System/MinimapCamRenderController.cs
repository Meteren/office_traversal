using System.Collections;
using System.Collections.Generic;
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
            foreach(var obj in objectsToBeEffected)
            {
                obj.GetComponent<Renderer>().material = fadeMaterial;
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
}
