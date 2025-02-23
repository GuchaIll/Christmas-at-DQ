using UnityEngine;

public class CarControllerMenu : MonoBehaviour
{
   
    
    [SerializeField, Range(0f, 500f)]
    float maxSpeed = 10f;
    
    [SerializeField, Range(0f, 500f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    //[SerializeField, Range(0f, 1f)]
    //float bounciness = 0.5f;

    //[SerializeField]
    //Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);   


    private Vector3 velocity, desiredVelocity;
    public Rigidbody body;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    int jumpPhase;

    bool desiredJump;
    bool onGround;


    void Awake() {
        body = GetComponent<Rigidbody>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        desiredJump |= Input.GetButtonDown("Jump");

        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
       
        
        
    }

    void FixedUpdate()
    {
       
       
        velocity = body.linearVelocity;
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        
        
        UpdateState();
    
        body.linearVelocity = velocity;

        onGround = false;
    }

    void UpdateState()
    {
        if(desiredJump ){
            desiredJump = false;
            if(onGround)
            {
                
                jumpPhase = 0;
               
            }
           Jump();
        }
    }

    void Jump()
    {
        if(onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if(velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision (Collision collision)
    {
        for(int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}

