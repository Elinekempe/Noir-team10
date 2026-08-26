using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClueUIBehavior : MonoBehaviour
{
    private CanvasGroup ClueUIGroup;
    private TMP_Text ClueText;
    public string ClueInfo;

    public InputAction PressLMB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
            // Works with mouse clicks and screen touches
            if (Input.GetMouseButtonDown(0))
            {
                DeactivateUI();
            }
    }

    void Start()
    {
        ClueUIGroup = GetComponent<CanvasGroup>();
        ClueText = GetComponentInChildren<TMP_Text>();
        ActivateUI("Info");
    }
    public void ActivateUI(string clueinfo)
    {
        ClueInfo = clueinfo;
        UpdateUI();
        ClueUIGroup.alpha = 1;
    }
    void DeactivateUI()
    {
        ClueUIGroup.alpha = 0;
    }
    void UpdateUI()
    {
        Debug.Log(ClueInfo);
        ClueText.text = ClueInfo;
    }
    void OnMouseDown()
    {
        DeactivateUI();
    }
}
