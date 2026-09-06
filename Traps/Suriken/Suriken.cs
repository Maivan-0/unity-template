using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Suriken : MonoBehaviour
{
    [Header("Suriken")]
    [SerializeField] private float _movementDistance;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    private bool _movingLeft;
    private float _rightEdge;
    private float _leftEdge;

    private void Awake()
    {
        _rightEdge = transform.position.x + _movementDistance;
        _leftEdge = transform.position.x - _movementDistance;
    }

    private void Update()
    {
        if (_movingLeft)
        {
            if (transform.position.x > _leftEdge)
            {
                transform.position = new Vector3(transform.position.x - _speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                _movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < _rightEdge)
            {
                transform.position = new Vector3(transform.position.x + _speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                _movingLeft = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            _collision.GetComponent<PlayerHealth>().TakePlayerHit(_damage);
        }
    }
}
