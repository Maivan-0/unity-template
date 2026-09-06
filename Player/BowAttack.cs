using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAttack : MonoBehaviour
{
    [Header("Bow")]
    [SerializeField] private GameObject _arrow;
    [SerializeField] private int _launchForce; 
    [SerializeField] private Vector2 _angle;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private Transform _player;
    private bool _isAttacking = true;
    private int _direction;

    [Header("Animator")]
    [SerializeField] Animator _animator;


    private void Update()
    {
        if (_player.transform.localScale.x < 0)
        {
            _direction = 1;
        }
        else
        {
            _direction = -1;
        }
        if (Input.GetMouseButtonDown(0) && _isAttacking)
        {
            _isAttacking = false;
            _animator.SetTrigger("BowAttack");
        }
    }

    public void ShotArrow()
    {
        GameObject _newArrow = Instantiate(_arrow, _shotPoint.position, _shotPoint.rotation);
        Rigidbody2D _rigidbody2D = _newArrow.GetComponent<Rigidbody2D>();
        float angleRad = Random.Range(_angle.x, _angle.y) * Mathf.Deg2Rad * _direction;
        Vector2 _velocity = new Vector2(Mathf.Cos(angleRad) * _launchForce, Mathf.Sin(angleRad) * _launchForce);
        _rigidbody2D.velocity = _velocity * _direction;
        _isAttacking = true;
    }
}
