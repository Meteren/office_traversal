using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccludableObject : MonoBehaviour
{
    private MeshRenderer objectRenderer;

    [Header("Fade Speed")]
    [SerializeField] private float fadeSpeed;

    [HideInInspector] public float alphaValue = 1;
    bool isObstructing;

    [Header("Clip Shadows")]
    [SerializeField] private bool clipShadows;

    [Header("Is A Part")]
    public bool isPart;
    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();    

    }
    private void Update()
    {
        if(gameObject.layer != LayerMask.NameToLayer("Furniture"))
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            objectRenderer.GetPropertyBlock(block);

            if (!isPart)
                block.SetFloat("_AlphaValue", GetAlphaValue(isObstructing));
            else
            {
                block.SetFloat("_AlphaValue", alphaValue);
            }

            block.SetInt("_ClipShadows", Convert.ToInt32(clipShadows));

            objectRenderer.SetPropertyBlock(block);
        }  

    }

    public void SetMaterial(bool isObstructing) =>
        this.isObstructing = isObstructing;

    float GetAlphaValue(bool isObstructing)
    {
        if (isObstructing)
            alphaValue -= Time.deltaTime * fadeSpeed;
        else
            alphaValue += Time.deltaTime * fadeSpeed;

        SetChildrenValues();
        alphaValue = Mathf.Clamp01(alphaValue); 

        return alphaValue;
    }

    private void SetChildrenValues()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<OccludableObject>(out OccludableObject occludableObject))
            {

                if (isObstructing)
                {
                    if (occludableObject.gameObject.layer == LayerMask.NameToLayer("Furniture"))
                        if(!GameManager.instance.miniMapController.shouldOpenMap)
                            occludableObject.SetState(false);
                    else
                        occludableObject.alphaValue -= Time.deltaTime * fadeSpeed;
                }
                else
                {
                    if (occludableObject.gameObject.layer == LayerMask.NameToLayer("Furniture"))
                    {
                        if (!GameManager.instance.miniMapController.shouldOpenMap)
                            occludableObject.SetState(true);
                    }
                    else
                        occludableObject.alphaValue += Time.deltaTime * fadeSpeed;
                     
                }
                   
                occludableObject.alphaValue = Mathf.Clamp01(alphaValue);

            }
        }

    }

    public void SetState(bool state) => gameObject.SetActive(state);
}
