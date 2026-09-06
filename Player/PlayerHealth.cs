using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private UnityEngine.Object _death;

    [Header("Invincible")]
    [SerializeField] private float _invulnerabilityDuration;
    [SerializeField] private int _numberFlashed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public bool _isInvincible = false;

    [Header("Respawn")]
    [SerializeField] private float fadeDuration;
    [SerializeField] private Vector2 respawnPosition;
    [SerializeField] private Animator animator;
    private bool isAlive = true;

    [Header("Other")]
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damageResistance;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _particleSystem;
    public bool isUnderEffect = false;

    [Header("LifeEssence")]
    [SerializeField] private KeyCode healKey;
    [SerializeField] private int lifeEssence;
    [SerializeField] private int maxLifeEssence;
    [SerializeField] private int essenceCost;
    [SerializeField] private int healAmount;
    [SerializeField] private float healHoldTime;
    [SerializeField] private Image lifeEssenceBar;
    private bool canUseEssenceHeal;
    private bool isHoldingHealKey;
    private float healTimer = 0f;

    private void Start()
    {
        respawnPosition = transform.position;
        uiManager = FindObjectOfType<UIManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateDate();
        UpdateUI();

        HandleHealInput();
    }

    public void UpdateDate()
    {
        _maxHealth = PlayerStats.Instance.maxHealth;
        damageResistance = PlayerStats.Instance.damageResistance;
        canUseEssenceHeal = PlayerStats.Instance.canUseEssenceHeal;
    }

    public void ApplyVampireEffect(bool vampirismEffect, int vampirismStrength)
    {
        if (!vampirismEffect) return;

        _health += vampirismStrength;

        if (_health > _maxHealth)
            _health = _maxHealth;
    }

    public void TakePlayerHit(int _damage)
    {
        if (!_isInvincible)
        {
            _health -= _damage - damageResistance;
            StartCoroutine(Invunerability());
        }

        if (_health <= 0 && isAlive)
        {
            StartCoroutine(Death());
        }
    }

    public void UpdateCheckPoint(Vector2 position)
    {
        respawnPosition = position;
    }

    private IEnumerator Invunerability()
    {
        _isInvincible = true;
        for (int i = 0; i < _numberFlashed; i++)
        {
            _spriteRenderer.color = new Color(1f, 1f, 1f, 0.7f);
            yield return new WaitForSeconds(_invulnerabilityDuration / (_numberFlashed * 2));
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(_invulnerabilityDuration / (_numberFlashed * 2));
        }
        _isInvincible = false;
    }

    private IEnumerator Death()
    {
        DeathEffect();
        player.IsAttackingOn();
        isAlive = false;
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(fadeDuration / 2);
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(fadeDuration * 2.5f);
        uiManager.RespawnScreen();
        transform.position = respawnPosition;
    }

    private void DeathEffect()
    {
        GameObject _deathReflect = (GameObject)Instantiate(_death);
        _deathReflect.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }

    public IEnumerator Respawn()
    {
        _spriteRenderer.enabled = true;
        _health = _maxHealth;
        yield return new WaitForSeconds(fadeDuration);
        animator.SetTrigger("End");
        player.IsAttackingOff();
        isAlive = true;
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
                TakePlayerHit(damage);
        }

        if (_particleSystem != null)
            _particleSystem.Stop();
        isUnderEffect = false;
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = (float)_health / _maxHealth;
        lifeEssenceBar.fillAmount = (float)lifeEssence / maxLifeEssence;
    }

    public void AddLifeEssence(int amount)
    {
        if ((lifeEssence + amount) > maxLifeEssence)
            lifeEssence = maxLifeEssence;
        else
            lifeEssence += amount;
    }

    private void HandleHealInput()
    {
        if (!canUseEssenceHeal || _health >= _maxHealth || lifeEssence < essenceCost)
        {
            if (_particleSystem.isPlaying)
                _particleSystem.Stop();

            return;
        }

        if(Input.GetKey(healKey))
        {
            healTimer += Time.deltaTime;

            if (!_particleSystem.isPlaying)
                _particleSystem.Play();

            if (!isHoldingHealKey && healTimer >= healHoldTime)
            {
                isHoldingHealKey = true;
                HealWithEssence();
            }
        }
        else
        {
            if (_particleSystem.isPlaying)
                _particleSystem.Stop();
        }
    }

    private void HealWithEssence()
    {
        lifeEssence -= essenceCost;
        _health += healAmount;
        _health = Mathf.Min(_health, _maxHealth);

        healTimer = 0;
        isHoldingHealKey = false;
    }
}
