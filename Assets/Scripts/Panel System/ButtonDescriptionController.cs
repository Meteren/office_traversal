using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDescriptionController : MonoBehaviour
{
    private InfoPanel panel;
    private RectTransform rectTransform;

    [Header("Button Description")]
    [SerializeField] private string buttonDescription;

    [Header("Is Panel Active")]
    public bool panelActive;

    private InfoPanel infoPanel;
    void Start()
    {
        panel = FindObjectOfType<InfoPanel>();
        rectTransform = GetComponent<RectTransform>();
    }

   
    void Update()
    {
        if (panelActive )
        {
            if (CheckIfInBoundaries() && Cursor.lockState == CursorLockMode.None)
            {
                if (!panel.gameObject.activeSelf)
                    panel.OpenPanel(buttonDescription,this);

            }
            else
            {
                SetButtonPanelStatesOn();
                if (panel.gameObject.activeSelf)
                    panel.ClosePanel();

            }
        }          
        
    }

    private void SetButtonPanelStatesOn()
    {
        for(int i = 0; i < panel.buttons.Length; i++)
        {
            panel.buttons[i].panelActive = true;
        }
    }

    private bool CheckIfInBoundaries()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);

        return rectTransform.rect.Contains(localPoint);
    }
}
