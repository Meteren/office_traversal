using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccludableObject : MonoBehaviour
{
    private MeshRenderer objectRenderer;
    [SerializeField] private float fadeSpeed;
    private float alphaValue = 1;
    bool isObstructing;
    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();    

    }
    private void Update()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        objectRenderer.GetPropertyBlock(block);

        block.SetFloat("_AlphaValue", GetAlphaValue(isObstructing));

        objectRenderer.SetPropertyBlock(block);

    }

    public void SetMaterial(bool isObstructing) =>
        this.isObstructing = isObstructing;


    float GetAlphaValue(bool isObstructing)
    {
        if (isObstructing)
            alphaValue -= Time.deltaTime * fadeSpeed;
        else
            alphaValue += Time.deltaTime * fadeSpeed;

        alphaValue = Mathf.Clamp01(alphaValue); 

        return alphaValue;
    }
}
