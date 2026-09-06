using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private int _arrowDamage;
    [SerializeField] private int _lifeTime;
    [SerializeField] private string _enemyTag;
    [SerializeField] private float knockbackForce;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private Rigidbody2D _rigidbody2D;
    private bool _hasHit = true;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        if (_hasHit)
        {
            Vector2 _direction = _rigidbody2D.velocity;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_hasHit) return;

        if (_collision.gameObject.CompareTag(_enemyTag) && _collision.transform.TryGetComponent(out PlayerHealth _playerHealth) && _collision.transform.TryGetComponent(out Player _player))
        {
            _hasHit = true;

            _playerHealth.TakePlayerHit(_arrowDamage);
            _player.Knockback(transform, knockbackForce);

            Vector3 shakeDirection = (_collision.transform.position - transform.position).normalized;
            CameraShake.instance.CameraShakeForce(impulseSource, shakeDirection);
        }

        Destroy(gameObject);
    }
}
