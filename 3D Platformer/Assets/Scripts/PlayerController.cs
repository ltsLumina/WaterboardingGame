#region
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Running")]
    [SerializeField] float topSpeed = 10f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float deceleration = 2f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 25f;
    [SerializeField] float jumpCancellationForce = 5f;
    [SerializeField] float gravityMultiplier = 1.25f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.5f;

    [Header("Dashing"), SerializeField, Tooltip("X is force forwards, Y is force upwards.")]
    Vector2 dashForce = new (25f, 10f);
    [SerializeField] float minDashDuration = 0.5f;
    [SerializeField] float dashDuration = 1f;

    [Header("GroundCheck")]
    [SerializeField] Vector3 groundCheckPosition = new (0f, -1f, 0f);
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayers;
    readonly Collider[] groundCheckHits = new Collider[10]; //Only used as storage for optimization purposes
    Coroutine activeJumpBuffer;
    bool canDash;
    Animator characterAnimator;
    float coyoteTimeCounter;
    float currentSpeed;
    float fallingTimer;
    bool isDashing;
    bool isGrounded;
    bool isJumping;
    bool landingLock;
    Transform mainCamera;
    Vector2 movementInput;

    //Cached references
    Rigidbody myRigidbody;
    readonly static int dashing = Animator.StringToHash("isDashing");
    readonly static int grounded = Animator.StringToHash("isGrounded");

    void Start()
    {
        myRigidbody       = GetComponent<Rigidbody>();
        if (Camera.main != null) mainCamera = Camera.main.transform;
        characterAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Rotation();
        UpdateCoyoteTimeCounter();
        UpdateAnimationStates();
    }

    void FixedUpdate()
    {
        GroundCheck();
        ApplyGravityMultiplier();
        Movement();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckPosition, groundCheckRadius);
    }

    void GroundCheck()
    {
        isGrounded = Physics.OverlapSphereNonAlloc(transform.position + groundCheckPosition, groundCheckRadius,
                                                   groundCheckHits, groundLayers) != 0;

        canDash = true;
    }

    void ApplyGravityMultiplier() { myRigidbody.velocity += Vector3.down * gravityMultiplier; }

    #region Animation updates from Update
    void UpdateAnimationStates()
    {
        //The "animation update - code" needs refactoring. This is not a priority.
        characterAnimator.SetBool(dashing, isDashing);

        if (isGrounded)
        {
            characterAnimator.SetBool(grounded, true);

            if (!landingLock && fallingTimer > 0.3f)
            {
                landingLock = true;
                characterAnimator.SetTrigger("landed");
            }

            fallingTimer = 0;
        }
        else
        {
            characterAnimator.SetBool("isGrounded", false);
            landingLock  =  false;
            fallingTimer += Time.deltaTime;
        }

        if (movementInput != Vector2.zero && isGrounded) characterAnimator.SetBool("isRunning", true);
        else characterAnimator.SetBool("isRunning", false);

        if (isGrounded)
        {
            characterAnimator.SetBool("isFalling", false);
            characterAnimator.SetBool("isJumpingUp", false);
            return;
        }

        characterAnimator.SetBool("isJumpingUp", myRigidbody.velocity.y > 1);

        characterAnimator.SetBool("isFalling", myRigidbody.velocity.y < 0);
    }
    #endregion

    #region Called from Update
    void Rotation()
    {
        if (isDashing) return;
        if (movementInput.magnitude < Mathf.Epsilon) return;

        float angle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
        myRigidbody.rotation = Quaternion.Euler(0, angle, 0);
    }

    void UpdateCoyoteTimeCounter()
    {
        coyoteTimeCounter += isGrounded && !isJumping ? -coyoteTimeCounter : Time.deltaTime;
    }
    #endregion

    #region Movement
    void OnMove(InputValue value) { movementInput = value.Get<Vector2>(); }

    void Movement()
    {
        if (isDashing) return;

        float targetSpeed = movementInput.magnitude < Mathf.Epsilon ? 0 : topSpeed;
        float t           = targetSpeed             == 0 ? deceleration : acceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, t * Time.fixedDeltaTime);

        Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 movementVelocity  = movementDirection * currentSpeed + Vector3.up * myRigidbody.velocity.y;
        myRigidbody.velocity = movementVelocity;
    }
    #endregion

    #region Jumping
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (activeJumpBuffer != null) StopCoroutine(activeJumpBuffer);
            activeJumpBuffer = StartCoroutine(JumpBufferRoutine());
        }
        else { StartCoroutine(CancelJumpRoutine()); }
    }

    void Jump()
    {
        isJumping            =  true;
        coyoteTimeCounter    =  coyoteTime;
        myRigidbody.velocity -= Vector3.up * myRigidbody.velocity.y;
        myRigidbody.AddForce(jumpForce * Vector3.up, ForceMode.VelocityChange);

        //Trigger squash and stretch animation and particle effect
        characterAnimator.SetTrigger("squashJump");
    }

    IEnumerator JumpBufferRoutine()
    {
        float timeSincePressed = 0f;

        while (timeSincePressed < jumpBufferTime)
        {
            if (coyoteTimeCounter < coyoteTime)
            {
                Jump();
                break;
            }

            timeSincePressed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        activeJumpBuffer = null;
    }

    IEnumerator CancelJumpRoutine()
    {
        while (myRigidbody.velocity.y > 0f)
        {
            myRigidbody.velocity += Vector3.down * jumpCancellationForce;
            yield return new WaitForFixedUpdate();
        }

        isJumping = false;
    }
    #endregion

    #region Dashing
    void OnDash(InputValue value)
    {
        if (!value.isPressed || !canDash || isDashing) return;

        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        isDashing               = true;
        myRigidbody.isKinematic = true;
        yield return new WaitForSecondsRealtime(0.15f);
        myRigidbody.isKinematic = false;

        myRigidbody.velocity = Vector3.zero;
        myRigidbody.velocity = transform.forward * dashForce.x + transform.up * dashForce.y;

        for (float t = 0; t < dashDuration; t += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            if (t < minDashDuration) continue;

            if (isGrounded) break;
        }

        isDashing = false;
    }
    #endregion
}