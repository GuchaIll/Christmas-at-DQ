using UnityEngine;

public class CameraRotation : MonoBehaviour {

    public float rotationSpeed = 100f; // Speed of rotation
    public float maxPitch = 60f; // Maximum pitch angle
    public float minPitch = -60f; // Minimum pitch angle
    public float dampingSpeed = 8f; // Speed of damping (higher = faster)

    public float maxYaw = 90f;
    public float minYaw = -90f;
    private float targetPitch; // Target pitch (vertical rotation)
    private float targetYaw; // Target yaw (horizontal rotation)
    private float currentPitch; // Current pitch (used for damping)
    private float currentYaw; // Current yaw (used for damping)

    void Update()
    {
        // Get input for horizontal and vertical rotation

        if(Input.GetMouseButton(0))
        {
           
        
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");
        
        if(Mathf.Abs(currentYaw) <= 30 && Mathf.Abs(currentYaw) <= 30)
        {
            dampingSpeed = 3f;
        }
        else{
            dampingSpeed = 8f;
        }

        // Calculate target rotation based on input
        targetYaw += horizontalInput * rotationSpeed * Time.deltaTime;
        targetPitch -= verticalInput * rotationSpeed * Time.deltaTime;

       
        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);
        targetYaw = Mathf.Clamp(targetYaw, minYaw, maxYaw);

        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * dampingSpeed);
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * dampingSpeed);

        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
    }
    }
}