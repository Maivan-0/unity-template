using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlSettings : MonoBehaviour
{
    [SerializeField] List<InputButton> inputButtons = new List<InputButton>();

    private Dictionary<KeyName, KeyCode> keyBindings = new();
    private KeyName bindingToChange = KeyName.None;
    private bool isWaitingForKey = false;

    private void Start()
    {
        keyBindings = InputManager.instance.GetKeyMap();

        foreach(var inputButton in inputButtons)
        {
            KeyName localName = inputButton.keyName;
            inputButton.button.onClick.AddListener(() => OnRebindPressed(localName));
            UpdateInformation(localName);
        }
    }

    private void Update()
    {
        if(bindingToChange == KeyName.None || !isWaitingForKey) return;

        if(Input.GetKeyDown(KeyCode.Escape) && isWaitingForKey)
        {
            UpdateInformation(bindingToChange);
            bindingToChange = KeyName.None;
            isWaitingForKey = false;
        }
        
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if(Input.GetKeyDown(keyCode))
            {
                keyBindings[bindingToChange] = keyCode;
                UpdateInformation(bindingToChange);
                bindingToChange = KeyName.None;
                isWaitingForKey = false;
            }
        }
    }

    private void OnRebindPressed(KeyName actionName)
    {
        isWaitingForKey = true;
        bindingToChange = actionName;

        foreach (var inputButton in inputButtons)
        {
            if (inputButton.keyName == actionName)
                inputButton.keyText.text = "...";
        }
    }

    private void UpdateInformation(KeyName actionName)
    {
        foreach (var inputButton in inputButtons)
        {
            if(inputButton.keyName == actionName)
            {
                if(keyBindings.ContainsKey(actionName))
                {
                    inputButton.keyText.text = keyBindings[actionName].ToString();
                    InputManager.instance.SetKey(actionName, keyBindings[actionName]);
                }
                else
                {
                    inputButton.keyText.text = "None";
                    InputManager.instance.SetKey(actionName, KeyCode.None);
                }
                break;
            }
        }
    }
}

[System.Serializable]
public class InputButton
{
    public KeyName keyName;
    public Button button;
    public TMP_Text keyText;
}