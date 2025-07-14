using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialProvider : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material fadeMaterial;
    public Material DefaultMaterial { get { return defaultMaterial; } set { defaultMaterial = value; } }

    public Material FadeMaterial { get { return fadeMaterial; } set { fadeMaterial = value; } }

    [Header("Attributes")]
    public string layer;
    public bool isFloor;

}
