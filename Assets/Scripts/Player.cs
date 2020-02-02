using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Movement")]
    public Rigidbody playerRigidbody;
    public float speed = 4.5f;
    private Vector3 inputDirection;
    private Vector3 movement;

    [Header("Other")]
    public GameManager gm;

    private InputMaster imaster;
    private Vector2 moveDir;
    private Vector2 moveDirInput;
    private Vector2 lookDir;

    private float setTimer;
    private float tickTimer;
    private Vector3 desiredDirection;
    private Animator m_Animator;
    float h;
    float v;
    [HideInInspector]
    public bool wantsToPickUp;
    private List<GameObject> closeObjs;

    private Vector3 jump;
    private float jumpForce = 2.5f;

    public bool isGrounded = true;
    Rigidbody rb;

    void Awake()
    {
        imaster = new InputMaster();
        // imaster.Player.Movement.performed += ctx => moveDir=ctx.ReadValue<Vector2>();
        // imaster.Player.Movement.performed += ctx => lookDir = ctx.ReadValue<Vector2>();
        // imaster.Player.Movement.canceled += ctx => StopMovement();


        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        mainCamera.gameObject.transform.parent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        if(gameObject.CompareTag("Player1"))
        {
            speed = 5.5f;
        }

        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

        h = moveDir.x;
        v = moveDir.y;

        if (h != 0)
        {
            m_Animator.SetBool("Move", true);
        }
        else if (h == 0 && v == 0)
        {
            m_Animator.SetBool("Move", false);
        }

        CheckControler();
    }

    void CheckControler()
    {
        if(setTimer>=0)
        {
            setTimer -= Time.deltaTime;
        }
        else
        {
            moveDir = new Vector2(0,0);
        }
    }


    private void FixedUpdate()
    {
       
        Vector3 targetInput = new Vector3(h, 0, v);
        inputDirection = Vector3.Lerp(inputDirection, targetInput, Time.deltaTime * 10f);

        //Camera Direction
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        //Try not to use var for roadshows or learning code
        Vector3 desiredDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

        MoveThePlayer(desiredDirection);
        TurnThePlayer();


    }

 

    void StopMovement()
    {
        moveDir = new Vector2(0, 0);
    }


    void MoveThePlayer(Vector3 desiredDirection)
    {
        movement.Set(desiredDirection.x, 0f, desiredDirection.z);

        movement = movement * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);

    }

    void TurnThePlayer()
    {
        Vector2 input = lookDir;

        // Convert "input" to a Vector3 where the Y axis will be used as the Z axis
        Vector3 lookDirection = new Vector3(input.x, 0,input.y);
        Vector3 lookRot = mainCamera.transform.TransformDirection(lookDirection);
        lookRot = Vector3.ProjectOnPlane(lookRot, Vector3.up);

        if (lookRot != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(lookRot);
            playerRigidbody.MoveRotation(newRotation);
        }
    }


    void OnMovement(InputValue _value)
    {
        if(gm.startGame)
        {
            moveDir = _value.Get<Vector2>();
            lookDir = _value.Get<Vector2>();

            setTimer = 0.4f;
        }
     
    }

    void OnPickUp()
    {
        if(this.gameObject.CompareTag("Player2"))
        {
            if(!gm.objectPickedUp)
            {
                //Pick it up
                gm.PlayerPickUpDetected();
            }
            else
            {
                //Drop
                GetComponent<TestPlayerMovement>().CloseToTransObj();
            }
        }
    }

    void OnDrop()
    {
        gm.DropObject();
    }

    void OnEscape()
    {
        Debug.Log("Escaped");
        Application.Quit();
    }

    void OnBark()
    {
        if(gameObject.CompareTag("Player1"))
        {
            m_Animator.SetTrigger("Bark");
        }
        
    }

    void OnJump()
    {
       
        if (gameObject.CompareTag("Player1") && isGrounded)
        {          
            Debug.Log("doggo jumped");
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("being called 0");
        if (other.gameObject.tag == "Floor" && !isGrounded)
        {
            Debug.Log("being called");
            isGrounded = true;
        }
            
    }


}
