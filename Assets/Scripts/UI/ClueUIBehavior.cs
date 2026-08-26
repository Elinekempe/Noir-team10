using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueUIBehavior : MonoBehaviour
{
    private CanvasGroup ClueUIGroup;
    private TMP_Text ClueText;
    public string ClueInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClueUIGroup = GetComponent<CanvasGroup>();
        ClueText = GetComponentInChildren<TMP_Text>();
        DeactivateUI();
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
}
