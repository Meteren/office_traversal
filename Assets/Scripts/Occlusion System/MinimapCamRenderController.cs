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
                    MaterialProvider materialProvider = obj.GetComponent<MaterialProvider>();
                    obj.GetComponent<Renderer>().material = materialProvider.DefaultMaterial;
                }
            }
            else
            {
                foreach (var obj in objectsToBeEffected)
                {
                    MaterialProvider materialProvider = obj.GetComponent<MaterialProvider>();
                    obj.GetComponent<Renderer>().material = materialProvider.FadeMaterial;
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
                MaterialProvider materialProvider = obj.GetComponent<MaterialProvider>();

                if (materialProvider != null)
                {
                    obj.GetComponent<Renderer>().material = materialProvider.DefaultMaterial;
                }
                
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
        return objects.SelectMany(x => GetPartsAndItself(x.transform)).ToList();
    }

    private IEnumerable<Renderer> GetPartsAndItself(Transform transform)
    {
        if (transform.TryGetComponent<Renderer>(out Renderer parentRenderer))
            yield return parentRenderer;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Renderer>(out Renderer childRenderer))
            {
                yield return childRenderer;
            }
        }

    }
    /*
    private List<Renderer> GetRenderers(List<GameObject> objects)
    {
        return objects.Select(x =>
        {
            return x.TryGetComponent<Renderer>(out Renderer renderer) ? renderer : null;
        }).Where(x => x).ToList();
    }*/

}
