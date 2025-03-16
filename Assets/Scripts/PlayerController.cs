using System;
using Unity.Cinemachine;
using UnityEditor.PackageManager.UI;
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

    [SerializeField, Range(0, 90f)] private float maxWheelTurn = 45f;
    [SerializeField, Range(0, 100f)] private float wheelTurnSpeed = 13.5f;
    [SerializeField, Range(0, 100f)] private float wheelTurnToTurnThreshhold = 1f;

    private Vector3 velocity, desiredVelocity;
    private float wheelTurn = 0.0f;

    bool onGround;

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;
    private CinemachineCamera virtualCamera;



    void Awake()
    {
        body  = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        virtualCamera = FindFirstObjectByType<CinemachineCamera>();

        if(virtualCamera == null)
        {
            Debug.LogWarning("No virtual camera found");
        }
    
    }
    
    void Start()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        virtualCamera.transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        desiredVelocity = new Vector3(0, 0, defaultVelocity + playerInput.y);

        if (Mathf.Abs(wheelTurn) > wheelTurnToTurnThreshhold)
            desiredVelocity.x = Mathf.Sign(wheelTurn) * 
                1.5f * (Mathf.Abs(wheelTurn) - wheelTurnToTurnThreshhold) / (maxWheelTurn - wheelTurnToTurnThreshhold);

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        if (Mathf.Abs(playerInput.x) > 0.5f) {
            float opposingBonus = wheelTurn * playerInput.x < 0 ? Mathf.Max(Mathf.Abs(wheelTurn) / wheelTurnSpeed, 1.0f) : 1.0f;

            wheelTurn += Time.deltaTime * (playerInput.x * wheelTurnSpeed * opposingBonus);
            wheelTurn = Mathf.Clamp(wheelTurn, -maxWheelTurn, maxWheelTurn);
        } else {
            wheelTurn += Mathf.Max(-wheelTurn, Time.deltaTime * -3 * wheelTurn);
            if (Mathf.Abs(wheelTurn) < 4.0f) 
                wheelTurn = 0.0f;
        }

        float yaw = wheelTurn * 0.6f;
        Quaternion targetRotation = Quaternion.Euler(0, yaw, 0);
        transform.localRotation = targetRotation;


        if (virtualCamera != null)
        {
            Vector3 mPosNormal = Input.mousePosition / new Vector2(Screen.width, Screen.height);

            Debug.Log("MP: " + mPosNormal);
            Debug.Log("Turn: " + wheelTurn);

            float glanceYaw = Mathf.Clamp(mPosNormal.x * 90, -82, 82);
            float glancePitch = Mathf.Clamp(-mPosNormal.y * 90, -20, 0);

            Quaternion rotation = Quaternion.Euler(glancePitch, glanceYaw + yaw, 0);
            virtualCamera.transform.rotation = rotation;
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
}
