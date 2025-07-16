
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI textMesh;

    private RectTransform panelRectTransform;

    private RectTransform canvasRectTransform;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas;

    [HideInInspector]public ButtonDescriptionController[] buttons;


    private void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        panelRectTransform = GetComponent<RectTransform>();

        buttons = FindObjectsByType<ButtonDescriptionController>(FindObjectsSortMode.InstanceID);

    }

    public void OpenPanel(string text,ButtonDescriptionController button)
    {
        ManageButtonPanelConditions(button);
        gameObject.SetActive(true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, null, out Vector2 localPos);
        panelRectTransform.localPosition = localPos;

        Vector2 canvasSize = canvasRectTransform.rect.size;
        Vector2 panelSize = panelRectTransform.rect.size;

        float x = Mathf.Clamp(panelRectTransform.localPosition.x, -canvasSize.x / 2 + panelSize.x / 2, canvasSize.x / 2 - panelSize.x/2);
        float y = Mathf.Clamp(panelRectTransform.localPosition.y, -canvasSize.y / 2 + panelSize.y / 2, canvasSize.y / 2 - panelSize.y / 2);

        panelRectTransform.localPosition = new Vector2(x, y);

        textMesh.text = text;

    }

    public void ClosePanel() => gameObject.SetActive(false);

    private void ManageButtonPanelConditions(ButtonDescriptionController button)
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            Debug.Log($"Button name: {buttons[i].name}");
            if (buttons[i] == button)
                buttons[i].panelActive = true;
            else
                buttons[i].panelActive = false;   
        }
    }

}
