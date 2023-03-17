using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Running")]
    [SerializeField] float topSpeed = 10f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float deacceleration = 2f;

    private Vector2 movementInput;
    private float currentSpeed = 0f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 25f;
    [SerializeField] float jumpCancellationForce = 5f;
    [SerializeField] float gravityMultiplier = 1.25f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.5f;

    private bool isJumping;
    private float coyoteTimeCounter = 0;
    private Coroutine activeJumpBuffer;
    private bool landingLock;
    private float fallingTimer;

    [Header("Dashing")]
    [SerializeField, Tooltip("X is force forwards, Y is force upwards.")]
    Vector2 dashForce = new Vector2(25f, 10f);
    [SerializeField] float minDashDuration = 0.5f;
    [SerializeField] float dashDuration = 1f;

    private bool isDashing;
    private bool canDash;

    [Header("GroundCheck")]
    [SerializeField] Vector3 groundCheckPosition = new Vector3(0f, -1f, 0f);
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayers;
    
    private bool isGrounded;
    private Collider[] grundCheckHits = new Collider[10]; //Only used as storage for optimization purposes

    //Cached references
    private Rigidbody myRigidbody;
    private Transform mainCamera;
    private Animator characterAnimator;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main.transform;
        characterAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Rotation();
        UpdateCoyoteTimeCounter();
        UpdateAnimationStates();
    }

    #region Called from Update
    private void Rotation()
    {
        if(isDashing) { return;  }
        if (movementInput.magnitude < Mathf.Epsilon) { return; }

        float angle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
        myRigidbody.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void UpdateCoyoteTimeCounter()
    {
        coyoteTimeCounter += (isGrounded && !isJumping) ? -coyoteTimeCounter : Time.deltaTime;
    }

    #endregion

    private void FixedUpdate()
    {
        GroundCheck();
        ApplyGravityMultiplier();
        Movement();
      
    }

    private void GroundCheck()
    {
        isGrounded = Physics.OverlapSphereNonAlloc(transform.position + groundCheckPosition, groundCheckRadius, grundCheckHits, groundLayers) != 0;
        canDash = true;
    }

    private void ApplyGravityMultiplier()
    {
        myRigidbody.velocity += Vector3.down * gravityMultiplier;
    }

    #region Movement
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void Movement()
    {
        if (isDashing) { return; }

        float targetSpeed = (movementInput.magnitude < Mathf.Epsilon) ? 0 : topSpeed;
        float t = (targetSpeed == 0) ? deacceleration : acceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, t * Time.fixedDeltaTime);

        Vector3 movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 movementVelocity = (movementDirection * currentSpeed) + (Vector3.up * myRigidbody.velocity.y);
        myRigidbody.velocity = movementVelocity;
    }
    #endregion

    #region Jumping
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (activeJumpBuffer != null) { StopCoroutine(activeJumpBuffer); }
            activeJumpBuffer = StartCoroutine(JumpBufferRoutine());
        }
        else
        {
            StartCoroutine(CancelJumpRoutine());
        }
    }

    private void Jump()
    {
      
        isJumping = true;
        coyoteTimeCounter = coyoteTime;
        myRigidbody.velocity -= Vector3.up * myRigidbody.velocity.y;
        myRigidbody.AddForce(jumpForce * Vector3.up, ForceMode.VelocityChange);

        //Trigger squash and stretch animation and particle effect
        characterAnimator.SetTrigger("squashJump");
    }

    private IEnumerator JumpBufferRoutine()
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

    private IEnumerator CancelJumpRoutine()
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
    private void OnDash(InputValue value)
    {
        if (!value.isPressed || !canDash || isDashing) { return; }

        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        myRigidbody.isKinematic = true;
        yield return new WaitForSecondsRealtime(0.15f);
        myRigidbody.isKinematic = false;

        myRigidbody.velocity = Vector3.zero;
        myRigidbody.velocity = (transform.forward * dashForce.x) + (transform.up * dashForce.y);

        for (float t = 0; t < dashDuration; t += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            if (t < minDashDuration) { continue; }
           
            if (isGrounded) { break; }
        }

        isDashing = false;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckPosition, groundCheckRadius);
    }

    #region Animation updates from Update
    private void UpdateAnimationStates() {
        //The "animation update - code" needs refactoring. This is not a priority.
        if (isDashing) {
            characterAnimator.SetBool("isDashing", true);
        } else {
            characterAnimator.SetBool("isDashing", false);
        }

        if (isGrounded) {
            characterAnimator.SetBool("isGrounded", true);
            if(!landingLock && fallingTimer > 0.3f) {
                landingLock = true;
                characterAnimator.SetTrigger("landed");
            }

            fallingTimer = 0;
            
        }   else {
            characterAnimator.SetBool("isGrounded", false);
            landingLock = false;
            fallingTimer += Time.deltaTime;
        }

        if(movementInput != Vector2.zero && isGrounded) {
            characterAnimator.SetBool("isRunning", true);
        }   else {
            characterAnimator.SetBool("isRunning", false);
        }


        if(isGrounded) {
            characterAnimator.SetBool("isFalling", false);
            characterAnimator.SetBool("isJumpingUp", false);
            return;
        }

        if(myRigidbody.velocity.y > 1) {
            characterAnimator.SetBool("isJumpingUp", true);
        }   else {
            characterAnimator.SetBool("isJumpingUp", false);
        }
      
        if (myRigidbody.velocity.y < 0) {
            characterAnimator.SetBool("isFalling", true);
        }   else {
            characterAnimator.SetBool("isFalling", false);
        }

        
    }

    #endregion
}
