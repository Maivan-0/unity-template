using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyVisual enemyVisual;
    [SerializeField] private EnemyPatrol enemyPatrol;

    [Header("Attack Settings")]
    [SerializeField] private float rangedAttackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int launchForce;
    [SerializeField] private int collisionDamage;
    [SerializeField] private int knockbackForce;

    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;

    [Header("Detection")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Effects")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Internal")]
    private int _direction;
    private float cooldownTime = Mathf.Infinity;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        boxCollider = GetComponent<BoxCollider2D>();
        enemyPatrol = GetComponentInChildren<EnemyPatrol>();
        enemyVisual = GetComponentInChildren<EnemyVisual>();
    }

    private void Update()
    {
        if (playerObject == null || player == null || playerHealth == null)
            UpdateComponents();

        bool inSight = EnemyInSight();

        cooldownTime += Time.deltaTime;
        if (inSight && cooldownTime >= rangedAttackCooldown)
        {
            cooldownTime = 0;
            enemyVisual.EnemyAttack();
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !inSight;
        }

        _direction = transform.localScale.x > 0 ? 1 : -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
                playerHealth.TakePlayerHit(collisionDamage);

            if (player != null)
                player.Knockback(transform, knockbackForce);

            if (playerObject != null && impulseSource != null)
            {
                Vector3 shakeDirection = (playerObject.transform.position - transform.position).normalized;
                CameraShake.instance.CameraShakeForce(impulseSource, shakeDirection);
            }
            else
            {
                CameraShake.instance.CameraShakeForce(impulseSource, Vector3.zero);
            }

            Vector2 separationForce = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.AddForce(separationForce * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void UpdateComponents()
    {
        if (playerObject == null)
            playerObject = PlayerManager.instance.GetPlayerObject();
        if (player == null)
            player = PlayerManager.instance.GetPlayer();
        if (playerHealth == null)
            playerHealth = PlayerManager.instance.GetPlayerHealth();
    }

    private bool EnemyInSight()
    {
        RaycastHit2D _hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        return _hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void ShootProjectileAtPlayer()
    {
        if (EnemyInSight())
        {
            GameObject newArrow = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
            Rigidbody2D rigidbody = newArrow.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(launchForce * _direction, 0);
        }
    }
}
