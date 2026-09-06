using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus;
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private Animator animator;

    private void Start()
    {
        foreach (var menu in menus) { menu.SetActive(false); }
    }

    public void AudioMenu()
    {
        ActivateTab(menus[0], buttons[0], menus, buttons);
        animator.SetTrigger("Audio");
    }

    public void VideoMenu()
    {
        ActivateTab(menus[1], buttons[1], menus, buttons);
        animator.SetTrigger("Video");
    }

    public void ControlMenu()
    {
        ActivateTab(menus[2], buttons[2], menus, buttons);
        animator.SetTrigger("Control");
    }

    public void Back()
    {
        foreach (var menu in menus) { menu.SetActive(false); }
        foreach (var button in buttons) { button.SetActive(true); }
    }

    private void ActivateTab(GameObject activeTab, GameObject activeButton, List<GameObject> allTabs, List<GameObject> allButtons)
    {
        foreach (var tab in allTabs) tab.SetActive(false);
        foreach (var button in allButtons) button.SetActive(true);

        if (activeTab != null) activeTab.SetActive(true);
        if (activeButton != null) activeButton.SetActive(false);
    }
}
