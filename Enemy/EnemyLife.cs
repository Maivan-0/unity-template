using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Rigidbody2D rb;
    public bool isDeath = false;

    [Header("Visual & Effects")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Flash _flash;
    [SerializeField] private ParticleSystem _particleSystem;
    public bool isUnderEffect = false;

    [Header("References")]
    [SerializeField] private EnemyPatrol _enemyPatrol;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration;

    [Header("LifeEssence")]
    [SerializeField] private int minEssence;
    [SerializeField] private int maxEssence;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        _animator = GetComponentInChildren<Animator>();
        _enemyPatrol = GetComponentInChildren<EnemyPatrol>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        _flash = GetComponentInChildren<Flash>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();

        if (_particleSystem != null)
            _particleSystem.Stop();
    }

    private void Update()
    {
        if (playerObject == null || playerHealth == null)
            UpdateComponents();
    }

    private void UpdateComponents()
    {
        if (playerObject == null)
            playerObject = PlayerManager.instance.GetPlayerObject();
        if (playerHealth == null)
            playerHealth = PlayerManager.instance.GetPlayerHealth();
    }

    public void TakeEnemyHit(int _damage)
    {
        if (isDeath) return;

        _health -= _damage;

        if (_health <= 0)
        {
            isDeath = true;

            if(playerHealth != null)
                playerHealth.AddLifeEssence(Random.Range(minEssence, maxEssence + 1));

            rb.isKinematic = true;
            boxCollider.enabled = false;
            _animator.SetTrigger("Death");
        }
        else
        {
            _flash.FlashEffect();
            StartCoroutine(KnockbackPause());

            if (playerObject != null && impulseSource != null)
            {
                Vector3 shakeDirection = (playerObject.transform.position - transform.position).normalized;
                CameraShake.instance.CameraShakeForce(impulseSource, shakeDirection);
            }
            else
            {
                CameraShake.instance.CameraShakeForce(impulseSource, Vector3.zero);
            }
        }
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
    }

    public void Knockback(Transform playerTransform, float knockbackForce)
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;
    }

    private IEnumerator KnockbackPause()
    {
        _enemyPatrol.enabled = false;
        yield return new WaitForSeconds(knockbackDuration);
        _enemyPatrol.enabled = true;
    }

    public IEnumerator ApplyDamageOverTime(int damage, int tickCount, float tickInterval, Gradient gradient)
    {
        isUnderEffect = true;
        if (_particleSystem != null)
        {
            var main = _particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(gradient);
            _particleSystem.Play();
        }

        for (int i = 0; i < tickCount; i++)
        {
            yield return new WaitForSeconds(tickInterval);
            if (_health > 0)
                TakeEnemyHit(damage);
            else
                break;
        }

        if (_particleSystem != null)
            _particleSystem.Stop();
        isUnderEffect = false;
    }
}
