using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Melee Attack")]
    [SerializeField] private bool EnemyNearby;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private int collisionDamage;
    [SerializeField] private int knockbackForce;

    [Header("Ranged Attack")]
    [SerializeField] private bool EnemyFar;
    [SerializeField] private float _rangedAttackCooldown;
    [SerializeField] private float _range;
    [SerializeField] private float _colliderDistance;
    [SerializeField] private int _launchForce;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private GameObject _projectile;
    private int _direction;

    [SerializeField] private GameObject playerPosition;
    [SerializeField] private Player player;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Collider2D")]
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Patrol")]
    [SerializeField] private EnemyPatrol _enemyPatrol;
    private float _cooldownTime = Mathf.Infinity;

    [Header("Health")]
    [SerializeField] private PlayerHealth _playerHealth;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Update()
    {
        _cooldownTime += Time.deltaTime;
        if (EnemyInSight())
        {
            if (_cooldownTime >= attackCooldown)
            {
                _cooldownTime = 0;
                animator.SetTrigger("Attack");
            }
        }

        if (_enemyPatrol != null)
        {
            _enemyPatrol.enabled = !EnemyInSight();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerHealth.TakePlayerHit(collisionDamage);
            player.Knockback(transform, knockbackForce);

            Vector2 separationForce = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.AddForce(separationForce * knockbackForce, ForceMode2D.Impulse);

            Vector3 shakeDirection = (playerPosition.transform.position - transform.position).normalized;
            CameraShake.instance.CameraShakeForce(impulseSource, shakeDirection);
        }
    }

    private bool EnemyInSight()
    {
        RaycastHit2D _hit = Physics2D.BoxCast(_boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z), 0, Vector2.left, 0, _enemyLayer);
        return _hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z));
    }

    public void DamagePlayer()
    {
        if (EnemyInSight())
        {
            _playerHealth.TakePlayerHit(damage);
            player.Knockback(transform, knockbackForce);
            Vector3 shakeDirection = (playerPosition.transform.position - transform.position).normalized;
            CameraShake.instance.CameraShakeForce(impulseSource, shakeDirection);
        }
    }
}
