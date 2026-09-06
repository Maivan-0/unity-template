using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterArrow : MonoBehaviour
{
    [Header("ShooterArrow")]
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Animator _animator;
    private float _cooldownTimer;

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;
        if (_cooldownTimer >= _attackCooldown)
        {
            _cooldownTimer = 0;
            _animator.SetTrigger("Attack");
        }
    }

    public void ShootArrow()
    {
        GameObject _newArrow = Instantiate(_arrow, _firePoint.position, _firePoint.rotation);
    }
}
