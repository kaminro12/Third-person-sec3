using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playervelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpheight = 2.5f;

    public Animator animator;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Victory.Instance.isWinner){
            case true:
                animator.SetBool("Victory",Victory.Instance.isWinner); 
                break;
            case false:
                Movement();
                break;
        }
       
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if (characterController.isGrounded && playervelocity.y < -2f)
        {
            playervelocity.y = -1f;
        }

        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3 (HorizontalInput, 0, VerticalInput);

        Vector3 movementDirection = movementInput.normalized;

        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        if(movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playervelocity.y += Mathf.Sqrt(jumpheight * -3f * gravityValue);
            animator.SetTrigger("Jumping");
        }

        playervelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playervelocity * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);
    }
}
