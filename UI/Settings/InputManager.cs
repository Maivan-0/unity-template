using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] private List<KeyBinding> keyBindings = new List<KeyBinding>();
    private Dictionary<KeyName, KeyCode> keyMap = new();

    private void Awake()
    {
        instance = this;

        foreach (var binding in keyBindings)
        {
            keyMap[binding.keyName] = binding.keyCode;
        }
    }

    public Dictionary<KeyName, KeyCode> GetKeyMap() { return keyMap; }

    public bool JumpPressed() => Input.GetKeyDown(keyMap[KeyName.Jump]);
    public bool DashPressed() => Input.GetKeyDown(keyMap[KeyName.Dash]);
    public bool InteractPressed() => Input.GetKeyDown(keyMap[KeyName.Interact]);

    public bool UseCharmPressed() => Input.GetKey(keyMap[KeyName.UseCharm]);
    public bool InventoryPressed() => Input.GetKeyDown(keyMap[KeyName.Inventory]);

    public bool PausePressed() => Input.GetKeyDown(keyMap[KeyName.Pause]);

    public void SetKey(KeyName keyName, KeyCode newKey)
    {
        foreach(var binding in keyBindings)
        {
            if(binding.keyName.Equals(keyName))
            {
                binding.keyCode = newKey;
                keyMap[binding.keyName] = newKey;
                return;
            }    
        }
    }
}

public enum KeyName
{
    None,
    Left,
    Right,
    Jump,
    Dash,
    Attack,
    Interact,
    UseCharm,
    Inventory,
    Pause
}

[System.Serializable]
public class KeyBinding
{
    public KeyName keyName;
    public KeyCode keyCode;
}