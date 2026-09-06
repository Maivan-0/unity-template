using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _enemy;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyLife enemyLife;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerStats playerStats;

    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 patrolIntervalTime;
    private float patrolInterval;
    private float patrolTimer;
    private Vector3 _initScale;
    private bool _movingLeft = true;

    [Header("Idle State")]
    [SerializeField] private float _idleDuration;
    private bool isIdle = false;

    [Header("Player Follow Settings")]
    [SerializeField] private int _horizontalChaseRadius;
    [SerializeField] private int _verticalChaseLimit;
    [SerializeField] private float _idleFollowDuration;
    [SerializeField] private float detectionRangeReduction;
    private float _horizontalDistanceToPlayer;
    private float _verticalDistanceToPlayer;
    private bool isChangingDirection;

    [Header("Environment Checks")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _groundLayer;

    private void Start()
    {
        enemyLife = GetComponentInParent<EnemyLife>();

        patrolInterval = Random.Range(patrolIntervalTime.x, patrolIntervalTime.y);
        _initScale = _enemy.localScale;
    }

    private void OnDisable()
    {
        _animator.SetBool("Run", false);
    }

    private void Update()
    {
        if (playerTransform == null || playerStats == null)
            UpdateComponents();

        if(enemyLife != null && enemyLife.isDeath)
        {
            _animator.SetBool("Run", false);
            return;
        }

        if(playerStats != null)
            detectionRangeReduction = playerStats.detectionRangeReduction;

        if (playerTransform != null)
        {
            _horizontalDistanceToPlayer = Vector2.Distance(_enemy.position, playerTransform.position);
            _verticalDistanceToPlayer = Mathf.Abs(_enemy.position.y - playerTransform.position.y);

            if (_horizontalDistanceToPlayer < (_horizontalChaseRadius * detectionRangeReduction) && _verticalDistanceToPlayer < (_verticalChaseLimit * detectionRangeReduction))
            {
                PlayerFollow();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Gizmos.DrawWireCube(transform.position, new Vector3((_horizontalChaseRadius * 2 * detectionRangeReduction), (_verticalChaseLimit * 2 * detectionRangeReduction), 0.1f));
    }

    private void UpdateComponents()
    {
        if (playerTransform == null)
            playerTransform = PlayerManager.instance.GetPlayerTransform();
        if (playerStats == null)
            playerStats = PlayerManager.instance.GetPlayerStats();
    }

    private void PlayerFollow()
    {
        if(IsTouchingWall())
        {
            _animator.SetBool("Run", false);
            return;
        }

        if (_movingLeft)
        {
            if (_enemy.position.x >= playerTransform.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                if(!isChangingDirection)
                    StartCoroutine(DirectionChange(_idleFollowDuration));
            }
        }
        else
        {
            if (_enemy.position.x <= playerTransform.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                if (!isChangingDirection)
                    StartCoroutine(DirectionChange(_idleFollowDuration));
            }
        }
    }

    private void Patrol()
    {
        if (isIdle) return;

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolInterval)
        {
            StartCoroutine(PauseBeforeDirectionChange(_idleDuration));
            patrolInterval = Random.Range(patrolIntervalTime.x, patrolIntervalTime.y);
            patrolTimer = 0;
            return;
        }
        if (_movingLeft)
        {
            if (!IsGrounded() || IsTouchingWall())
            {
                FlipEnemy();
            }
            else
            {
                MoveInDirection(-1);
            }
        }
        else
        {
            if (!IsGrounded() || IsTouchingWall())
            {
                FlipEnemy();
            }
            else
            {
                MoveInDirection(1);
            }
        }
    }

    private IEnumerator PauseBeforeDirectionChange(float duration)
    {
        isIdle = true;
        _animator.SetBool("Run", false);
        yield return new WaitForSeconds(duration);
        _animator.SetBool("Run", true);
        isIdle = false;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    public bool IsTouchingWall()
    {
        return Physics2D.OverlapCircle(_wallCheck.position, 0.2f, _groundLayer);
    }

    private IEnumerator DirectionChange(float idleDuration)
    {
        if (isChangingDirection) yield break;

        isChangingDirection = true;

        isIdle = true;
        _animator.SetBool("Run", false);
        yield return new WaitForSeconds(idleDuration);
        FlipEnemy();
        isIdle = false;

        isChangingDirection = false;
    }

    private void MoveInDirection(int _direction)
    {
        _animator.SetBool("Run", true);
        _enemy.localScale = new Vector3(Mathf.Abs(_initScale.x) * _direction, _initScale.y, _initScale.z);

        _enemy.position = new Vector3(_enemy.position.x + Time.deltaTime * _direction * _speed, _enemy.position.y, _enemy.position.z);
    }

    private void FlipEnemy()
    {
        _movingLeft = !_movingLeft;
        _enemy.localScale = new Vector3(_enemy.localScale.x * -1, _enemy.localScale.y, _enemy.localScale.z);
    }
}
