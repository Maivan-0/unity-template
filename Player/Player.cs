using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    private float _movingSpeed;
    private Rigidbody2D rb;
    private Vector2 _moveVector;
    private float _minMovingSpeed = 0;
    private bool _run = false;
    public bool _faceRight = false;
    private int _isAttack = 1;
    [SerializeField] private float _knockbackDuration;
    private bool _isKnockedBack = false;
    private float _knockbackTimer;
    private float _knockbackResistance = 0;
    private PlayerHealth playerHealth;

    [Header("Dash")]
    [SerializeField] private bool canDash = true;
    [SerializeField] public bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private float trailTime;
    [SerializeField] GameObject windTrailObject;
    [SerializeField] private Animator animator;

    [Header("Skill")]
    [SerializeField] public bool unlockDash = false;

    private ParticleSystem windTrail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        windTrail = windTrailObject.GetComponent<ParticleSystem>();
        windTrail.Stop();

        playerHealth = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        if (PauseMenu.instance.isPaused) return;

        UpdateDate();

        if(isDashing)
        {
            return;
        }

        if (!_isKnockedBack && !PlayerJump.Instance.isWallJump)
        {
            PlayerRun();

            if(Input.GetKeyDown(KeyCode.LeftShift) && canDash && unlockDash)
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            _knockbackTimer -= Time.fixedDeltaTime;
            if (_knockbackTimer <= 0)
            {
                _isKnockedBack = false;
            }
        }
    }

    public void UpdateDate()
    {
        _movingSpeed = PlayerStats.Instance.movingSpeed;
        _knockbackResistance = PlayerStats.Instance.knockbackResistance;
    }

    public void IsAttackingOn()
    {
        _isAttack = 0;
    }

    public void IsAttackingOff()
    {
        _isAttack = 1;
    }

    public bool Run()
    {
        return _run;
    }

    public void Knockback(Transform enemyTransform, float knockbackForce)
    {
        return;
        Vector2 direction = (transform.position - enemyTransform.position).normalized;
        rb.velocity = direction * (knockbackForce - _knockbackResistance);
        _isKnockedBack = true;
        _knockbackTimer = _knockbackDuration;
    }

    private void PlayerRun()
    {
        _moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(_moveVector.x * _movingSpeed * _isAttack, rb.velocity.y);

        if (Mathf.Abs(_moveVector.x) > _minMovingSpeed || Mathf.Abs(_moveVector.y) > _minMovingSpeed)
        {
            _run = true;
        }
        else
        {
            _run = false;
        }
        if ((_moveVector.x < 0 && !_faceRight) || (_moveVector.x > 0 && _faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            _faceRight = !_faceRight;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetTrigger("Dash");
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(_faceRight ? -dashingPower : dashingPower, 0f);
        windTrail.Play();
        yield return new WaitForSeconds(dashingTime);
        windTrail.Stop();
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}