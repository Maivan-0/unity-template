using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMapManager : MonoBehaviour
{
    [Header("UIManager")]
    [SerializeField] private KeyCode OpenCloseMap;

    [Header("Inventory Tabs")]
    [SerializeField] private GameObject inventoryItemTab;
    [SerializeField] private GameObject inventoryCharmTab;

    [Header("Inventory Buttons")]
    [SerializeField] private GameObject inventoryItemButton;
    [SerializeField] private GameObject inventoryCharmButton;

    [SerializeField] private List<GameObject> allTabs;
    [SerializeField] private List<GameObject> allButtons;

    private Animator mapAnimator;
    private Image image;
    private bool isMapOpen = false;

    private void Awake()
    {
        mapAnimator = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.enabled = false;
    }

    private void Start()
    {
        ButtonOff();
    }

    private void Update()
    {
        if (PauseMenu.instance.isPaused) return;

        if (Input.GetKeyDown(OpenCloseMap) && !isMapOpen)
        {
            image.enabled = true;
            mapAnimator.SetTrigger("Open");
            isMapOpen = true;
        }
        else if (Input.GetKeyDown(OpenCloseMap))
        {
            mapAnimator.SetTrigger("Close");
            isMapOpen = false;
        }
    }

    public void ButtonOff()
    {
        inventoryCharmButton.SetActive(false);
        inventoryItemButton.SetActive(false);
        inventoryCharmTab.SetActive(false);
        inventoryItemTab.SetActive(false);
    }

    private void ActivateTab(GameObject activeTab, GameObject activeButton, List<GameObject> allTabs, List<GameObject> allButtons)
    {
        foreach (var tab in allTabs) tab.SetActive(false);
        foreach (var button in allButtons) button.SetActive(false);

        if(activeTab != null) activeTab.SetActive(true);
        if(activeButton != null) activeButton.SetActive(true);
    }

    public void InventoryCharmTab()
    {
        mapAnimator.SetTrigger("InventoryCharm");

        ActivateTab(inventoryCharmTab, inventoryItemButton, allTabs, allButtons);
    }

    public void ChangeOpenTab()
    {
        if (CraftingSystem.instance.playerInCraftZone)
        {
            CraftTab();
        }
        else
        {
            InventoryCharmTab();
        }
    }

    public void ChangeTab()
    {
        if(CraftingSystem.instance.playerInCraftZone)
        {
            CraftTab();
        }
        else
        {
            InventoryItemTab();
        }
    }

    private void InventoryItemTab()
    {
        mapAnimator.SetTrigger("InventoryItem");

        ActivateTab(inventoryItemTab, inventoryCharmButton, allTabs, allButtons);
    }

    private void CraftTab()
    {
        mapAnimator.SetTrigger("Craft");

        ActivateTab(inventoryItemTab, inventoryCharmButton, allTabs, allButtons);
    }
}
