using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

   [SerializeField, Range(0, 100)] private float defaultVelocity = 5f;
    [SerializeField, Range(0, 100)] private float maxAcceleration = 10f;
    [SerializeField, Range(0, 100)] private float maxSpeed = 10f;

    [SerializeField, Range(0, 100)] private float maxTurnSpeed = 5f;

    [SerializeField, Range(0, 100)] private float stepHeight = 0.5f;

    [SerializeField, Range(0, 90)] private float rollAngle = 30f;

    [SerializeField, Range(0, 20)] private float rollSpeed = 10f;

    [SerializeField, Range(0, 100f)] private float driftSpeed = 15f;
    private Vector3 velocity, desiredVelocity;

    bool onGround;

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;
    [SerializeField] private CinemachineCamera virtualCamera;

    private bool visibility;

    void Awake()
    {
        body  = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
      
        if(virtualCamera == null)
        {
            Debug.LogWarning("No virtual camera found");
        }
    
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        desiredVelocity = new Vector3(0, 0, defaultVelocity);
        desiredVelocity += new Vector3(playerInput.x, 0, playerInput.y).normalized * maxSpeed;
        
        if (virtualCamera != null)
        {
            float targetRollAngle = playerInput.x * rollAngle;
            float currentRollAngle = transform.localEulerAngles.z;
            float newRollAngle = Mathf.MoveTowardsAngle(currentRollAngle, targetRollAngle, rollSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Euler(0, 0, newRollAngle);
            transform.localRotation = targetRotation;
        }
    }

    void Accelerate()
    {
        Vector3 velocityChange = desiredVelocity - velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxAcceleration, maxAcceleration);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxAcceleration, maxAcceleration);
        velocityChange.y = 0;

        body.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
       velocity = body.linearVelocity;
       float maxSpeedChange = maxAcceleration * Time.deltaTime;
       float maxTurnSpeedChange = maxTurnSpeed * Time.deltaTime;
       velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxTurnSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        body.linearVelocity = velocity;

        HandleStepClimbing();
    
    }

    void HandleStepClimbing()
    {
        RaycastHit hitLower;
        RaycastHit hitUpper;

        if(Physics.Raycast(transform.position, transform.forward, out hitLower, capsuleCollider.radius + 0.1f))
        {
            if(!Physics.Raycast(transform.position + Vector3.up * stepHeight, transform.forward, out hitUpper, capsuleCollider.radius + 0.1f))
            {
                RaycastHit hitGround;
                if(Physics.Raycast(transform.position + Vector3.up * stepHeight + transform.forward * (capsuleCollider.radius + 0.5f), Vector3.down, out hitGround, stepHeight))
                {
                    body.position = new Vector3(body.position.x, hitGround.point.y, body.position.z);
                }
            }
        }
        
    }
    public bool getVisibility()
    {
        return visibility;
    }

    public void setVisibility(bool value)
    {
        visibility = value;
    }

    public void toggleParkMode(bool park)
    {
        if(park)
        {
            body.linearVelocity = Vector3.zero;
            defaultVelocity = 0;
        }
        else{
            defaultVelocity = 5f;
        }

    }
}
