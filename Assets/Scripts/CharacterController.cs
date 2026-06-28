using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 5.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 8.5f;

    private Animator animator;
    
    private GroundChecker groundChecker;
    private Rigidbody rb;
    private float currentRotationY = 0f;

    
    
    
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<GroundChecker>();
    }

    private void Update()
    {
        HandleInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }


    
    
    
    
    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && groundChecker.isGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }

        if (Input.GetButton("Fire1"))
        {
            animator.SetInteger("Attack", 1);
        }
        else if (Input.GetButton("Fire2"))
        {
            animator.SetInteger("Attack", 2);
        }
        else
        {
            animator.SetInteger("Attack", 0);
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        currentRotationY += horizontal * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);

        Vector3 direction = transform.forward * vertical * speed;
        direction.y = rb.linearVelocity.y;

        rb.linearVelocity = direction;
    }

    private void HandleAnimation()
    {
        bool isGroundCheck = groundChecker.isGround;
        
        float horizontal = Mathf.Abs(Input.GetAxis("Horizontal"));
        float vertical = Mathf.Abs(Input.GetAxis("Vertical"));
        
        animator.SetFloat("Speed", horizontal + vertical);
    }
}

