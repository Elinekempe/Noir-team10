using TMPro;
using UnityEngine;

public class ClueUIBehavior : MonoBehaviour
{
    private CanvasGroup clueUIGroup;
    private TMP_Text clueText;
    public string ClueInfo;

    private void Start()
    {
        clueUIGroup = GetComponent<CanvasGroup>();
        if (clueUIGroup == null)
        {
            clueUIGroup = gameObject.AddComponent<CanvasGroup>();
        }

        clueText = GetComponentInChildren<TMP_Text>(true);
        DeactivateUI();
    }

    public void ActivateUI(string clueinfo)
    {
        ClueInfo = clueinfo;
        UpdateUI();

        if (clueUIGroup != null)
        {
            clueUIGroup.alpha = 1f;
            clueUIGroup.interactable = true;
            clueUIGroup.blocksRaycasts = true;
        }
    }

    public void DeactivateUI()
    {
        if (clueUIGroup != null)
        {
            clueUIGroup.alpha = 0f;
            clueUIGroup.interactable = false;
            clueUIGroup.blocksRaycasts = false;
        }
    }

    private void UpdateUI()
    {
        if (clueText != null)
        {
            clueText.text = ClueInfo;
        }
    }
}