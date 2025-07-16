using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Image image;
    [Header("Waiting Time")]

    [Header("Button Color To Change")]
    [SerializeField] private Color color;

    private bool coroutineInProgress = false;
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!coroutineInProgress)
        {
            StartCoroutine(ChangeColor());
        }
    }

    private IEnumerator ChangeColor()
    {
        coroutineInProgress = true;
        yield return new WaitForSeconds(3f);
        Color defaultColor = image.color;
        image.color = color;
        yield return new WaitForSeconds(0.2f);
        image.color = defaultColor;
        coroutineInProgress = false;
    }

}
