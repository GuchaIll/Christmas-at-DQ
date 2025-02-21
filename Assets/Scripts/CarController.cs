using Unity.Cinemachine;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField, Range(0, 100)] private float cruiseSpeed = 3f;
    [SerializeField, Range(0, 100)] private float acceleration = 10f;
    [SerializeField, Range(0, 100)] private float maxSpeed = 10f;

    [SerializeField, Range(0, 100)] private float maxTurnSpeed = 5f;

    [SerializeField, Range(0, 100)] private float stepHeight = 0.5f;

    [SerializeField, Range(0, 90)] private float rollAngle = 30f;

    [SerializeField, Range(0, 20)] private float rollSpeed = 10f;

    [SerializeField, Range(0, 100f)] private float driftSpeed = 15f;

    [SerializeField, Range(0, 3f)] private float panicMeter = 0f;

    public float wheelAngle;
    private Vector2 flat_velocity = new Vector2(0, 0);
    private Vector3 velocity = new Vector3(0, 0, 0);

    private Rigidbody body;
    private CapsuleCollider capsuleCollider;
    private CinemachineCamera virtualCamera;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        virtualCamera = FindFirstObjectByType<CinemachineCamera>();

        if (virtualCamera == null)
        {
            Debug.LogWarning("No virtual camera found");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        flat_velocity = new Vector2 (0, 0);
    }

    // Update is called once per frame
    void Update() {
        float acc = Input.GetAxis("Vertical");
        float turning = Input.GetAxis("Horizontal");

        if (acceleration > 0) {
            flat_velocity.y += acceleration * acc * Time.deltaTime;
            if (flat_velocity.magnitude > maxSpeed) {
                flat_velocity = flat_velocity.normalized * maxSpeed;
            }
        }
        else {
            flat_velocity.y = flat_velocity.y - (flat_velocity.y - )
        }
    }
}
