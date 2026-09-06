using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Player Objects")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform playerTransform;

    [Header("Player Components")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.Find("Player");
        }
        else
        {
            playerTransform = playerObject.transform;
            player = playerObject.GetComponent<Player>();
            playerHealth = playerObject.GetComponent<PlayerHealth>();
            playerStats = playerObject.GetComponent<PlayerStats>();
        }
    }

    public GameObject GetPlayerObject() { return playerObject; }

    public Transform GetPlayerTransform() { return playerTransform; }

    public Player GetPlayer() { return player; }

    public PlayerHealth GetPlayerHealth() { return playerHealth; }

    public PlayerStats GetPlayerStats() { return playerStats; }
}
