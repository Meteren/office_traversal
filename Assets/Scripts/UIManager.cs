using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI indicator;

    [Header("Text Timer")]
    [SerializeField] private float timer;


    public void ShowIndicator(string text)
    {
        StartCoroutine(Timer(text));
    }

    public IEnumerator Timer(string text)
    {
        indicator.gameObject.SetActive(true);
        indicator.text = text;
        yield return new WaitForSeconds(timer);
        indicator.gameObject.SetActive(false);
    }

}
