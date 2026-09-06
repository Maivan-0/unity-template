using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAttack : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private int _nrClicks;
    [SerializeField] private float _lastClickedTime;
    [SerializeField] private float _maxComboDelay = 0.8f;
    [SerializeField] public int _meleeDamage;
    [SerializeField] private float knockbackForce;

    [Header("Animator")]
    [SerializeField] private Animator _animator;
    private const string ATTACK1 = "Attack1";
    private const string ATTACK2 = "Attack2";

    [Header("Collider2D")]
    [SerializeField] private string _enemyTag;
    [SerializeField] private Collider2D _attack1Collider2D;
    [SerializeField] private Collider2D _attack2Collider2D;

    [Header("Burning")]
    [SerializeField] private int burnDamage;
    [SerializeField] private int burnTickCount;
    [SerializeField] private float burnTickInterval;
    [SerializeField] private Gradient burnGradient;
    private bool canBurn;

    [Header("Poison")]
    [SerializeField] private int poisonDamage;
    [SerializeField] private int poisonTickCount;
    [SerializeField] private float poisonTickInterval;
    [SerializeField] private Gradient poisonGradient;
    private bool canPoison;

    private PlayerStats playerStats;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem != null)
            particleSystem.Stop();
    }

    private void Update()
    {
        if (PauseMenu.instance.isPaused) return;

        UpdateDate();

        if (Time.time - _lastClickedTime > _maxComboDelay)
        {
            _nrClicks = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _lastClickedTime = Time.time;
            _nrClicks++;
            if (_nrClicks == 1)
            {
                _animator.SetTrigger(ATTACK1);
            }
            _nrClicks = Mathf.Clamp(_nrClicks, 0, 2);
        }
    }

    public void UpdateDate()
    {
        _meleeDamage = PlayerStats.Instance.meleeDamage;
        canBurn = PlayerStats.Instance.isBurning;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.tag == _enemyTag && _collision.transform.TryGetComponent(out EnemyLife _enemyLife))
        {
            _enemyLife.TakeEnemyHit(_meleeDamage);

            if(!_enemyLife.isDeath)
            {
                _enemyLife.Knockback(transform, knockbackForce);
            }

            playerStats.ApplyVampireEffect();

            if (canBurn && !_enemyLife.isUnderEffect)
                StartCoroutine(_enemyLife.ApplyDamageOverTime(burnDamage, burnTickCount, burnTickInterval, burnGradient));

            if (canPoison && !_enemyLife.isUnderEffect)
                StartCoroutine(_enemyLife.ApplyDamageOverTime(poisonDamage, poisonTickCount, poisonTickInterval, poisonGradient));
        }

        if(_collision.gameObject.tag == "Ore" && _collision.transform.TryGetComponent(out Ore ore))
        {
            ore.Hit();
        }
    }

    public void return1()
    {
        if (_nrClicks >= 2)
        {
            _animator.SetBool(ATTACK2, true);
        }
        else
        {
            _nrClicks = 0;
        }
    }

    public void return2()
    {
        _animator.SetBool(ATTACK2, false);
        _nrClicks = 0;
    }

    public void Attack1PoligonColliderOn()
    {
        _attack1Collider2D.enabled = true;
    }

    public void Attack1PoligonColliderOff()
    {
        _attack1Collider2D.enabled = false;
    }

    public void Attack2PoligonColliderOn()
    {
        _attack2Collider2D.enabled = true;
    }

    public void Attack2PoligonColliderOff()
    {
        _attack2Collider2D.enabled = false;
    }

    private void ChangeColor(Color colorA, Color colorB)
    {
        var main = particleSystem.main;

        Gradient gradient = new Gradient();

        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(colorA, 0.0f),
                new GradientColorKey(colorB, 1.0f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
            }
        );

        main.startColor = new ParticleSystem.MinMaxGradient(gradient);
    }
}
