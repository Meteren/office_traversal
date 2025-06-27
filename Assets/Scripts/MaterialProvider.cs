using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialProvider : MonoBehaviour
{
    private Material defaultMaterial;
    public Material DefaultMaterial { get { return defaultMaterial; } set { defaultMaterial = value; } }

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        defaultMaterial = renderer.material;   
    }

    
}
