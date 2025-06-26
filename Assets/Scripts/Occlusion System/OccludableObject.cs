using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccludableObject : MonoBehaviour
{
    // Start is called before the first frame update
    private MeshRenderer objectRenderer;
    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();  
    }
 
    public void SetMaterial(bool isObstructing)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        
        objectRenderer.GetPropertyBlock(block);

        block.SetInt("_PlayerObstructed", isObstructing ? 1 : 0);

        objectRenderer.SetPropertyBlock(block); 

    }
}
