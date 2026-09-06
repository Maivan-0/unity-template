using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private int _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == "Player" && _collision.transform.TryGetComponent(out PlayerHealth _playerHealth))
        {
            _playerHealth.TakePlayerHit(_damage);
        }
        if (_collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
