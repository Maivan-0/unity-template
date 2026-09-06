using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "InventoryObjects/Item")]

public class Item : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public Sprite image;
    [SerializeField] public ItemCategory itemCategory;
    [SerializeField] public ItemType itemType;
    [SerializeField] public ItemName artefactType;
    [SerializeField] public string info;
    [SerializeField] public string effect;
}

public enum ItemCategory
{
    Charm,
    Item
}

public enum ItemType
{
    None,
    Wepon,
    Necklace,
    Ring,
    Book,
    Cloak,
    Crystal,
    Heart
}

public enum ItemName
{
    None,
    VitalNecklace,
    WardNecklace,
    RegenNecklace,
    SwiftRing,
    SharpRing,
    IronRing,
    BurnBook,
    KnockbackBook,
    ShadowCloak,
    TrapResistCloak,
    VitalResonance,
    ResonantCrystal,
    LivingHeart
}
