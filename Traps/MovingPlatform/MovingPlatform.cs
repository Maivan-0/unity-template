using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Square point")]
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;

    [Header("Platform")]
    [SerializeField] private float _speed;
    private Vector2 _targetPosition;

    private void Start()
    {
        _targetPosition = _left.position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _right.position) < .1f)
        {
            _targetPosition = _left.position;
        }
        if (Vector2.Distance(transform.position, _left.position) < .1f)
        {
            _targetPosition = _right.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            _collision.transform.SetParent(this.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            _collision.transform.SetParent(null);
        }
    }
}
