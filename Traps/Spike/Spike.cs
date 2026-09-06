using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Spike")]
    [SerializeField] private int _damage;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private GameObject _player;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private SafeGroundCheckPoint safeGroundCheckPoint;

    private void Start()
    {
        safeGroundCheckPoint = _player.GetComponent<SafeGroundCheckPoint>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
            playerHealth.TakePlayerHit(_damage);

            Vector3 shakeDirection = (_player.transform.position - transform.position).normalized;
            CameraShake.instance.CameraShakeForce(impulseSource, shakeDirection);

            safeGroundCheckPoint.WarpPlayerToSafeGround();
        }
    }
}
