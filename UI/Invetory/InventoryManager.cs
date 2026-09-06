using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public InventorySlot[] inventorySlots;
    [SerializeField] public GameObject inventoryItemPrefab;

    [SerializeField] public InventorySlot ArtefactSlots;

    [Header("PlayerComponent")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Sloturi artefacte")]
    [SerializeField] private InventorySlot necklaceSlot;
    [SerializeField] private InventorySlot ringSlot;
    [SerializeField] private InventorySlot bookSlot;
    [SerializeField] private InventorySlot cloakSlot;

    private Dictionary<ItemType?, ItemName?> currentTypes = new();

    private void Start()
    {
        currentTypes[ItemType.Necklace] = null;
        currentTypes[ItemType.Ring] = null;
        currentTypes[ItemType.Book] = null;
        currentTypes[ItemType.Cloak] = null;
    }

    private void Update()
    {
        CheckCharmSlot(ItemType.Necklace, necklaceSlot);
        CheckCharmSlot(ItemType.Ring, ringSlot);
        CheckCharmSlot(ItemType.Book, bookSlot);
        CheckCharmSlot(ItemType.Cloak, cloakSlot);
    }

    public void Add(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null && slot.itemCategorySlot == item.itemCategory)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    private void SpawnNewItem(Item item, InventorySlot inventorySlot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, inventorySlot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    private void CheckCharmSlot(ItemType itemType, InventorySlot charmSlot)
    {
        InventoryItem item = charmSlot.GetComponentInChildren<InventoryItem>();
        ItemName? currentType = currentTypes[itemType];

        if(item != null)
        {
            if(currentType != item.item.artefactType)
            {
                if(currentType != null)
                    RemoveCharmEffect(currentType, itemType);

                ApplyCharmEffect(item.item.artefactType, itemType);
                currentTypes[itemType] = item.item.artefactType;
            }
        }
        else if(item == null && currentType != null)
        {
            RemoveCharmEffect(currentType, itemType);
            currentTypes[itemType] = null;
        }
    }

    private void ApplyCharmEffect(ItemName artefactType, ItemType itemType)
    {
        switch(itemType)
        {
            case ItemType.Necklace:
                ApplyNecklaceEffect(artefactType);
                break;
            case ItemType.Ring: 
                ApplyRingEffect(artefactType);
                break;
            case ItemType.Book:
                ApplyBookEffect(artefactType);
                break;
            case ItemType.Cloak:
                ApplyCloakEffect(artefactType);
                break;
        }
    }

    private void ApplyNecklaceEffect(ItemName necklaceType)
    {
        switch(necklaceType)
        {
            case ItemName.VitalNecklace:
                playerStats.ModifyMaxHealth(20, true);
                break;
            case ItemName.WardNecklace:
                playerStats.ModifyDamageResistance(5, true);
                break;
            case ItemName.RegenNecklace:
                playerStats.ModifyVampireEffect(true);
                break;
            case ItemName.VitalResonance:
                playerStats.ModifyEssenceHeal(true);
                break;
        }
    }

    private void ApplyRingEffect(ItemName ringType)
    {
        switch (ringType)
        {
            case ItemName.SwiftRing:
                playerStats.ModifySpeed(5, true);
                break;
            case ItemName.SharpRing:
                playerStats.ModifyMeleeDamage(5, true);
                break;
            case ItemName.IronRing:
                playerStats.ModifyKnockbackResistance(2, true);
                break;
        }
    }

    private void ApplyBookEffect(ItemName bookType)
    {
        switch (bookType)
        {
            case ItemName.BurnBook:
                playerStats.ModifyBurning(true);
                break;
        }
    }

    private void ApplyCloakEffect(ItemName cloakType)
    {
        switch (cloakType)
        {
            case ItemName.ShadowCloak:
                playerStats.ModifyChaseRadius(0.2f, true);
                break;
            case ItemName.TrapResistCloak:
                playerStats.ModifyMeleeDamage(5, true);
                break;
        }
    }

    private void RemoveCharmEffect(ItemName? currentType, ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Necklace:
                RemoveNecklaceEffect(currentType);                
                break;
            case ItemType.Ring:
                RemoveRingEffect(currentType);
                break;
            case ItemType.Book:
                RemoveBookEffect(currentType);
                break;
            case ItemType.Cloak: 
                RemoveCloakEffect(currentType); 
                break;
        }
    }

    private void RemoveNecklaceEffect(ItemName? currentType)
    {
        if (currentType == null)
            return;

        switch (currentType)
        {
            case ItemName.VitalNecklace:
                playerStats.ModifyMaxHealth(20, false);
                break;
            case ItemName.WardNecklace:
                playerStats.ModifyDamageResistance(5, false);
                break;
            case ItemName.RegenNecklace:
                playerStats.ModifyVampireEffect(false);
                break;
            case ItemName.VitalResonance:
                playerStats.ModifyEssenceHeal(false);
                break;
        }
    }

    private void RemoveRingEffect(ItemName? currentType)
    {
        if (currentType == null)
            return;

        switch (currentType)
        {
            case ItemName.SwiftRing:
                playerStats.ModifySpeed(5, false);
                break;
            case ItemName.SharpRing:
                playerStats.ModifyMeleeDamage(5, false);
                break;
            case ItemName.IronRing:
                playerStats.ModifyKnockbackResistance(2, false);
                break;
        }
    }

    private void RemoveBookEffect(ItemName? currentType)
    {
        if (currentType == null)
            return;

        switch (currentType)
        {
            case ItemName.BurnBook:
                playerStats.ModifyBurning(false);
                break;
        }
    }

    private void RemoveCloakEffect(ItemName? currentType)
    {
        if (currentType == null)
            return;

        switch (currentType)
        {
            case ItemName.ShadowCloak:
                playerStats.ModifyChaseRadius(0.2f, false);
                break;
            case ItemName.TrapResistCloak:
                playerStats.ModifyMeleeDamage(5, true);
                break;
        }
    }
}
