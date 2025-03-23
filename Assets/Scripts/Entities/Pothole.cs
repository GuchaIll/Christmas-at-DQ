using UnityEngine;

public class Pothole : MonoBehaviour
{
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has a Rigidbody (optional, to ensure it's a physical object)
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (triggered == false && rb != null && other.CompareTag("Player"))
        {
            // Turn the object upside down by rotating it 180 degrees around its local x-axis
            PlayerController player = other.GetComponent<PlayerController>();
            if(player)
                player.toggleParkMode(true);
                other.transform.Rotate(180f, 0f, 0f, Space.Self);
                

            Debug.Log($"{other.name} has been turned upside down by the pothole!");
            triggered = true;
        }
    }
}