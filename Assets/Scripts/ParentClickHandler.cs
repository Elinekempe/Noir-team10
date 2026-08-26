using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentHoverHandler2D : MonoBehaviour
{
    [Header("Hover settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private float returnDuration = 0.2f;

    [Header("Child settings")]
    [SerializeField] private bool includeChildren = true;
    [SerializeField] private bool includeInactiveChildren = false;

    [Header("Clue settings")]
    [SerializeField] private ClueUIBehavior clueUI;

    private readonly Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
    private readonly Dictionary<GameObject, Coroutine> activeAnimations = new Dictionary<GameObject, Coroutine>();
    private readonly List<GameObject> allChildren = new List<GameObject>();

    private GameObject currentHoverObject;

    private void Start()
    {
        FindAllChildren(transform);

        foreach (var obj in allChildren)
        {
            if (obj == null)
            {
                continue;
            }

            originalScales[obj] = obj.transform.localScale;
        }
    }

    private void Update()
    {
        if (Camera.main == null)
        {
            return;
        }

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.OverlapPoint(mouseWorldPos);

        GameObject hitObject = null;

        if (hit != null)
        {
            var potentialObject = hit.gameObject;
            if (IsChildOfParent(potentialObject))
            {
                hitObject = potentialObject;
            }
        }

        if (hitObject == currentHoverObject)
        {
            if (Input.GetMouseButtonDown(0) && hitObject != null)
            {
                ShowClue(hitObject);
            }
            return;
        }

        if (currentHoverObject != null && IsChildOfParent(currentHoverObject))
        {
            ResetObject(currentHoverObject);
        }

        if (hitObject != null)
        {
            GrowObject(hitObject);
        }

        currentHoverObject = hitObject;
    }

    private void ShowClue(GameObject clickedObject)
    {
        if (clickedObject == null)
        {
            return;
        }

        if (clueUI == null)
        {
            clueUI = FindAnyObjectByType<ClueUIBehavior>();
        }

        var clueItem = clickedObject.GetComponent<ClueItem>();
        var clueText = clueItem != null ? clueItem.clueText : "Geen hint beschikbaar";

        if (clueUI != null)
        {
            clueUI.ActivateUI(clueText);
        }
    }

    private void FindAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (includeInactiveChildren || child.gameObject.activeSelf)
            {
                allChildren.Add(child.gameObject);
            }

            if (includeChildren && child.childCount > 0)
            {
                FindAllChildren(child);
            }
        }
    }

    private bool IsChildOfParent(GameObject obj)
    {
        if (obj == null)
        {
            return false;
        }

        return allChildren.Contains(obj) || obj.transform.IsChildOf(transform);
    }

    private void GrowObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        if (activeAnimations.TryGetValue(obj, out var existingCoroutine) && existingCoroutine != null)
        {
            StopCoroutine(existingCoroutine);
        }

        if (!originalScales.TryGetValue(obj, out var originalScale))
        {
            originalScale = obj.transform.localScale;
            originalScales[obj] = originalScale;
        }

        var targetScale = originalScale * hoverScaleMultiplier;
        var newAnim = StartCoroutine(AnimateScale(obj, targetScale, animationDuration));
        activeAnimations[obj] = newAnim;
    }

    private void ResetObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        if (activeAnimations.TryGetValue(obj, out var existingCoroutine) && existingCoroutine != null)
        {
            StopCoroutine(existingCoroutine);
        }

        if (!originalScales.TryGetValue(obj, out var originalScale))
        {
            originalScale = obj.transform.localScale;
            originalScales[obj] = originalScale;
        }

        var newAnim = StartCoroutine(AnimateScale(obj, originalScale, returnDuration));
        activeAnimations[obj] = newAnim;
    }

    private IEnumerator AnimateScale(GameObject obj, Vector3 targetScale, float duration)
    {
        if (obj == null)
        {
            yield break;
        }

        var startScale = obj.transform.localScale;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        obj.transform.localScale = targetScale;
        activeAnimations.Remove(obj);
    }

    private void OnDisable()
    {
        foreach (var obj in allChildren)
        {
            if (obj == null)
            {
                continue;
            }

            if (activeAnimations.TryGetValue(obj, out var activeCoroutine) && activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeAnimations.Remove(obj);
            }

            if (originalScales.TryGetValue(obj, out var originalScale))
            {
                obj.transform.localScale = originalScale;
            }
        }

        currentHoverObject = null;
    }
}