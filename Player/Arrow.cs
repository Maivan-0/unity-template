using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow")]
    [SerializeField] private int _arrowDamage;
    [SerializeField] private float _knockbackStrength;
    [SerializeField] private int _lifeTime;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private string _enemyTag;
    private bool _hasHit = true;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        if (_hasHit)
        {
            Vector2 _direction = _rigidbody2D.velocity;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == _enemyTag && _collision.transform.TryGetComponent(out EnemyLife _enemyLife))
        {
            _enemyLife.TakeEnemyHit(_arrowDamage);
            Destroy(gameObject);
        }

        Rigidbody2D _knockback = _collision.gameObject.GetComponent<Rigidbody2D>();

        if (_knockback != null)
        {
            Vector2 _knockbackDirection = _collision.transform.position - transform.position;
            _knockbackDirection.Normalize();
            _knockback.AddForce(_knockbackDirection * _knockbackStrength, ForceMode2D.Impulse);
        }

        _hasHit = false;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.isKinematic = true;

        BoxCollider2D _collider2D = GetComponent<BoxCollider2D>();
        _collider2D.enabled = false;

        Destroy(gameObject, _lifeTime / 2);
    }
}
