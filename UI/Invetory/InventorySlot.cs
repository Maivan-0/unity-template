using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public ItemCategory itemCategorySlot;
    [SerializeField] public ItemType itemTypeSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (inventoryItem != null && (inventoryItem.item.itemType == itemTypeSlot || (inventoryItem.item.itemCategory == itemCategorySlot && itemTypeSlot == ItemType.None)))
            {
                inventoryItem.parentAfterDrag = transform;
            }
        }
    }
}
