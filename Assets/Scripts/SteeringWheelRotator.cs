using UnityEngine;

public class SteeringWheelRotator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private float rotationMultiplier = 20f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float rotationAngle = horizontalInput * rotationMultiplier * Time.deltaTime;

        // Apply the rotation to the steering wheel
        transform.Rotate(Vector3.up, -rotationAngle);

    }
}
