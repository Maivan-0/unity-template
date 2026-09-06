using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("PlayerComponent")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CombatAttack combatAttack;

    [Header("PlayerStats")]
    [Header("Player")]
    [SerializeField] public float movingSpeed;
    [SerializeField] public float knockbackForce;
    [SerializeField] public float knockbackResistance = 0;
    [SerializeField] public float detectionRangeReduction = 1f;

    [Header("PlayerHealth")]
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int damageResistance = 0;
    [SerializeField] public bool vampirismEffect;
    [SerializeField] public bool canUseEssenceHeal;

    [Header("CombatAttack")]
    [SerializeField] public int meleeDamage;
    [SerializeField] public int vampirismStrength;
    [SerializeField] public bool isBurning;

    private void Awake()
    {
        Instance = this;
        player = GetComponent<Player>();
        playerHealth = GetComponent<PlayerHealth>();
        combatAttack = GetComponentInChildren <CombatAttack>();

        detectionRangeReduction = 1f;
    }

    public void ModifyMaxHealth(int amount, bool apply)
    {
        if (apply)
            maxHealth += amount;
        else
            maxHealth -= amount;
    }

    public void ModifyDamageResistance(int resistance, bool apply)
    {
        if (apply)
            damageResistance += resistance;
        else
            damageResistance -= resistance;
    }

    public void ModifyMeleeDamage(int damage, bool apply)
    {
        if (apply)
            meleeDamage += damage;
        else
            meleeDamage -= damage;
    }

    public void ModifySpeed(int amount, bool apply)
    {
        if (apply)
            movingSpeed += amount;
        else
            movingSpeed -= amount;
    }

    public void ModifyVampireEffect(bool apply)
    {
        vampirismEffect = apply;
    }

    public void ApplyVampireEffect()
    {
        playerHealth.ApplyVampireEffect(vampirismEffect, vampirismStrength);
    }

    public void ModifyKnockbackResistance(int resistance, bool apply)
    {
        if (apply)
            knockbackResistance += resistance;
        else
            knockbackResistance -= resistance;
    }

    public void ModifyChaseRadius(float distance,  bool apply)
    {
        if(apply)
            detectionRangeReduction -= distance;
        else
            detectionRangeReduction += distance;  
    }

    public void ModifyBurning(bool apply)
    {
        isBurning = apply;
    }

    public void ModifyEssenceHeal(bool apply)
    {
        canUseEssenceHeal = apply;
    }

    public void UnlockDash()
    {
        player.unlockDash = true;
    }
}
