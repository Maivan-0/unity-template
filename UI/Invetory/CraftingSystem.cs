using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance { get; private set; }

    [Header("Crafting Setup")]
    [SerializeField] private List<CraftingRecipe> craftingRecipes;

    [Header("Crafting Slots")]
    [SerializeField] private List<InventorySlot> craftingSlots;
    [SerializeField] private Item craftedItem;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventoryItemPrefab;

    [Header("Crafter")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool playerInCraftZone;

    private int previousItemCount = 0;
    private bool hasCrafted = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        var items = GetItemsFromSlots();

        if (hasCrafted && InputManager.instance.InteractPressed())
        {
            foreach (InventorySlot slot in craftingSlots)
            {
                ClearCraftingSlot(slot);
            }

            inventoryManager.Add(craftedItem);
            craftedItem = null;
            spriteRenderer.sprite = null;
            
            hasCrafted = false;
        }
        else
        {
            foreach (CraftingRecipe craftingRecipe in craftingRecipes)
            {
                MatchesRecipe(craftingRecipe, items);
            }
        }

        if (previousItemCount != items.Count)
        {
            previousItemCount = items.Count;

            spriteRenderer.sprite = null;
            RefreshCrafting(items);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            playerInCraftZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerInCraftZone = false;
    }

    private List<ItemName> GetItemsFromSlots()
    {
        List<ItemName> items = new List<ItemName>();

        foreach (InventorySlot slot in craftingSlots)
        {
            InventoryItem inventory = slot.GetComponentInChildren<InventoryItem>();

            if (inventory != null)
                items.Add(inventory.item.artefactType);
        }

        return items;
    }

    private void MatchesRecipe(CraftingRecipe recipe, List<ItemName> items)
    {
        if (items.Count != recipe.requiredItems.Count)
            return;

        var itemsSet = new HashSet<ItemName>(items);
        var requiredSet = new HashSet<ItemName>(recipe.requiredItems);

        if(itemsSet.SetEquals(requiredSet))
        {
            craftedItem = recipe.resultItem;
            hasCrafted = true;

            spriteRenderer.sprite = recipe.resultItem.image;
        }
    }

    private void RefreshCrafting(List<ItemName> items)
    {
        if (hasCrafted)
        {
            craftedItem = null;
            hasCrafted = false;
            spriteRenderer.sprite = null;
        }

        foreach (CraftingRecipe craftingRecipe in craftingRecipes)
        {
            MatchesRecipe(craftingRecipe, items);
        }
    }

    private void ClearCraftingSlot(InventorySlot inventorySlot)
    {
        if (inventorySlot.transform.childCount > 0)
        {
            Transform itemTransform = inventorySlot.transform.GetChild(0);
            Destroy(itemTransform.gameObject);
        }
    }
}

[System.Serializable]
public class CraftingRecipe
{
    [Header("CraftingRecipe")]
    public List<ItemName> requiredItems;
    public Item resultItem;
}
