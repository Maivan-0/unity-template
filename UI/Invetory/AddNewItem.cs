using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypePoint
{
    Iron,
    Gold,
    Ametist
}

public class AddNewItem : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Item item;
    [SerializeField] private GameObject objectD;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private TypePoint typePoint;

    private void Start()
    {
        if (inventoryManager == null)
        {
            GameObject canvasObj = GameObject.Find("Canvas");
            if (canvasObj != null)
            {
                inventoryManager = canvasObj.GetComponentInChildren<InventoryManager>();
            }
        }

        if (skillManager == null)
        {
            GameObject canvasObj = GameObject.Find("Canvas");
            if (canvasObj != null)
            {
                skillManager = canvasObj.GetComponentInChildren<SkillManager>();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (typePoint)
            {
                case TypePoint.Iron: 
                    skillManager.AddIronPoint(); 
                    break;
                case TypePoint.Gold: 
                    skillManager.AddGoldPoint();
                    break;
                case TypePoint.Ametist: 
                    skillManager.AddAmetistPoint();
                    break;
            } 

            inventoryManager.Add(item);
            Destroy(objectD);
        }
    }
}