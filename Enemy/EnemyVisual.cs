using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;
    [SerializeField] private RangedEnemyAttack rangedEnemyAttack;
    [SerializeField] private EnemyLife enemyLife;
    [SerializeField] private Material material;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Dissolve Settings")]
    [SerializeField] private float dissolveTime;

    private int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        meleeEnemyAttack = GetComponentInParent<MeleeEnemyAttack>();
        rangedEnemyAttack = GetComponentInParent<RangedEnemyAttack>();

        enemyLife = GetComponentInParent<EnemyLife>();

        material = spriteRenderer.material;
    }

    public IEnumerator Dissolve()
    {
        float elapsedTime = 0f;

        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0f, 1f, (elapsedTime / dissolveTime));

            material.SetFloat(dissolveAmount, lerpedDissolve);

            yield return null;
        }

        enemyLife.EnemyDeath();
    }

    public void EnemyAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void TakePlayerDamage()
    {
        if(meleeEnemyAttack != null)
        {
            meleeEnemyAttack.DamagePlayer();
        }

        if (rangedEnemyAttack != null)
        {
            rangedEnemyAttack.ShootProjectileAtPlayer();
        }
    }
}
