using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using TMPro;
using Unity.VisualScripting;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] public Image image;
    [SerializeField] public Item item;
    [SerializeField] public TMP_Text textMeshPro;

    [HideInInspector] public Transform parentAfterDrag;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 initialScale;

    private void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        textMeshPro = GameObject.Find("Information").gameObject.GetComponent<TMP_Text>();
        initialScale = rectTransform.localScale;
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        StopAllCoroutines();
        StartCoroutine(ScaleTo(initialScale * 1.25f, 0.15f));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);

        StopAllCoroutines();
        StartCoroutine(ScaleTo(initialScale, 0.05f));

        rectTransform.localScale = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(item != null && textMeshPro != null)
        {
            textMeshPro.text = item.itemName + "\n" + item.info + "\n" + item.effect;
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 StartScale = rectTransform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            rectTransform.localScale = Vector3.Lerp(StartScale, targetScale, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }
}
