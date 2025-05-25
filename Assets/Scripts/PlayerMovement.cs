using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]



public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Collider2D capsuleCollider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    [Header("滑动设置")]
    public float slideAccelerationMultiplier = 2.5f; // 滑动时的加速度倍增器
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        capsuleCollider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        capsuleCollider.enabled = false;
        velocity = Vector2.zero;
        inputAxis = 0f;
        jumping = false;
    }

    private void Update()
    {
        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);

        if (grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        // Move mario based on his velocity
        Vector2 position = rb.position;
        position += velocity * Time.fixedDeltaTime;

        // Clamp within the screen bounds
        Vector2 leftEdge = mainCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rb.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        // 获取输入
        inputAxis = Input.GetAxis("Horizontal");

        // 判断是否处于滑动状态（方向切换）
        bool isSliding = (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

        // 应用加速度，滑动时使用更高的加速率
        float currentAcceleration = moveSpeed;
        if (isSliding)
        {
            currentAcceleration *= slideAccelerationMultiplier; // 滑动时加速更快
        }

        // 加速/减速
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, currentAcceleration * Time.deltaTime);

        // 检查是否撞墙
        if (rb.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        // 翻转角色面向
        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }
    private void GroundedMovement()
    {
        // Prevent gravity from infinitly building up
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // Perform jump
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
            AudioManager.Instance.PlaySound("Jump", transform.position);
        }
    }

    private void ApplyGravity()
    {
        // Check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        // Apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Bounce off enemy head
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // Stop vertical movement if mario bonks his head
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }

}
