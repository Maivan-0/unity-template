using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public static PlayerJump Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private float _jumpforce = 20f;
    [SerializeField] private Rigidbody2D _rigidbody2d;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private bool _isAttack = true;

    [Header("Wall Slide")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private Animator _animator;
    private bool isWallDetected;
    private bool canWallS1ide;
    private bool isWallSliding;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 16f);
    private float wallJumpDirection;
    public bool isWallJump;


    private void Awake()
    {
        Instance = this;
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (PauseMenu.instance.isPaused) return;

        if (Input.GetButtonDown("Jump") && IsGrounded() && _isAttack && !(player.isDashing))
        {
            _rigidbody2d.AddForce(new Vector2(_rigidbody2d.velocity.x, _jumpforce ), ForceMode2D.Impulse);
        }

        WallJump();
    }

    private void FixedUpdate()
    {
        isWall();

        if (isWallDetected && canWallS1ide)
        {
            isWallSliding = true;
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        _animator.SetBool("Wall Slide", isWallSliding);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + (player._faceRight ? wallCheckDistance : -wallCheckDistance), wallCheck.position.y, wallCheck.position.z));
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer | _enemyLayer);
    }

    public void isWall()
    {
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallCheckDistance, _groundLayer);
        canWallS1ide = !IsGrounded() && _rigidbody2d.velocity.y < 0 && isWallDetected;
    }

    public void IsAttackingOn()
    {
        _isAttack = false;
    }

    public void IsAttackingOff()
    {
        _isAttack = true;
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJump = false;
            wallJumpDirection = player._faceRight ? -1f : 1f;

            CancelInvoke(nameof(StopWallJump));
        }

        if(Input.GetButtonDown("Jump") && isWallSliding)
        {
            isWallJump = true;
            _rigidbody2d.velocity = new Vector2(wallJumpDirection * -wallJumpPower.x, wallJumpPower.y);

            Vector3 scale = transform.localScale;
            if ((wallJumpDirection > 0f && scale.x < 0f) || (wallJumpDirection < 0f && scale.x > 0f))
            {   
                scale.x *= -1f;
                transform.localScale = scale;
                player._faceRight = !player._faceRight;
            }

            Invoke(nameof(StopWallJump), wallJumpDuration);
        }
    }

    private void StopWallJump()
    {
        isWallJump = false;
    }
}